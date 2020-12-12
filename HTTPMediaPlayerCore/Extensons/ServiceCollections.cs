using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Http;
using HTTPMediaPlayerCore.Services;

namespace HTTPMediaPlayerCore.Extensons
{
	public static class ServiceCollections
	{
		/// <summary>
		/// Adds web project services to dependency injection container and authentication for POP Forums. This method 
		/// fails if the ISetupService can't connect to the database or the database isn't set up.
		/// </summary>
		/// <param name="services"></param>
		/// <returns>The updated IServiceCollection.</returns>
		public static IServiceCollection AddDuwaysMvc(this IServiceCollection services)
		{
			return services.AddDuwaysMvc(true);
		}

		/// <summary>
		/// Adds web project services to dependency injection container and authentication for POP Forums. This method 
		/// fails if the ISetupService can't connect to the database or the database isn't set up.
		/// </summary>
		/// <param name="services"></param>
		/// <param name="includePopForumsBaseServices">Indicate false if you intend to call
		/// services.AddPopForumsBase() on your own.</param>
		/// <returns>The updated IServiceCollection.</returns>
		public static IServiceCollection AddDuwaysMvc(this IServiceCollection services, bool includeDuwaysBaseServices)
		{
			if (includeDuwaysBaseServices) ;
				//services.AddPopForumsBase();
			services.AddHttpContextAccessor();
			//services.AddPopIdentity();
			
			/*services.AddTransient<IUserRetrievalShim, UserRetrievalShim>();
			services.AddTransient<ITopicViewCountService, TopicViewCountService>();
			services.AddTransient<IExternalLoginRoutingService, ExternalLoginRoutingService>();
			services.AddTransient<IExternalLoginTempService, ExternalLoginTempService>();
			services.AddTransient<IBroker, Broker>();*/
			
			// this is required for error logging:
			services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

			/*var serviceProvider = services.BuildServiceProvider();
			var setupService = serviceProvider.GetService<ISetupService>();
			if (!setupService.IsConnectionPossible() || !setupService.IsDatabaseSetup())
				return services;

			services.AddAuthentication()
				.AddCookie(AdsMasterAuthorizationDefaults.AuthenticationScheme, option => option.ExpireTimeSpan = new TimeSpan(365, 0, 0, 0));
			*/
			services.AddTransient<ICourseService, CourseService>();

			return services;
		}
	}
}
