using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();
var global ="";
// Chaining Middleware
app.Use(async (context, next) =>
{
    // Logic before passing to the next middleware
    Console.WriteLine("Before Chaining Middleware");
    global += "B Chaining - ";

    await next();

    // Logic after the next middleware
    Console.WriteLine("After Chaining Middleware");
    global += "A Chaining - ";

});

// Short-Circuit Middleware
app.Map("/short-circuit", appBuilder => appBuilder.Run(async context =>
{
    global += "short-circuit - ";
    await context.Response.WriteAsync($"Short-Circuit Middleware triggered! \n  {global}");
}));

// Branching Middleware
app.MapWhen(context => context.Request.Query.ContainsKey("branch"), branchBuilder =>
{
    branchBuilder.Run(async context =>
    {
        global += "Branching - ";
        await context.Response.WriteAsync($"Branching Middleware path taken! \n {global} ");
    });
});

// Ending Middleware
app.Run(async context =>
{
    global += "Ending - ";
    await context.Response.WriteAsync($"Hello from Ending Middleware! \n {global}");
});

app.Run();