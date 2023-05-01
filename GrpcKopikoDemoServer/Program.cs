using Calzolari.Grpc.AspNetCore.Validation;
using GrpcKopikoDemoServer.Data;
using GrpcKopikoDemoServer.DataModelServices.OrderDetailsService;
using GrpcKopikoDemoServer.DataModelServices.OrderModelService;
using GrpcKopikoDemoServer.DataModelServices.ProductModelService;
using GrpcKopikoDemoServer.DataModelServices.UserModelService;
using GrpcKopikoDemoServer.Interceptors.AuthInterceptors;
using GrpcKopikoDemoServer.Interfaces;
using GrpcKopikoDemoServer.RequestValidators.AuthValidators;
using GrpcKopikoDemoServer.RequestValidators.UserOrderValidators;
using GrpcKopikoDemoServer.Services;
using GrpcKopikoDemoServer.ServicesHelpers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace GrpcKopikoDemoServer
{
	public class Program
	{
		public static void Main(string[] args)
		{
			var builder = WebApplication.CreateBuilder(args);

			// Set database context 
			builder.Services.AddDbContext<DatabaseContext>(options => options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

			// Inject Token Helper
			builder.Services.AddScoped(typeof(ITokenServiceHelper), typeof(TokenServiceHelper));

			// Add services to the container.

			// Authentication
			builder.Services.AddAuthentication(options =>
			{
				options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
				options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
			}).AddJwtBearer(options =>
			{
				options.RequireHttpsMetadata = false;
				options.SaveToken = true;
				options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
				{
					ValidateIssuerSigningKey = true,
					IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Environment.GetEnvironmentVariable("JWT_SECRET"))),
					ValidateIssuer = false,
					ValidateAudience = false,
				};
			});

			builder.Services.AddAuthorization();

			// Set Grpc options and add access token interceptor
			builder.Services.AddGrpc(options => { options.EnableMessageValidation(); })							
							.AddServiceOptions<UserActionService>(options => { options.Interceptors.Add<AccessTokenInterceptor>(); })
							.AddServiceOptions<UserOrderService>(options => { options.Interceptors.Add<AccessTokenInterceptor>(); });
			

			// Set validators 
			builder.Services.AddValidator<RegisterRequestValidator>();
			builder.Services.AddValidator<RefreshTokenRequestValidator>();
			builder.Services.AddValidator<LoginRequestValidator>();
			builder.Services.AddValidator<CreateOrderValidator>();
			builder.Services.AddValidators();
			builder.Services.AddGrpcValidation();

			// Model services injections 
			builder.Services.AddScoped(typeof(IUserService), typeof(UserService));
			builder.Services.AddScoped(typeof(IProductService), typeof(ProductService));
			builder.Services.AddScoped(typeof(IOrderDetailsService), typeof(OrderDetailsService));
			builder.Services.AddScoped(typeof(IOrderService), typeof(OrderService));

			// Set AutoMapper 
			builder.Services.AddAutoMapper(typeof(Program));

			var app = builder.Build();

			// **ONLY FOR TESTING PURPOSES**
			// Populates Products talbe with data
			DBTablesInit.InitProducts(app);

			app.MapGrpcService<AuthService>();
			app.MapGrpcService<UserActionService>();
			app.MapGrpcService<ProductItemService>();
			app.MapGrpcService<UserOrderService>();
			app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

			app.Run();
		}
	}
}