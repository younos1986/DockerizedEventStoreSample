﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace EventStoreSample
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
        }

        // feature barnch 1 is added
        // feature barnch 1 is added 2
        // feature barnch 1 is added 3

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>();

        // this line is added by user in github editor
        // this line is added by user in github editor 2
        // this line is added by user in github editor 3
        // this line is added by user in github editor 4
        // this line is added by user in github editor 5
        // this line is added by user in github editor 6
    }
}
