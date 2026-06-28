using DbUp;
using DbUp.Engine;
using Serilog;
using System.Text;
using System.Text.RegularExpressions;

namespace DatabaseMigrator;

public static class Program
{
    private static readonly Regex TimestampRegex = new(@"^(\d{14})", RegexOptions.Compiled);

    private static readonly (string Path, string Name)[] Stages =
    [
        ("Security/Schemas", "Schemas"),
        ("Tables", "Tables"),
        ("Seeds", "Seeds"),
    ];

    public static int Main(string[] args)
    {
        Log.Logger = new LoggerConfiguration().WriteTo.Console().CreateLogger();

        try
        {
            if (!TryParseOptions(args, out var skipConfirmation, out var positionalArgs))
                return 2;

            if (!TryResolveArguments(positionalArgs, out var server, out var database, out var userId, out var password))
                return 2;

            if (!skipConfirmation && !ConfirmRunMigrations(server, database))
            {
                Log.Information("Migration skipped by user");
                return 0;
            }

            var connectionString = BuildConnectionString(server, database, userId, password);
            var scriptsPath = Path.Combine(AppContext.BaseDirectory, "MeuBanco");

            if (!Directory.Exists(scriptsPath))
            {
                Log.Error("Scripts folder not found: {Path}", scriptsPath);
                return 1;
            }

            Log.Information("Starting migration for database {Database} on {Server}", database, server);

            if (!EnsureDatabaseExists(connectionString, server, database))
                return 1;

            foreach (var (relativePath, displayName) in Stages)
            {
                if (!ExecuteStage(connectionString, scriptsPath, relativePath, displayName))
                    return 1;
            }

            Log.Information("Migration completed successfully");
            return 0;
        }
        catch (Exception ex)
        {
            Log.Fatal(ex, "Unexpected error during migration");
            return 1;
        }
        finally
        {
            Log.CloseAndFlush();
        }
    }

    private static bool TryParseOptions(
        string[] args,
        out bool skipConfirmation,
        out string[] positionalArgs)
    {
        skipConfirmation = false;
        var positional = new List<string>();

        foreach (var arg in args)
        {
            if (arg is "--yes" or "-y")
            {
                skipConfirmation = true;
                continue;
            }

            positional.Add(arg);
        }

        positionalArgs = positional.ToArray();
        return true;
    }

    private static bool TryResolveArguments(
        string[] args,
        out string server,
        out string database,
        out string userId,
        out string password)
    {
        if (args.Length == 4)
        {
            server = args[0];
            database = args[1];
            userId = args[2];
            password = args[3];
            return true;
        }

        if (args.Length == 0)
        {
            Console.WriteLine("Enter connection details:");
            server = ReadRequired("Server");
            database = ReadRequired("Database");
            userId = ReadRequired("User");
            Console.Write("Password: ");
            password = ReadPassword();
            return true;
        }

        server = database = userId = password = string.Empty;
        PrintUsage();
        return false;
    }

    private static string ReadRequired(string label)
    {
        while (true)
        {
            Console.Write($"{label}: ");
            var value = Console.ReadLine()?.Trim();
            if (!string.IsNullOrWhiteSpace(value))
                return value;

            Console.WriteLine($"{label} is required.");
        }
    }

    private static string ReadPassword()
    {
        var password = new StringBuilder();

        while (true)
        {
            var key = Console.ReadKey(intercept: true);

            if (key.Key == ConsoleKey.Enter)
            {
                Console.WriteLine();
                break;
            }

            if (key.Key == ConsoleKey.Backspace)
            {
                if (password.Length > 0)
                    password.Length--;
                continue;
            }

            if (!char.IsControl(key.KeyChar))
                password.Append(key.KeyChar);
        }

        return password.ToString();
    }

    private static bool ConfirmRunMigrations(string server, string database)
    {
        while (true)
        {
            Console.Write($"Run migrations on {database}@{server}? (y/n): ");
            var answer = Console.ReadLine()?.Trim();

            if (string.IsNullOrWhiteSpace(answer))
                continue;

            if (answer.Equals("y", StringComparison.OrdinalIgnoreCase) ||
                answer.Equals("yes", StringComparison.OrdinalIgnoreCase))
                return true;

            if (answer.Equals("n", StringComparison.OrdinalIgnoreCase) ||
                answer.Equals("no", StringComparison.OrdinalIgnoreCase))
                return false;

            Console.WriteLine("Please answer y/yes or n/no.");
        }
    }

    private static bool EnsureDatabaseExists(string connectionString, string server, string database)
    {
        try
        {
            Log.Information("Checking database {Database} on {Server}...", database, server);
            EnsureDatabase.For.SqlDatabase(connectionString);
            Log.Information(
                "Database {Database} is ready. It was created automatically if it did not exist.",
                database);
            return true;
        }
        catch (Exception ex)
        {
            Log.Error(
                "Unable to connect to server {Server} or ensure database {Database}. " +
                "Check server availability, credentials, and whether the user can create databases.",
                server, database);
            Log.Error(ex, "Error details");
            return false;
        }
    }

    private static bool ExecuteStage(string connectionString, string scriptsPath, string relativePath, string displayName)
    {
        var stagePath = Path.Combine(scriptsPath, relativePath);

        if (!Directory.Exists(stagePath))
        {
            Log.Warning("Stage skipped ({Stage}): folder not found", displayName);
            return true;
        }

        var scripts = Directory
            .GetFiles(stagePath, "*.sql", SearchOption.AllDirectories)
            .Select(file => new
            {
                Name = Path.GetFileName(file),
                FullPath = file,
                Timestamp = ExtractTimestamp(Path.GetFileName(file)),
            })
            .OrderBy(script => script.Timestamp)
            .ThenBy(script => script.Name, StringComparer.Ordinal)
            .Select(script => new SqlScript(script.Name, File.ReadAllText(script.FullPath)))
            .ToList();

        if (scripts.Count == 0)
        {
            Log.Warning("Stage skipped ({Stage}): no scripts found", displayName);
            return true;
        }

        Log.Information("Running stage {Stage} ({Count} scripts)...", displayName, scripts.Count);

        var result = DeployChanges.To
            .SqlDatabase(connectionString)
            .WithScripts(scripts)
            .JournalToSqlTable("dbo", "MigrationsJournal")
            .LogTo(new SerilogUpgradeLog(Log.Logger))
            .Build()
            .PerformUpgrade();

        if (!result.Successful)
        {
            Log.Error(result.Error, "Stage {Stage} failed", displayName);
            return false;
        }

        Log.Information("Stage {Stage} completed", displayName);
        return true;
    }

    private static long ExtractTimestamp(string fileName)
    {
        var match = TimestampRegex.Match(fileName);
        return match.Success && long.TryParse(match.Groups[1].Value, out var timestamp) ? timestamp : long.MaxValue;
    }

    private static string BuildConnectionString(string server, string database, string userId, string password)
    {
        var builder = new StringBuilder();
        builder.Append($"Server={server};");
        builder.Append($"Database={database};");
        builder.Append($"User Id={userId};");
        builder.Append($"Password={password};");
        builder.Append("Encrypt=False;");
        builder.Append("TrustServerCertificate=True;");
        return builder.ToString();
    }

    private static void PrintUsage()
    {
        Console.WriteLine("""
            Usage:
              DatabaseMigrator [--yes|-y] <server> <database> <user> <password>

            Options:
              --yes, -y   Skip confirmation prompt (for CI/Docker)

            Without arguments, connection details will be requested interactively.
            You will be asked to confirm before migrations are executed.

            Execution order:
              1. Security/Schemas
              2. Tables
              3. Seeds
            """);
    }
}
