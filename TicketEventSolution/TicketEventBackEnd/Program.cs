using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using MVC_DataAccess.Services.Customer;
using System.Text;
using TicketEventBackEnd.Repositories.Customer;
using System.Configuration;


var builder = WebApplication.CreateBuilder(args);
var config = builder.Configuration;
// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();
builder.Services.AddScoped<ICustomerServices, CustomerServices>();
//Allows the controller to recieve its dependency
//just make sure to go to program.cs to inject the dependency
/*
  services.AddControllersWithViews();
  // Register the repository with the dependency injection container

 */
/*
 Use AddAuthentication for Controller
 This is used for cookie based login and JWT bearer authentication
 */
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
})
   .AddJwtBearer(options =>
   {
       //Set https to false to allow retrieval of http
       options.RequireHttpsMetadata = false;
       //Allow saving tokens
       options.SaveToken = true;
       //set parameters for validating tokens
       options.TokenValidationParameters = new TokenValidationParameters
       {
           //validate the issuer given
           ValidateIssuer = true,
           //valide the audience given
           ValidateAudience = true,
           //validate the lifetime of a security token
           ValidateLifetime = true,

           ValidateIssuerSigningKey = true,
           //Check info in appsetting.json
           ValidIssuer = config["Jwt: Issuer"],
           ValidAudience = config["Jwt: Audience"],
           //A symmetric key is a single key used for signing and validating a token
           //Encoding will grab the secret key string and encode it from a string into bytes using UTF-8
           //Which will then be used as a symmetric security key
           IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["Jwt:SecretKey"]))
       };
   }
   );
var app = builder.Build();

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
