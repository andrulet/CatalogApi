using CatalogApi.Helpers;
using CatalogApi.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using CatalogApi.Models;
using CatalogApi.Repositories;
using CatalogApi.Repositories.CollectionRepository;
using CatalogApi.Repositories.CommentRepository;
using CatalogApi.Repositories.FilmRepository;
using CatalogApi.Repositories.RatingRepository;
using CatalogApi.Repositories.UserRepository;
using CatalogApi.Services.IServices;
using FileStorage;

namespace CatalogApi
{
    public class Startup
    {
        private IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        // add services to the DI container
        public void ConfigureServices(IServiceCollection services)
        {            
            services.AddDbContext<CatalogContext>(options => options.UseSqlServer(Configuration.GetConnectionString("CatalogApiDatabase")));

            services.AddCors();
            services.AddControllers();
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            services.Configure<AppSettings>(Configuration.GetSection("AppSettings"));

            // configure DI for application services
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IFilmService, FilmService>();
            services.AddScoped<ICommentService, CommentService>();
            services.AddScoped<IRatingService, RatingService>();
            services.AddScoped<IFileStorageService, FileStorageService>();
            services.AddScoped<ICollectionService, CollectionService>();
            //services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            services.AddScoped<ICollectionRepository, CollectionRepository>();
            services.AddScoped<ICommentRepository, CommentRepository>();
            services.AddScoped<IFilmRepository, FilmRepository>();
            services.AddScoped<IRatingRepository, RatingRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
        }

        // configure the HTTP request pipeline
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseRouting();

            // global cors policy
            app.UseCors(x => x
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader());

            // custom jwt auth middleware
            app.UseMiddleware<JwtMiddleware>();

            app.UseEndpoints(x => x.MapControllers());
        }
    }
}