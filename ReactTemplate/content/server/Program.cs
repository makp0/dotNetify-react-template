using System.Text;
using BrunoLau.SpaServices.Webpack;
using DotNetify;
using DotNetify.Security;
using Microsoft.IdentityModel.Tokens;
using projectName;

var builder = WebApplication.CreateBuilder(args);

var services = builder.Services;

services.AddSignalR();
services.AddDotNetify();

services.AddTransient<ILiveDataService, MockLiveDataService>();
services.AddSingleton<IEmployeeService, EmployeeService>();

var app = builder.Build();

app.UseWebSockets();
app.UseDotNetify();

if (app.Environment.IsDevelopment())
   app.UseWebpackDevMiddlewareEx(new WebpackDevMiddlewareOptions { HotModuleReplacement = true });

app.UseFileServer();
app.UseRouting();
app.UseEndpoints(endpoints =>
{
   endpoints.MapHub<DotNetifyHub>("/dotnetify");
   endpoints.MapFallbackToFile("index.html");
});

app.Run();