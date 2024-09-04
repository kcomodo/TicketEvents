using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using TicketEventBackEnd.Services.Customer;
using System.Text;
using TicketEventBackEnd.Repositories.Customer;
using System.Configuration;
using Microsoft.OpenApi.Models;
using MVC_DataAccess.Repositories.Admin;
using MVC_DataAccess.Services.Admin;


var builder = WebApplication.CreateBuilder(args);
var config = builder.Configuration;
// Add services to the container.

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();
builder.Services.AddScoped<ICustomerServices, CustomerServices>();
builder.Services.AddScoped<IAdminRepository, AdminRepository>();
builder.Services.AddScoped<IAdminServices, AdminServices>();
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
       options.IncludeErrorDetails = true;
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
           ValidateAudience = false,
           //validate the lifetime of a security token
           ValidateLifetime = true,

           ValidateIssuerSigningKey = true,
           //Check info in appsetting.json
           ValidIssuer = config["Jwt:Issuer"],
           ValidAudience = config["Jwt:Audience"],
           //A symmetric key is a single key used for signing and validating a token
           //Encoding will grab the secret key string and encode it from a string into bytes using UTF-8
           //Which will then be used as a symmetric security key
           IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["Jwt:SecretKey"])),
       };
   }
   );
//add some info for the swagger api
builder.Services.AddSwaggerGen(c =>
    {

        //Give it a name and add a version
        c.SwaggerDoc("v1", new OpenApiInfo {Title = "Ticket Event Backend", Version="v1" });

        //Create bearer token which is used to access authorized resources
        //Bearer token can make requests to the server on behalf of the user
        c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
        {
            Name= "Authorization",
            //make the bearer token an apikey type
            Type=SecuritySchemeType.ApiKey,
            Scheme = "Bearer",
            BearerFormat="JWT",
            In = ParameterLocation.Header,
            Description = "Enter 'Bearer' [space] and then your valid token in the text input below.\n\nExample: \"Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9\"",
        });
        //Add a security requirement when using [Authorized]
        //Inside the security create a collection of requirements 
        //The type will be a security scheme
        //Make the array empty since this will indicate that there are no specific scopes to fullfil the requirement
        //It just needs a valid Bearer token 
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
    });
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigin",
        builder =>
        {
            builder.WithOrigins("http://localhost:4200") // Allow requests from this origin
                   .AllowAnyMethod()  // Allow any HTTP method (GET, POST, PUT, etc.)
                   .AllowAnyHeader(); // Allow any HTTP headers
        });
});
builder.Services.AddControllers();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Ticket Event Backend");
        c.RoutePrefix = "swagger";
    });
}
app.UseAuthentication();
app.UseAuthorization();
app.UseHttpsRedirection();

app.UseCors("AllowSpecificOrigin"); // Apply CORS policy globally
app.MapControllers();

app.Run();
