using Hangfire;
using Hangfire.Storage.SQLite;
using webApi.Services;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddHostedService<JobService>();
builder.Services.AddScoped<IJobTestService, JobTestService>();

builder.Services.AddHangfire(x =>
{
    x.UseSimpleAssemblyNameTypeSerializer();
    x.UseRecommendedSerializerSettings();
    x.UseSQLiteStorage("test.db");
});

builder.Services.AddHangfireServer();

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
app.UseHangfireDashboard();