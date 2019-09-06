using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MovieRental.Core.Account;
using MovieRental.Core.Movie;
using MovieRental.Persitence;

namespace MovieRental
{
    public class Program
    {
        public static void Main(string[] args)
        {
            // CreateWebHostBuilder(args).Build().Run();
            var host = BuildWebHost(args);
            using (var serviceScope = host.Services.CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetRequiredService<MovieDbContext>();
                context.Database.EnsureDeleted();
                context.Database.EnsureCreated();
                context.Account.AddRange(AccountEntity.CreateDumpData());
                context.Movie.AddRange(MovieEntity.CreateDumpData());
                context.MovieLikes.AddRange(MovieLikesEntity.CreateDumpData());
                context.SaveChanges();
            }
            host.Run();
        }

        public static IWebHost BuildWebHost(string[] args) =>
           WebHost.CreateDefaultBuilder(args)
               .UseStartup<Startup>()
               .Build();
    }
}
