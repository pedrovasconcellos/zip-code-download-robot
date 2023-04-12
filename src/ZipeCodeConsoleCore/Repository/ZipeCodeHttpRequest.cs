using System;
using System.Net;
using System.Threading;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using ZipeCodeConsoleCore.Models;

namespace ZipeCodeConsoleCore.Repository
{
    public class ZipeCodeHttpRequest
    {
        private readonly ILogger<ZipeCodeHttpRequest> _logger;
        private readonly string _token;

        public ZipeCodeHttpRequest(ILogger<ZipeCodeHttpRequest> logger, string token)
        {
            _logger = logger;
            _token = token;
        }

        public ZipeCode GetZipeCodeInfo(string zipeCode)
        {

            var jsonZipecode = RequestingJsonZipeCode(zipeCode);
            if (jsonZipecode == null)
                return null;

            return JsonCepToZipeCodeModel(ref jsonZipecode);
        }

        private string RequestingJsonZipeCode(string zipeCode)
        {
            try
            {
                Thread.Sleep(1000);
                var accessToken = $"Token token={_token}";
                var url = "https://www.cepaberto.com/api/v3/cep?cep={0}";
                var client = new WebClient { Encoding = System.Text.Encoding.UTF8 };
                client.Headers.Add(HttpRequestHeader.Authorization, accessToken);
                var result = client.DownloadString(string.Format(url, zipeCode));
                return result;
            }
            catch (Exception ex)
            {
                if(ex.Message.Contains("403"))
                    _logger.LogError("Error: {errorMessaage}; ZipeCode: {zipeCode};",
                        ex.Message, zipeCode);
                else
                    _logger.LogError(ex, "Error: {errorMessaage}; ZipeCode: {zipeCode};",
                        ex.Message, zipeCode);

                _logger.LogWarning("Thread sleep 10 minutes.");

                Thread.Sleep(600000);
                return null;
            }
        }

        private ZipeCode JsonCepToZipeCodeModel(ref string json)
        {
            return JsonConvert.DeserializeObject<ZipeCode>(json);
        }
    }
}
