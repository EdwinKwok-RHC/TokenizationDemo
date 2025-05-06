using ScopeMinimizedTokenizationDemo.Tokenization;

var builder = WebApplication.CreateBuilder(args);

// Register the tokenization service
builder.Services.AddSingleton<ITokenizationService, TokenizationService>();

var app = builder.Build();

app.MapGet("/", () => "Hello World!");


app.MapPost("/tokenize", (string contractAccount, string contractNumber, ITokenizationService tokenizationService) =>
{
    var token = tokenizationService.Tokenize("ContractAccount", contractAccount);
    var token2 = tokenizationService.Tokenize("ContractNumber", contractNumber);
    return Results.Ok(new { ContractAccountToken = token, ContractNumberToken = token2 });
});

app.MapPost("/detokenize", (string token, ITokenizationService tokenizationService) =>
{
    var value = tokenizationService.Detokenize(token);
    return value != null ? Results.Ok(new { Value = value }) : Results.NotFound();
});


app.Run();
