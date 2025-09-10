using GameStore.Api.Data;
using GameStore.Api.Endpoints;

var builder = WebApplication.CreateBuilder(args);

// Configures our DbContext with SQLite
var connectionString = builder.Configuration.GetConnectionString("GameStore");
builder.Services.AddSqlite<GameStoreDbContext>(connectionString);

var app = builder.Build();

// Map endpoints
app.MapGamesEndpoints();

// Execute migrations on startup
app.MigrateDB();

app.Run();
