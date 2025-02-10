using Geta.Optimizely.Extensions.Web;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

Host.CreateDefaultBuilder(args)
    .ConfigureCmsDefaults()
    .ConfigureWebHostDefaults(webBuilder =>
    {
        webBuilder.UseStartup<Startup>();
        webBuilder.UseContentRoot(Path.GetFullPath("../../sandbox/geta-packages-foundation-sandbox/src/Foundation"));
    })
    .Build()
    .Run();
