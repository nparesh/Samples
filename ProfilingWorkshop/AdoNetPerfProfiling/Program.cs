﻿using AdoNetPerfProfiling.Controller;
using Microsoft.Owin.FileSystems;
using Microsoft.Owin.Hosting;
using Microsoft.Owin.StaticFiles;
using Owin;
using System;
using System.IO;
using System.Reflection;
using System.Web.Http;

namespace AdoNetPerfProfiling
{
	class Program
	{
		static void Main(string[] args)
		{
			//using (WebApp.Start<Startup>("http://localhost:12345"))
			//{
			//	Console.WriteLine("Listening on port 12345. Press any key to quit.");
			//	Console.ReadLine();
			//}

			for (var i = 0; i < 2000; i++)
			{
				var result = new CachingSearchController().Get("Lee");
			}
		}
	}

	public class Startup
	{
		public void Configuration(IAppBuilder app)
		{
			// Setup routes
			var config = new HttpConfiguration();

			// Removing XML formatter, we just want to support JSON
			config.Formatters.Remove(config.Formatters.XmlFormatter);

			app.UseFileServer(new FileServerOptions()
			{
				FileSystem = new PhysicalFileSystem(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "..", "..", "WebUI")),
				EnableDirectoryBrowsing = true,
				EnableDefaultFiles = true
			});

			Startup.SetupWebApiRoutes(config);
			app.UseWebApi(config);
		}

		private static void SetupWebApiRoutes(HttpConfiguration config)
		{
			config.Routes.MapHttpRoute(
				name: "webapi",
				routeTemplate: "api/{controller}",
				defaults: new { customerName = RouteParameter.Optional }
			);
		}
	}
}
