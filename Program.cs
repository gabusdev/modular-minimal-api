using MyAppServices;

var builder = WebApplication.CreateBuilder(args);
// Confiure AppServices
ModuleExtensions.RegisterModules(builder);
builder.AddMyAuth();
builder.AddMySwagger();

var app = builder.Build();
// Use AppServices
app.UseHttpsRedirection();
app.UseMySwagger();
app.UseMyAuth();
ModuleExtensions.MapEndpoints(app);

app.Run();
