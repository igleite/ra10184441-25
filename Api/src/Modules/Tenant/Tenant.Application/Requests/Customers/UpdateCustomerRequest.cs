namespace Tenant.Application.Requests.Customers;

public record UpdateCustomerRequest(string Name, string Document, byte[] RowVersion);