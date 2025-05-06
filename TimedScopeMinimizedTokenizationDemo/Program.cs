using Microsoft.Extensions.Options;
using System.IdentityModel.Tokens.Jwt;
using TimedScopeMinimizedTokenizationDemo.Models;
using TimedScopeMinimizedTokenizationDemo.Tokenization;

var builder = WebApplication.CreateBuilder(args);

// Register the tokenization service
builder.Services.AddSingleton<ITokenizationService, TokenizationService>();
// Register the token cleanup service
builder.Services.AddHostedService<TokenCleanupService>();

// Bind TokenSettings from appsettings.json
builder.Services.Configure<TokenSettings>(builder.Configuration.GetSection("TokenSettings"));


var app = builder.Build();



app.MapGet("/", () => "Hello World!");


#region Use request body
//app.MapPost("/tokenize", (ITokenizationService tokenizationService, CustomerData customerData) =>
//{
//    var tokenizedAccountNumber = tokenizationService.Tokenize(customerData.AccountNumber);
//    var tokenizedContractNumber = tokenizationService.Tokenize(customerData.ContractNumber);

//    return Results.Ok(new { TokenizedAccountNumber = tokenizedAccountNumber, TokenizedContractNumber = tokenizedContractNumber });
//}).RequireAuthorization();

////tokenized value to Jwt
//app.MapPost("/tokenizedJwt", (ITokenizationService tokenizationService, IOptions<TokenSettings> tokenSettings, CustomerData customerData) =>
//{
//    var tokenizedAccountNumber = tokenizationService.Tokenize(customerData.AccountNumber);
//    var tokenizedContractNumber = tokenizationService.Tokenize(customerData.ContractNumber);

//    var jwtToken = tokenizationService.GenerateJwtToken(tokenizedAccountNumber, tokenizedContractNumber);

//    return Results.Ok(new { JwtToken = jwtToken });
//}).RequireAuthorization();


////De-tokeniz value from Jwt
//app.MapPost("/detokenizedJwt", (ITokenizationService tokenizationService, JwtTokenData jwtTokenData) =>
//{
//    var tokenHandler = new JwtSecurityTokenHandler();
//    var jwtToken = tokenHandler.ReadJwtToken(jwtTokenData.JwtToken);

//    var tokenizedAccountNumber = jwtToken.Claims.First(c => c.Type == "accountNumber").Value;
//    var tokenizedContractNumber = jwtToken.Claims.First(c => c.Type == "contractNumber").Value;

//    var accountNumber = tokenizationService.Detokenize(tokenizedAccountNumber);
//    var contractNumber = tokenizationService.Detokenize(tokenizedContractNumber);

//    return Results.Ok(new { AccountNumber = accountNumber, ContractNumber = contractNumber });
//}).RequireAuthorization(); 
#endregion



app.MapGet("/tokenize", (ITokenizationService tokenizationService, string accountNumber, string contractNumber) =>
{
    var tokenizedAccountNumber = tokenizationService.Tokenize(accountNumber);
    var tokenizedContractNumber = tokenizationService.Tokenize(contractNumber);

    return Results.Ok(new { TokenizedAccountNumber = tokenizedAccountNumber, TokenizedContractNumber = tokenizedContractNumber });
});

app.MapGet("/tokenizedJwt", (ITokenizationService tokenizationService, IOptions<TokenSettings> tokenSettings, string accountNumber, string contractNumber) =>
{
    var tokenizedAccountNumber = tokenizationService.Tokenize(accountNumber);
    var tokenizedContractNumber = tokenizationService.Tokenize(contractNumber);

    var jwtToken = tokenizationService.GenerateJwtToken(tokenizedAccountNumber, tokenizedContractNumber);

    return Results.Ok(new { JwtToken = jwtToken });
});

app.MapGet("/detokenizedJwt", (ITokenizationService tokenizationService, string jwtToken) =>
{
    var tokenHandler = new JwtSecurityTokenHandler();
    var token = tokenHandler.ReadJwtToken(jwtToken);

    var tokenizedAccountNumber = token.Claims.First(c => c.Type == "accountNumber").Value;
    var tokenizedContractNumber = token.Claims.First(c => c.Type == "contractNumber").Value;

    var accountNumber = tokenizationService.Detokenize(tokenizedAccountNumber);
    var contractNumber = tokenizationService.Detokenize(tokenizedContractNumber);

    return Results.Ok(new { AccountNumber = accountNumber, ContractNumber = contractNumber });
});


app.Run();
