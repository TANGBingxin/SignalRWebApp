using System.Configuration.Internal;
using System;
using SignalRWebApp.Hubs;
using SignalRWebApp.Services;
using SqlSugar;
using System.Threading.Tasks;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

//Add signalR service
builder.Services.AddSignalR();

//Add session service
builder.Services.AddSession();

//Add sqlsugar
builder.Services.AddTransient<ISqlSugarClient>(provider =>
{
    SqlSugarClient db = new SqlSugarClient(new ConnectionConfig()
    {
        ConnectionString = builder.Configuration.GetSection("SqlConn").Value,
        DbType = DbType.SqlServer,
        IsAutoCloseConnection=true
    });
    return db;
});

builder.Services.AddTransient<IUserService, UserService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

//add routing
app.MapHub<ChatHub>("/chatHub");
app.UseSession();

app.Run();

