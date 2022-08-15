using IdentityServer4_Samples.Configuration;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var services = builder.Services;

// Identity Server
var identityBuilder = services.AddIdentityServer()
              .AddInMemoryApiScopes(Config.ApiScopes)
              .AddInMemoryClients(Config.Clients);

identityBuilder.AddDeveloperSigningCredential();

var app = builder.Build();

app.UseIdentityServer();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
