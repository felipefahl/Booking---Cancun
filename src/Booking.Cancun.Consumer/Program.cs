﻿//setup our DI
using Microsoft.Extensions.DependencyInjection;

var serviceProvider = new ServiceCollection()
    .AddLogging()
    .AddSingleton<IFooService, FooService>()
    .AddSingleton<IBarService, BarService>()
    .BuildServiceProvider();

//configure console logging
serviceProvider
    .GetService<ILoggerFactory>()
    .AddConsole(LogLevel.Debug);

var logger = serviceProvider.GetService<ILoggerFactory>()
    .CreateLogger<Program>();
logger.LogDebug("Starting application");

//do the actual work here
var bar = serviceProvider.GetService<IBarService>();
bar.DoSomeRealWork();

logger.LogDebug("All done!");
