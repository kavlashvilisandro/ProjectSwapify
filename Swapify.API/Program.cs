using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Swapify.API.BackgroundWorkers;
using Swapify.API.PipelineBehiaviour;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddSwaggerGen();
builder.Services.AddMediatR(configuration =>
{
    configuration.Lifetime = ServiceLifetime.Singleton;
    configuration.RegisterServicesFromAssembly(typeof(Program).Assembly);
});
builder.Services.AddHostedService<LablerWorker>();
builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ExceptionHandlerPipeline<,>));
builder.Services.AddAuthentication()
    .AddJwtBearer((JwtBearerOptions options) =>
    {
        options.TokenValidationParameters = new TokenValidationParameters()
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey =
                new SymmetricSecurityKey
                (
                    Encoding.UTF8.GetBytes(builder.Configuration.GetValue<string>("JWT:SecretKey"))
                ),
            ValidIssuer = builder.Configuration.GetValue<string>("JWT:Issuer"),
            ValidAudience = builder.Configuration.GetValue<string>("JWT:Audience")
        };
    });

var app = builder.Build();

app.UseRouting();

app.UseSwagger();
app.UseSwaggerUI();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});


app.Run();
