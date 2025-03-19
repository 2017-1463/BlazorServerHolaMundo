using ERP.Web.Components;
using ERP.Web.Data;
using ERP.Web.Domain.Dto;
using ERP.Web.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

// Data Services
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Agregar el servicio para la gestión de los clientes
builder.Services.AddScoped<IClienteService, ClienteService>();
builder.Services.AddScoped<ICiudadService, CiudadService>();

// Registra el servicio de Empleado
builder.Services.AddScoped<IEmpleadoService, EmpleadoService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    dbContext.Database.Migrate();
    var seeder = new AppDbContextSeeder(dbContext);
    seeder.SeedAsync().Wait();
}

app.Run();
