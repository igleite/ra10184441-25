using System.Text;

namespace BuildingBlocks.Application.Helpers;

/// <summary>
/// Fornece métodos para codificação e decodificação de strings em um formato Base64 seguro para URLs.
/// A solução foi copiada de: <a href="https://stackoverflow.com/questions/11743160/how-do-i-encode-and-decode-a-base64-string">Stack Overflow</a>.
/// 
/// See also: <see href="https://stackoverflow.com/a/60738564"/>
/// </summary>
public static class Base64Url
{

    /// <summary>
    /// Codifica a string especificada em um formato Base64 seguro para URLs.
    /// <example>
    /// Exemplo:
    /// <code>
    /// string encoded = Base64Url.Encode("Hello, World!");
    /// Console.WriteLine(encoded); // Saída: SGVsbG8sIFdvcmxkIQ
    /// </code>
    /// </example>
    /// </summary>
    /// <param name="text">A string a ser codificada.</param>
    /// <returns>Uma string codificada em formato Base64 seguro para URLs.</returns>
    public static string Encode(string text)
    {
        try
        {
            if (string.IsNullOrEmpty(text)) return string.Empty;

            return Convert.ToBase64String(Encoding.UTF8.GetBytes(text)).TrimEnd('=').Replace('+', '-')
            .Replace('/', '_');
        }
        catch
        {
            return string.Empty;
        }
    }

    /// <summary>
    /// Decodifica a string codificada em formato Base64 seguro para URLs.
    /// <example>
    /// Exemplo:
    /// <code>
    /// string decoded = Base64Url.Decode("SGVsbG8sIFdvcmxkIQ");
    /// Console.WriteLine(decoded); // Saída: Hello, World!
    /// </code>
    /// </example>
    /// </summary>
    /// <param name="text">A string codificada a ser decodificada.</param>
    /// <returns>A string original decodificada.</returns>
    public static string Decode(string text)
    {
        try
        {
            if (string.IsNullOrEmpty(text)) return string.Empty;

            text = text.Replace('_', '/').Replace('-', '+');
            switch (text.Length % 4)
            {
                case 2:
                    text += "==";
                    break;
                case 3:
                    text += "=";
                    break;
            }
            return Encoding.UTF8.GetString(Convert.FromBase64String(text));
        }
        catch
        {
            return string.Empty;
        }
    }
}