using System.Text;
using Microsoft.AspNetCore.DataProtection;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDataProtection();
builder.Services.AddHttpContextAccessor();
builder.Services.AddAuthentication("cookie")
.AddCookie("cookie");
//builder.Services.AddScoped<AuthSevice>();
var app = builder.Build();


// app.Use((ctx, next) =>
// {
//     var dataProtection = ctx.RequestServices.GetRequiredService<IDataProtectionProvider>();
//     var protector = dataProtection.CreateProtector("auth-cookie");
//     var authcokie = ctx.Request.Headers.Cookie.FirstOrDefault(x => x.StartsWith("auth="));
//     Console.WriteLine(authcokie);
//     var parts = authcokie.Split('=').Last();
//     var payload = protector.Unprotect(parts);
//     var res = payload.Split(":");
//     var key = res[0];
//     var value = res[1];

//     var claimIdentity = new ClaimsIdentity(new Claim[] { new Claim(key, value) });
//     ctx.User = new ClaimsPrincipal(claimIdentity);
//     return next();
// });
// app.MapGet("/username", (HttpContext httpcontext) =>
// {
//     //var all= httpcontext.Request.Headers["cookie"];
//     return httpcontext.User.FindFirst("usr").ToString();
// });
// //login 
// app.MapGet("/login", (AuthSevice authSevice) =>
// {
//     authSevice.SignIn();
//     return "ok";
// });
app.UseAuthentication();
app.MapGet("/username", (HttpContext httpcontext) =>
{
    //var all= httpcontext.Request.Headers["cookie"];
    return httpcontext.User.FindFirst("usr").ToString();
});
app.MapGet("/login", async (HttpContext ctx) =>
{
    var claimIdentity = new ClaimsIdentity(new Claim[] { new Claim("usr", "Mohammed") }
    ,CookieAuthenticationDefaults.AuthenticationScheme);
    var User = new ClaimsPrincipal(claimIdentity  );
   await ctx.SignInAsync("cookie", User);

    return "ok";
});
app.Run();


// public class AuthSevice
// {
//     private readonly IHttpContextAccessor _httpContextAccessor;
//     private readonly IDataProtectionProvider _dataProtection;

//     public AuthSevice(IDataProtectionProvider dataProtection, IHttpContextAccessor httpContextAccessor)
//     {
//         this._httpContextAccessor = httpContextAccessor;
//         this._dataProtection = dataProtection;
//     }

//     public void SignIn()
//     {
//         var protector = _dataProtection.CreateProtector("auth-cookie");
//         var x = $"auth={protector.Protect("usr:Mohammed".ToString())}";
//         _httpContextAccessor.HttpContext.Response.Headers["set-cookie"] = x;
//         Console.WriteLine(x);
//     }
// }