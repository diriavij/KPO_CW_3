var builder = WebApplication.CreateBuilder(args);
builder.WebHost.UseUrls("http://0.0.0.0:80");

builder.Services
    .AddReverseProxy()
    .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseSwagger(c =>
{
    c.RouteTemplate = "swagger/{documentName}/swagger.json";
});

app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/orders/swagger/v1/swagger.json",   "Orders API");
    c.SwaggerEndpoint("/payments/swagger/v1/swagger.json","Payments API");
    c.RoutePrefix = "swagger"; // UI на /swagger/index.html
});

app.MapReverseProxy();

app.Run();