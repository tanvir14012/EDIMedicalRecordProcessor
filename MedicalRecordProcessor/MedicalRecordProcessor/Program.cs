using MedicalRecordProcessor.DataAccess;
using MedicalRecordProcessor.Factories;
using MedicalRecordProcessor.Services;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
    options.JsonSerializerOptions.WriteIndented = true;
}); ;

var connStr = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<AppDbContext>(ops =>
{
    ops.UseSqlServer(connStr);
});

builder.Services.AddScoped<IMedicalReportService, MedicalReportService>();
builder.Services.AddScoped<IReportDetailsService, ReportDetailsService>();
builder.Services.AddScoped<IMedicalReportFactory, MedicalReportFactory>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
                      builder =>
                      {
                          builder.AllowAnyOrigin()
                                .AllowAnyHeader()
                                .AllowAnyMethod();
                      });
});

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var scopedProvider = scope.ServiceProvider;
    try
    {
        var dbContext = scopedProvider.GetRequiredService<AppDbContext>();
        if (dbContext.Database.IsSqlServer())
        {
            if(dbContext.Database.GetPendingMigrations().Any())
                dbContext.Database.Migrate();
        }
    }
    catch (Exception ex)
    {
        app.Logger.LogError(ex, "An error occurred seeding the DB.");
    }
}


// Configure the HTTP request pipeline.

app.UseCors("AllowAll");

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
