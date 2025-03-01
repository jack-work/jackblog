using JackBlog.Models;
using JackBlog.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddSingleton<JackBlog.Models.BlogService>();
builder.Services.AddKeyedSingleton<
    ITestCaseProvider<SordidArraysTestCase, SordidArraysInput, IEnumerable<int>>
>(
    "SordidArrays",
    (_, key) => new TestCaseProvider<SordidArraysTestCase, SordidArraysInput, IEnumerable<int>>((string)key)
);
builder.Services.AddSingleton<SordidArraysService>();

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

app.MapControllerRoute(name: "default", pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
