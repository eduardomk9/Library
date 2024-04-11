
using Application.Business;
using Core.Business;
using Core.Repositories;
using Core.Services;
using Infrastructure.Persistence.Contexts;
using Infrastructure.Persistence.Repositories;
using Infrastructure.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.OData;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OData.ModelBuilder;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Reflection;
using System.Text;
using System.Text.Json.Serialization;
using WebApi.Controllers;
using WebAPI.Authentication;
using WebAPI.Controllers;

namespace WebAPI.Configurations
{
    public class DependencyInjectionConfig
    {
        public static void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {
            #region Configure Services     

            //CryptoUtils.Initialize(configuration);

            services.AddDbContext<GenericEnterpriseContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("GenericEnterpise")));


            // Configuração da autenticação personalizada como esquema padrão
            services.AddAuthentication("CustomAuth")
                .AddScheme<CustomAuthenticationSchemeOptions, CustomAuthenticationHandler>("CustomAuth", options => { });
            // Configuração da autorização com a política Bearer usando JWT e autenticação personalizada
            services.AddAuthorizationBuilder()
                .AddPolicy("Bearer", policy =>
                {
                    policy.AuthenticationSchemes.Add("CustomAuth"); // Adicionando o esquema personalizado
                    policy.AuthenticationSchemes.Add(JwtBearerDefaults.AuthenticationScheme);
                    policy.RequireAuthenticatedUser();
                });

            // Configuração da autenticação JWT
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = configuration["Jwt:Issuer"],
                    ValidAudience = configuration["Jwt:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"] ?? ""))
                };
            });

            ODataConventionModelBuilder modelBuilder = new();

            //OData
            services.AddControllers().AddOData(
                options => options.EnableQueryFeatures(null).AddRouteComponents(
                    routePrefix: "odata",
                    model: modelBuilder.GetEdmModel())).AddJsonOptions(options =>
                    {
                        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
                    });


            //Api Endpoints Configuration
            services.AddEndpointsApiExplorer();

            //Swagger
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Clients API",
                    Description = "Developed by Francisco Holanda",
                    Contact = new OpenApiContact { Name = "Francisco", Email = "eduardoeprf@hotmail.com" },
                    License = new OpenApiLicense { Name = "Francisco", Url = new Uri("https://www.linkedin.com/in/francisco-eduardo-machado-de-holanda-244a22171/") }
                });


                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    // Configuração do esquema JWT
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer",
                    BearerFormat = "JWT",
                    Description = "Insira o token JWT nesta caixa de texto",
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        Array.Empty<string>()
                    }
                });

                string xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                string xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
                c.EnableAnnotations();

                // Define o predicado de inclusão dos controladores
                c.DocInclusionPredicate((docName, apiDesc) =>
                {
                    if (!apiDesc.TryGetMethodInfo(out MethodInfo methodInfo))
                    {
                        return false;
                    }

                    // Lista de controladores que você deseja incluir no Swagger
                    Type[] includedControllers =
                    [
                     typeof(AuthController),
                     typeof(BookController)
                    ];
                    // Verifica se o controlador atual está na lista de controladores incluídos
                    bool isControllerIncluded = includedControllers.Contains(methodInfo.DeclaringType);
                    // Retorna true se o controlador estiver incluído, caso contrário, retorna false
                    return isControllerIncluded;
                });

            });

            //CORS
            services.AddCors(options =>
            {
                options.AddPolicy("CustomPolicy", builder =>
                {
                    string[]? corsOrigins = configuration.GetSection("Settings:CorsOrigins").Get<string[]>();

                    if (corsOrigins != null)
                    {
                        builder.WithOrigins(corsOrigins)
                            .AllowAnyMethod()
                            .AllowAnyHeader()
                            .AllowCredentials();
                    }
                });
            });

            services.AddHttpContextAccessor();

            //Repository
            services.AddScoped(typeof(IGenericEnterpriseRepository<>), typeof(GenericEnterpriseRepository<>));
            //Services       
            services.AddScoped<IAuthService, AuthService>();

            //Business           
            services.AddScoped<IAuthBusiness, AuthBusiness>();
            services.AddScoped<IBookBusiness, BookBusiness>();

            //Configs        
            services.AddAutoMapper(typeof(AutoMapperConfig).Assembly);
            #endregion
        }
    }
}