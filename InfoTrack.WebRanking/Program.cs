using InfoTrack.WebRanking.Interfaces;
using InfoTrack.WebRanking.Repository;
using InfoTrack.WebRanking.Services;

var builder = WebApplication.CreateBuilder(args);

// CORS services
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins",
        builder =>
        {
            builder.AllowAnyOrigin()
                   .AllowAnyMethod()
                   .AllowAnyHeader();
        });
});

// Added Configurations in one place to be more precise when editing. 
var config = new ConfigurationBuilder()
    .SetBasePath(builder.Environment.ContentRootPath)
    .AddJsonFile("//Configuration/appsettings.json", optional: false, reloadOnChange: true)
    .AddJsonFile($"//Configuration/appsettings.{builder.Environment.EnvironmentName}.json", optional: true)
    .AddEnvironmentVariables()
    .Build();

builder.Configuration.AddConfiguration(config);

// Register services 
builder.Services.AddControllersWithViews();
builder.Services.AddSingleton<ISearchService, SearchService>();
builder.Services.AddTransient<ISearchRepository, SearchRepository>();

var app = builder.Build();
// CORS policy
app.UseCors("AllowAllOrigins");

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();

    // Serve the default Vue page
    app.UseDefaultFiles();
}

app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();

