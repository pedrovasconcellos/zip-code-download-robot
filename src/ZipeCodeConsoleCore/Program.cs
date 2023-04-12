using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using ZipeCodeConsoleCore.Models;
using ZipeCodeConsoleCore.Repository;

namespace ZipeCodeConsoleCore
{
    class Program
    {
        private static MongoDBRepository _repository;
        private static ZipeCodeHttpRequest _http;

        static void Main(string[] args)
        {
            try
            {
                Init();
                Process().Wait();
                Console.ReadKey();
            }
            catch (Exception ex)
            {
                PrintColor(ex.Message, ConsoleColor.Red);
                Console.ReadKey();
            }
        }

        private static async Task Process()
        {
            PrintColor($"Start Process. DateTime: {DateTime.Now}", ConsoleColor.Green);

            var allZipeCodes = (await _repository.GetAllAsyncNumber())
                ?.Select(x => x.cep).ToList()
                ?? new List<string>();

            foreach (var zipeCodeItem in allZipeCodes)
            {
                var zipeCodeFormatItem = zipeCodeItem.ToString().PadLeft(8, '0');
                await ProcessCep(zipeCodeFormatItem);
            }

            PrintColor("Finished Process.", ConsoleColor.Green);
            await Task.CompletedTask;
        }

        private static async Task ProcessCep(string zipeCodeNumber)
        {
            if (_repository.ZipeCodeExist<ZipeCodeDesnormalized>(zipeCodeNumber))
                return;

            PrintColor((String.Concat("Get CEP: ", zipeCodeNumber.Insert(5, "-"))), (ConsoleColor)new Random().Next(7, 14));

            var zipeCodeSelected = default(ZipeCode);
            var zipeCodeDesnormalized = default(ZipeCodeDesnormalized);
            while (zipeCodeDesnormalized == null)
            {
                zipeCodeSelected = _http.GetZipeCodeInfo(zipeCodeNumber);
                if (zipeCodeSelected == null)
                    PrintColor($"Error on the server, performing a new request! ZipeCode = {zipeCodeNumber}", ConsoleColor.Magenta);
                else
                    zipeCodeDesnormalized = new ZipeCodeDesnormalized(zipeCodeSelected);
            }

            if (string.IsNullOrEmpty(zipeCodeDesnormalized.cep)
                && string.IsNullOrEmpty(zipeCodeDesnormalized.estado)
                && string.IsNullOrEmpty(zipeCodeDesnormalized.cidade))
            {
                PrintColor($"ZipeCode {zipeCodeNumber.Insert(5, "-")} not found!", ConsoleColor.Yellow);
                return;
            }

            PrintObject(zipeCodeDesnormalized);
            await SaveZipeCode(zipeCodeDesnormalized);
            await SaveZipeCode(zipeCodeSelected);
        }

        private static async Task SaveZipeCode<T>(
            T zipeCodeDesnormalized) where T : ZipeCodeBase
        {
            await _repository.InsertOneAsync(zipeCodeDesnormalized);
        }

        private static void PrintObject(object obj)
        {
            string response = JsonConvert.SerializeObject(obj);
            PrintColor(response, ConsoleColor.Green);
        }

        private static void PrintColor(string text, ConsoleColor fontColor)
        {
            Console.ForegroundColor = fontColor;
            Console.WriteLine(String.Concat(text, Environment.NewLine));
        }

        private static void Init()
        {
            using var serviceProvider = new ServiceCollection()
                .AddLogging(config =>
                    config
                        .ClearProviders()
                        .AddConsole()
                        .SetMinimumLevel(LogLevel.Information)
                    )
                .BuildServiceProvider();


            var connectionString = Environment.GetEnvironmentVariable("Vasconcellos_ZipeCode_ConsoleApp_MongoDB");
            if (string.IsNullOrEmpty(connectionString))
                throw new ArgumentException("Connection string invalid!");

            var token = Environment.GetEnvironmentVariable("Vasconcellos_ZipeCode_ConsoleApp_Token");
            if (string.IsNullOrEmpty(token))
                throw new ArgumentException("token http-request invalid!");

            var loggerM = serviceProvider
                .GetService<ILoggerFactory>()
                .CreateLogger<MongoDBRepository>();

            if (loggerM is null)
                throw new ArgumentNullException(nameof(loggerM));

            _repository = new MongoDBRepository(loggerM, connectionString);

            var loggerZ = serviceProvider
                .GetService<ILoggerFactory>()
                .CreateLogger<ZipeCodeHttpRequest>();

            if (loggerZ is null)
                throw new ArgumentNullException(nameof(loggerZ));
            _http = new ZipeCodeHttpRequest(loggerZ, token);
        }
    }
}
