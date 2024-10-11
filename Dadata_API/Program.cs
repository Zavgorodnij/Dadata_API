using Dadata_API.Models;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddLogging();
builder.Services.AddHttpClient("DadataClient", client =>
{
    client.BaseAddress = new Uri(builder.Configuration["DaDataAPI_Config:Base_Adress"]);
    client.DefaultRequestHeaders.Add("Authorization", $"Token {builder.Configuration["DaDataAPI_Config:API_Key"]}");
    client.DefaultRequestHeaders.Add("X-Secret", builder.Configuration["DaDataAPI_Config:Secret_Key"]);
});
builder.Services.AddAutoMapper(typeof(Adress_Mapping));


//Снимаем ограничения Cors для возможности вызова извне
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policyBuilder =>
    {
        policyBuilder.AllowAnyOrigin()
                       .AllowAnyHeader()
                       .WithMethods("GET");
    });
});

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
app.UseCors("AllowAll");
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=API}/{action=Index}/{id?}");

app.Run();
