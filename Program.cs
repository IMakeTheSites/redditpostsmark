// using System;
// using System.Net.Http;
// using System.Threading.Tasks;
using System.Text.Json;
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;

namespace RedditPostsMark
{
    class Program
    {
            // Create a new HttpClient instance
        private static readonly HttpClient httpClient = new HttpClient();
            // API Endpoint
        string apiUrl = "https://oauth.reddit.com/r/dadjokes/top";
       // Define intervals for fetching and time limit
        private const int intervalSeconds = 30;
        private const int totalDurationSeconds = 3 * 60;
        
        static async Task Main (string[] args)
        {
                var keyVaultName = "markredditkv1";
                var secretName = "newToken";

                var client = new SecretClient(new Uri($"https://{keyVaultName}.vault.azure.net"),
                    new DefaultAzureCredential(includeInteractiveCredentials: true));

                KeyVaultSecret secret = await client.GetSecretAsync(secretName);
                
                var cancellationTokenSource = new CancellationTokenSource(totalDurationSeconds * 1000);
                var accessToken = secret.Value;
               try
                {
                    while (!cancellationTokenSource.Token.IsCancellationRequested)
                    {
                    var request = new HttpRequestMessage(HttpMethod.Get, "https://oauth.reddit.com/r/dadjokes/top");
                    request.Headers.Add("User-Agent", "ChangeMeClient/0.1 by YourUsername");
                    request.Headers.Add("Authorization", $"Bearer {accessToken}");
                    var response = await httpClient.SendAsync(request);
                    await Task.Delay(TimeSpan.FromSeconds(intervalSeconds), cancellationTokenSource.Token);
                     // Read the response content as a string
                    string responseBody = await response.Content.ReadAsStringAsync();
                    ParseAndPrintRedditJson(responseBody);
                    }
                }
                catch (TaskCanceledException)
                {
                    Console.WriteLine("Reddit retrieval stops after 3 minutes.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"An error occurred: {ex.Message}");
                }

                    // Parse the JSON response
             static void ParseAndPrintRedditJson(string responseBody)
            {
                using (JsonDocument doc = JsonDocument.Parse(responseBody))
                {
                    JsonElement root = doc.RootElement;
                    JsonElement data = root.GetProperty("data");
                    JsonElement children = data.GetProperty("children");

                    Console.WriteLine("Top Posts:");

                    foreach (JsonElement child in children.EnumerateArray())
                    {
                        JsonElement postData = child.GetProperty("data");
                        string title = postData.GetProperty("title").GetString();
                        string url = postData.GetProperty("url").GetString();

                        Console.WriteLine($"Title: {title}");
                        Console.WriteLine($"URL: {url}");
                        
                    }
                }
            }
            }


        }
    }
