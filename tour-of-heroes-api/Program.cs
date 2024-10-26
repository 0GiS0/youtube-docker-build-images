using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<HeroesContext>(opt => opt.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", builder =>
    {
        builder.AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader();
    });
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseCors("AllowAll");

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

var heroes= new []
{
    new Hero(1, "Batman", "Bruce Wayne", "A wealthy American playboy, philanthropist, and owner of Wayne Enterprises."),
    new Hero(2, "Superman", "Clark Kent", "Born as Kal-El on the dying planet Krypton, his parents Jor-El and Lara sent him to Earth.")
};

app.MapGet("/", () =>
{
    return Results.Text("Welcome to the Tour of Heroes API", "text/html");
});

app.MapGet("/api/hero",  ( ) =>
{
    return heroes;
})
.WithName("GetHeroes")
.WithOpenApi();

app.Run();

public record Hero(int Id, string Name, string AlterEgo, string Description);