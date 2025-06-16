using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace StopwatchLatencyTester
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(IgnoreCertificateValidation);

            var client = new HttpClient();
            var url = "https://localhost:7116/api/recipe/getrecipes";

            long totalMilliseconds = 0;
            int failedRequests = 0;

            for (int i = 0; i < 500; i++)
            {
                var stopwatch = Stopwatch.StartNew();

                try
                {
                    var response = await client.GetAsync(url);
                    stopwatch.Stop();

                    if (!response.IsSuccessStatusCode)
                    {
                        Console.WriteLine($"Request {i + 1} failed with status code: {response.StatusCode}");
                        failedRequests++;
                    }
                    else
                    {
                        totalMilliseconds += stopwatch.ElapsedMilliseconds;
                    }
                }
                catch (Exception ex)
                {
                    stopwatch.Stop();
                    Console.WriteLine($"Request {i + 1} failed due to an unexpected error: {ex.Message}");
                    failedRequests++;
                }
            }

            double avgLatency = totalMilliseconds / (double)(500 - failedRequests);
            Console.WriteLine($"500 sequential calls");
            Console.WriteLine($"Average latency: {avgLatency:F2} ms");
            Console.WriteLine($"Failed requests: {failedRequests}");

            Console.ReadLine();
        }
        static bool IgnoreCertificateValidation(object sender, X509Certificate cert, X509Chain chain, SslPolicyErrors sslErrors)
        {
            return true;
        }
    }
}
