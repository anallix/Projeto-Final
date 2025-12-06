var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllersWithViews();


builder.Services.AddHttpClient<ReservaFront.Services.ReservaApiClient>(client =>
{
    var baseUrl = builder.Configuration["ApiBaseUrl"];
    if (string.IsNullOrEmpty(baseUrl))
        throw new InvalidOperationException("ApiBaseUrl não configurado no appsettings.json");
    
    client.BaseAddress = new Uri(baseUrl);
});

builder.Services.AddSession();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
   
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.UseSession();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
