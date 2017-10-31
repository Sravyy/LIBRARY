using System.IO;
using Microsoft.AspNetCore.Hosting;

namespace Registrar
{
  public class Registrar
  {
    public static void Main(string[] args)
    {
      var host = new WebHostBuilder()
                .UseKestrel()
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseIISIntegration()
                .UseStartup<Startup>()
                .Build();

        host.Run();
    }
  }
}
