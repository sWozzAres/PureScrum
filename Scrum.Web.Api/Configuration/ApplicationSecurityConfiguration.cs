using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;

namespace Scrum.Web.Api.Configuration;

public static class ApplicationSecurityConfiguration
{
    public static void AddApplicationSecurity(this IServiceCollection services)
    {
        JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

        services.AddAuthentication("Bearer")
            .AddJwtBearer("Bearer", options =>
            {
                options.Authority = "https://localhost:5000";
                options.Audience = "utopia";

                //options.MapInboundClaims = false;

                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateAudience = false,
                    //NameClaimType = "name",
                    //ClockSkew = TimeSpan.FromSeconds(5)
                };


            });

        services.AddAuthorizationBuilder()
            .AddPolicy("ClientPolicy", policy =>
            {
                policy.RequireAuthenticatedUser();
                policy.RequireClaim("scope", "purescrum.client");
            });
    }
}
