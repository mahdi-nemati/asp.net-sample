using ApplicationServices;
using DAL;
using Endpoint.CategoryEndpoint;
using Endpoint.Endpoint;
using Endpoint.Infastructure;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;

namespace Endpoint.Extentions
{
    public static class HostingExtention
    {
        public static WebApplication ConfigureService(this WebApplicationBuilder builder)
        {
            // connection string
            string connectionString = builder.Configuration.GetConnectionString("Coursecnn");

            // services
            builder.Services.AddScoped<CategoryServices>();
            builder.Services.AddScoped<TeacherService>();
            builder.Services.AddScoped<CourseServices>();
            builder.Services.AddScoped<IFileAdapter, LocalFileStorageAdapter>();
            builder.Services.AddOutputCache();
            builder.Services.AddOpenApi();
            //builder.Services.AddAntiforgery();
            //builder.Services.AddAutoMapper(typeof(HostingExtention));
            builder.Services.AddAutoMapper(cfg => { }, typeof(HostingExtention));
            builder.Services.AddDbContext<CourseDBContext>(c =>
            {
                c.UseSqlServer(connectionString);
            });
            return builder.Build();
        }

        public static WebApplication ConfigurePipeline(this WebApplication app)
        {
            app.UseOutputCache();
            app.UseStaticFiles();

            //app.UseRouting();
            //app.UseAntiforgery();

            app.MapOpenApi();
            app.MapScalarApiReference();


            // regester Category 
            app.MapCategories("/categories");

            // regester teacher
            app.MapTeacher("/teachers");

            // regester course
            app.MapCourse("/course");

            return app;
        }
    }
}
