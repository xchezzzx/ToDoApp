using Microsoft.EntityFrameworkCore;
using ToDoApp.Accessor.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services
    .AddControllers()
    .AddDapr(); // Add Dapr integration

builder.Services.AddDbContext<TodoDbContext>(opt => 
    opt.UseNpgsql(builder.Configuration.GetConnectionString("TodoDbConnection"))
    );

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<TodoDbContext>();
    db.Database.Migrate();
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseRouting();

app.UseCloudEvents();
app.MapSubscribeHandler();

app.MapControllers();

app.Run();
