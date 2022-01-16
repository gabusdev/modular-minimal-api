using Microsoft.OpenApi.Models;

namespace MyAppServices;
public static class MySwaggerModule
{
    public static WebApplicationBuilder AddMySwagger(this WebApplicationBuilder builder)
    {
        var SwaggerConfig = GetConfig(builder.Configuration);
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(opt =>
        {
            opt.SwaggerDoc("v1", new OpenApiInfo()
            {
                Description = SwaggerConfig.Description,
                // Title = "Minimal API Demo",
                Title = SwaggerConfig.Title,
                Version = SwaggerConfig.Version,
                Contact = new OpenApiContact()
                {
                    Name = SwaggerConfig.Contact_Name,
                    Url = new Uri(SwaggerConfig.Contact_Url)
                }
            });
            opt.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Name = "Authorization",
                Type = SecuritySchemeType.ApiKey,
                Scheme = "Bearer",
                BearerFormat = "JWT",
                In = ParameterLocation.Header,
                Description = "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer 1safsfsdfdfd\""
            });
            opt.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Id = "Bearer",
                            Type = ReferenceType.SecurityScheme
                        }
                    },
                    new List<string>()
                }
            });
        });

        return builder;
    }

    public static IApplicationBuilder UseMySwagger(this IApplicationBuilder app)
    {
        app.UseSwagger();
        app.UseSwaggerUI();
        return app;
    }

    private static MySwaggerConfig GetConfig(IConfiguration config)
    {
        return new MySwaggerConfig
        {
            Title = config["MySwagger:Title"] ?? "Default Title",
            Description = config["MySwagger:Description"] ?? "Default API Demo",
            Version = config["MySwagger:Version"] ?? "v1",
            Contact_Name = config["MySwagger:Contact_Name"] ?? "Default Name",
            Contact_Url = config["MySwagger:Contact_Url"] ?? "https://github.com/gabusdev"
        };
    }

    private class MySwaggerConfig
    {
        public string Title { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string Version { get; set; } = null!;
        public string Contact_Name { get; set; } = null!;
        public string Contact_Url { get; set; } = null!;
    }
}