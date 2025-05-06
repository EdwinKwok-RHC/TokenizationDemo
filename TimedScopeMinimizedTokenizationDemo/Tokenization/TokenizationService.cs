using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace TimedScopeMinimizedTokenizationDemo.Tokenization;

public interface ITokenizationService
{
    string Tokenize(string sensitiveData);
    string Detokenize(string token);
    void CleanupExpiredTokens();
    string GenerateJwtToken(string tokenizedAccountNumber, string tokenizedContractNumber);
}

public class TokenizationService : ITokenizationService
{
    private readonly Dictionary<string, (string Data, DateTime Expiry)> _tokenStore = new();

    public string Tokenize(string sensitiveData)
    {
        var token = Guid.NewGuid().ToString();
        _tokenStore[token] = (sensitiveData, DateTime.UtcNow.AddHours(1)); // Token expires in 1 hour
        return token;
    }

    public string Tokenize(string sensitiveData, TimeSpan expiryTime)
    {
        var token = Guid.NewGuid().ToString();
        _tokenStore[token] = (sensitiveData, DateTime.UtcNow.Add(expiryTime));
        return token;
    }

    public string Detokenize(string token)
    {
        if (_tokenStore.TryGetValue(token, out var value) && value.Expiry > DateTime.UtcNow)
        {
            return value.Data;
        }
        return null;
    }

    public void CleanupExpiredTokens()
    {
        var expiredTokens = _tokenStore.Where(kvp => kvp.Value.Expiry <= DateTime.UtcNow).Select(kvp => kvp.Key).ToList();
        foreach (var token in expiredTokens)
        {
            _tokenStore.Remove(token);
        }
    }

    public string GenerateJwtToken(string tokenizedAccountNumber, string tokenizedContractNumber)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        
        // Use a key that's at least 32 bytes (256 bits) long
        var key = Encoding.ASCII.GetBytes("your-256-bit-secret-key-that-is-long-enough-for-hmacsha256");

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]
            {
                new Claim("accountNumber", tokenizedAccountNumber),
                new Claim("contractNumber", tokenizedContractNumber)
            }),
            Expires = DateTime.UtcNow.AddHours(1),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
}
