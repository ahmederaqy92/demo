using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Benday.Presidents.Api.Interfaces;
using Benday.Presidents.Api.Services;
using Benday.Presidents.Common;
using Benday.Presidents.Api.DataAccess;
using Benday.Presidents.Api.Features;
using Benday.Presidents.Api.DataAccess.SqlServer;
using Microsoft.AspNetCore.Http;
using Benday.Presidents.WebUi.Data;
using Benday.Presidents.WebUi.Services;
using Benday.Presidents.WebUi.Models;

namespace Benday.Presidents.WebUi
{
	public class Startup
	{
		public Startup(IHostingEnvironment env)
		{
			var builder = new ConfigurationBuilder()
				.SetBasePath(env.ContentRootPath)
				.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
				.AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true);

			if (env.IsDevelopment())
			{
				// For more details on using the user secret store see https://go.microsoft.com/fwlink/?LinkID=532709
				builder.AddUserSecrets<Startup>();
			}

			builder.AddEnvironmentVariables();
			Configuration = builder.Build();

			Console.WriteLine("WebUi: Connection string: {0}", Configuration.GetConnectionString("default"));
		}

		public IConfigurationRoot Configuration { get; }

		// This method gets called by the runtime. Use this method to add services to the container.
		public void ConfigureServices(IServiceCollection services)
		{
			//services.AddDbContext<ApplicationDbContext>(options =>
			//	options.UseSqlite(Configuration.GetConnectionString("DefaultConnection")));
			services.AddDbContext<ApplicationDbContext>(options =>
				options.UseSqlServer(Configuration.GetConnectionString("default")));

			services.AddIdentity<ApplicationUser, IdentityRole>(
				options =>
				{
					options.Password.RequireDigit = false;
					options.Password.RequireLowercase = false;
					options.Password.RequiredLength = 4;
					options.Password.RequireNonAlphanumeric = false;
					options.Password.RequireUppercase = false;
				})
				.AddEntityFrameworkStores<ApplicationDbContext>()
				.AddDefaultTokenProviders();

			services.AddMvc();

			RegisterTypes(services);            
		}
		void RegisterTypes(IServiceCollection services)
		{
		// Add application services.
			services.AddTransient<IEmailSender, AuthMessageSender>();
			services.AddTransient<ISmsSender, AuthMessageSender>();		
		
			services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.AddTransient<IUsernameProvider, HttpContextUsernameProvider>();

			services.AddTransient<IFeatureManager, FeatureManager>();

			services.AddTransient<Api.Services.ILogger, Logger>();

			services.AddDbContext<PresidentsDbContext>(options =>
				options.UseSqlServer(Configuration.GetConnectionString("default")));

			services.AddTransient<IPresidentsDbContext, PresidentsDbContext>();

			services.AddTransient<IRepository<Person>, SqlEntityFrameworkPersonRepository>();
			services.AddTransient<IPresidentValidatorStrategy, PresidentValidatorStrategy>();

			services.AddTransient<IFeatureRepository, SqlEntityFrameworkFeatureRepository>();

			var tempServiceProvider = services.BuildServiceProvider();

			var features = tempServiceProvider.GetService<IFeatureManager>();

			if (features.FeatureUsageLogging == false)
			{
				services.AddTransient<IPresidentService, PresidentService>();
			}
			else
			{
				services.AddTransient<IPresidentService, PresidentServiceWithLogging>();
			}
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
		{
			loggerFactory.AddConsole(Configuration.GetSection("Logging"));
			loggerFactory.AddDebug();

			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
				app.UseDatabaseErrorPage();
				app.UseBrowserLink();
			}
			else
			{
				app.UseExceptionHandler("/Home/Error");
			}

			app.UseStaticFiles();

			app.UseIdentity();

			// Add external authentication middleware below. To configure them please see https://go.microsoft.com/fwlink/?LinkID=532715

			app.UseMvc(routes =>
			{
				routes.MapRoute(
					name: "default",
					template: "{controller=President}/{action=Index}/{id?}");
			});
		}
	}
}
