using EventPlanning;
using Lib.AspNetCore.ServerSentEvents;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddServerSentEvents();
builder.Services.AddHostedService<RandomNumberWorker>();
builder.Services.AddRazorPages();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapServerSentEvents("/rn-updates");
app.MapRazorPages();

app.MapGet("random", async (ctx) => {
    ctx.Response.ContentType = "text/html";
    await ctx.Response.WriteAsync($"<span>{Number.Value}</span>");
});

app.Run();

