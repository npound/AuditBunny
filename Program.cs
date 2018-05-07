using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;


using System;
using System.IO;
using System.Net;
using Microsoft.AspNetCore.Server.Kestrel.Https;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.AspNetCore.Server.Kestrel.Transport.Abstractions.Internal;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;


namespace AuditBunny
{
    public class Program
    {
        public static void Main(string[] args)
        {
            BuildWebHost(args).Run();
        }

        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
           .UseKestrel(options =>
           {
             options.Listen(new IPEndPoint(IPAddress.Loopback, 44397), listenOptions =>
             {
               var httpsConnectionAdapterOptions = new HttpsConnectionAdapterOptions()
               {
                 ClientCertificateMode = ClientCertificateMode.AllowCertificate,
                 SslProtocols = System.Security.Authentication.SslProtocols.Tls12,
                 ServerCertificate = new System.Security.Cryptography.X509Certificates.X509Certificate2("certs/TempCAAuditBunny.cer")
               };
               listenOptions.UseHttps(httpsConnectionAdapterOptions);
             });
           })
                .Build();
    }
}
