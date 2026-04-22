using Microsoft.EntityFrameworkCore;
using ST10330209PROG7311POE1.Data;
using ST10330209PROG7311POE1.Services;
using ST10330209PROG7311POE1.Patterns.Strategy;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));


builder.Services.AddHttpClient();


builder.Services.AddTransient<LiveExchangeRateStrategy>();
builder.Services.AddTransient<FallbackExchangeRateStrategy>();


builder.Services.AddScoped<CurrencyService>(provider =>
{
    var httpClient = provider.GetRequiredService<HttpClient>();
    var liveStrategy = new LiveExchangeRateStrategy(httpClient);
    return new CurrencyService(liveStrategy);
});

builder.Services.AddControllersWithViews();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();