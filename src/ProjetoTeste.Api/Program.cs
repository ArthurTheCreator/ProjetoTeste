using ProjetoTeste.Api.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.ConfigureJson();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.ConfigureContext(builder.Configuration);
builder.Services.ConfigureInjectionDependency();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();