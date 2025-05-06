namespace ScopeMinimizedTokenizationDemo.Tokenization;

public interface ITokenizationService
{
    string Tokenize(string key, string value);
    string Detokenize(string token);
}

public class TokenizationService : ITokenizationService
{
    private readonly Dictionary<string, string> _tokenStore = new();

    public string Tokenize(string key, string value)
    {
        var token = Convert.ToBase64String(Guid.NewGuid().ToByteArray());
        token = token.Replace('+', '-').Replace('/', '_').Replace("=", "");
        _tokenStore[token] = $"{key}:{value}";
        return token;
    }

    public string Detokenize(string token)
    {
        //var val = _tokenStore[token];
        return _tokenStore.TryGetValue(token, out var value) ? value : null;
    }
}