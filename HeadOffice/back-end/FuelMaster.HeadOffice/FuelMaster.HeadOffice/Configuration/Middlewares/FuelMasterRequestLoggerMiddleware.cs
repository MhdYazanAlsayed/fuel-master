using FuelMaster.HeadOffice.Core.Helpers;
using System.Diagnostics;

namespace FuelMaster.HeadOffice.Extensions.Middlewares
{
    public class FuelMasterRequestLoggerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly object lockObj = new object();
        public FuelMasterRequestLoggerMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            Console.ForegroundColor = ConsoleColor.White;

            var stopWatch = new Stopwatch();
            string logMessage = $"[{DateTimeCulture.Now.ToString("HH:mm:ss")}] ";

            logMessage += context.Request.Protocol.ToString() + " ";

            stopWatch.Start();
            await _next(context);
            stopWatch.Stop();

            lock (lockObj)
            {
                Console.Write(logMessage);
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.Write(context.Request.Path + " ");
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write($"Responded ");
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.Write(context.Response.StatusCode.ToString() + " ");

                Console.ForegroundColor = ConsoleColor.White;
                Console.Write("In ");
                Console.ForegroundColor = ConsoleColor.Yellow;

                Console.Write(stopWatch.ElapsedMilliseconds + " ms");
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write("\n");
            }

        }
    }
}
