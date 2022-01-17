using MyAppServices;

var builder = WebApplication.CreateBuilder(args);
// Confiure AppServices
ModuleExtensions.RegisterModules(builder);
builder.AddMyJwtAuth();
builder.AddMyAuthorization();
builder.AddMySwagger();

var app = builder.Build();
// Use AppServices
app.UseHttpsRedirection();
app.UseMySwagger();
app.UseAuthentication();
app.UseAuthorization();
ModuleExtensions.MapEndpoints(app);

app.Run();
