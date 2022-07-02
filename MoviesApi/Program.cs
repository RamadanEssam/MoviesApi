using Microsoft.OpenApi.Models;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer(); builder.Services.AddSwaggerGen(options =>
{

    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "Test API",
        Description = "My First api",
        TermsOfService = new Uri("https://www.linkedin.com/in/ramadanessameita/"),
        Contact = new OpenApiContact
        {
            Name = "Ramadan",
            Email = "Ramadaneita@gmail.com",
            Url = new Uri("https://www.linkedin.com/in/ramadanessameita/")

        },
        License = new OpenApiLicense {
    
            Name= "Ramadan Lisence",
            Url = new Uri("https://www.linkedin.com/in/ramadanessameita/")

        }



    });
});

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



