using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace MyAppServices
{
    public static class MyAuthorization
    {
        public static WebApplicationBuilder AddMyAuthorization(this WebApplicationBuilder builder)
        {
            builder.Services.AddAuthorization(opt =>
                {
                    opt.FallbackPolicy = new Microsoft.AspNetCore.Authorization.AuthorizationPolicyBuilder()
                        .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme)
                        .RequireAuthenticatedUser()
                        .Build();
                    opt.AddPolicy("Admin",
                        policy => policy.RequireRole("Admin"));
                }
            );
            return builder;
        }

        public static IApplicationBuilder UseMyAuthoriztion(this IApplicationBuilder app)
        {
            // app.UseAuthentication();
            app.UseAuthorization();
            return app;
        }
    }
}