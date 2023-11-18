using ProductsMinimalAPI.AppServicesExtensions;
using ProductsMinimalAPI.Endpoints;

var builder = WebApplication.CreateBuilder(args);

/*** Services extensions ***/
builder.Services.AddCors();

builder.AddApiSwagger();
builder.AddPersistence();
builder.AddAuthenticationJwt();

var app = builder.Build();
var environment = app.Environment;

app.UseExceptionHandling(environment)
    .UseSwaggerMiddleware()
    .UseAppCors();

/*** Endpoints extensions ***/
app.MapLoginEndpoint();
app.MapCategoriesEndpoint();
app.MapProductsEndpoint();

app.UseAuthentication();
app.UseAuthorization();

app.Run();
