using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using System.Text.Json.Serialization;
using System.Web.Mvc;
using TaskPlanner.BusinessLogic.Services;
using TaskPlanner.DataAccess;
using TaskPlanner.DataAccess.Repositories;
using TaskPlanner.Domain.Abstraction;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());

builder.Services.AddControllersWithViews();
//builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
//builder.Services.AddScoped<ITaskRepository, TaskRepository>();
//builder.Services.AddScoped<ITaskService, TaskService>();
//builder.Services.AddScoped<IProjectRepository, ProjectRepository>();
//builder.Services.AddScoped<IProjectService, ProjectService>();
builder.Services.AddDbContext<TaskPlannerDBContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("TaskPlannerDBContext"));
});

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });

builder.Host.ConfigureContainer<ContainerBuilder>(containerBuilder =>
{
    containerBuilder.RegisterType<ProjectService>().As<IProjectService>();
    containerBuilder.RegisterType<TaskService>().As<ITaskService>();
    containerBuilder.RegisterType<UnitOfWork>().As<IUnitOfWork>();
    containerBuilder.RegisterType<TaskRepository>().As<ITaskRepository>();
    containerBuilder.RegisterType<ProjectRepository>().As<IProjectRepository>();
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

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Project}/{action=Index}/{id?}");

app.Run();

//builder.Services.AddSwaggerGen(options =>
//{
//    options.SwaggerDoc("v1", new OpenApiInfo
//    {
//        Title = "TaskPlanner API",
//        Version = "v1",
//        Description = "API для керування проектами та задачами"
//    });
//})

//if (app.Environment.IsDevelopment())
//{
//    app.UseSwagger(); // Включаємо Swagger
//    app.UseSwaggerUI(options =>
//    {
//        options.SwaggerEndpoint("/swagger/v1/swagger.json", "TaskPlanner API v1");
//        options.RoutePrefix = string.Empty;  // Для доступу до документації на кореневому шляху
//    });
//}

//app.UseRouting();
//app.MapControllers();

//app.Run();
