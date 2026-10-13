using Microsoft.EntityFrameworkCore;
var builder = WebApplication.CreateBuilder(args);
builder.Configuration.AddJsonFile("appsettings.Local.json", optional: true, reloadOnChange: false);

// Add services to the container.
// Connection string comes from configuration, never from source code:
//   environment variable  ConnectionStrings__postgres
//   or dotnet user-secrets / appsettings.Local.json (git-ignored)
// See .env.example and README.md.
var connectionString = builder.Configuration.GetConnectionString("postgres")
    ?? throw new InvalidOperationException(
        "Missing connection string 'ConnectionStrings:postgres'. Set the ConnectionStrings__postgres environment variable (see .env.example).");

builder.Services.AddDbContext<NewsCollectorDbContext>(options =>
    options.UseNpgsql(connectionString));
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials()
            .SetIsOriginAllowed(origin => true);
    });
});
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSignalR();

builder.Services.AddHostedService<NewsContentPullerHostedService>();
builder.Services.AddHostedService<NewsSourcesPullHostedService>();
builder.Services.AddHostedService<NewsContentPredictHostedService>();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseCors();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();
app.MapHub<SentimentHub>("/hub");
app.Run();
