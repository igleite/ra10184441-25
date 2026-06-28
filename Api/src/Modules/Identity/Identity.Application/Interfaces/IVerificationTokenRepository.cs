using Identity.Domain.Entities;

namespace Identity.Application.Interfaces;

public interface IVerificationTokenRepository
{
    Task<VerificationToken?> GetByTokenAsync(string token);
    Task<bool> CreateAsync(VerificationToken verificationToken);
    Task<bool> DeleteByTokenAsync(string token);
}
