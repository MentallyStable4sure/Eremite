using System.Diagnostics;
using System.Net.Http.Json;
using System.Text;

namespace Eremite.Data
{
    internal class DataRouter
    {
        public const string BASE_ROUTE = "https://mentallystable4sure.dev/API/";

        /// <summary>
        /// Sends data with GET method and retrieves response text
        /// </summary>
        /// <param name="additionalUrl">url after a base url (e.g subfolders)</param>
        /// <returns>raw text response</returns>
        public static async Task<string> SendGetRequest(string additionalUrl)
        {
            HttpClient httpClient = new HttpClient();
            var request = new HttpRequestMessage(HttpMethod.Get, $"{BASE_ROUTE}{additionalUrl}");
            var response = await httpClient.SendAsync(request);

            string responseText = new string(await response.Content.ReadAsStringAsync());

            request.Dispose();
            response.Dispose();

            return responseText;
        }

        /// <summary>
        /// Sends data with GET method and retrieves response text
        /// </summary>
        /// <param name="additionalUrl">url after a base url (e.g subfolders)</param>
        /// <returns>raw text response</returns>
        public static async Task<string> SendPostRequest(string additionalUrl, FormUrlEncodedContent content)
        {
            HttpClient httpClient = new HttpClient();
            var request = new HttpRequestMessage(HttpMethod.Post, $"{BASE_ROUTE}{additionalUrl}");

            var response = await httpClient.PostAsync(request.RequestUri, content);

            string responseText = new string(await response.Content.ReadAsStringAsync());

            request.Dispose();
            response.Dispose();

            return responseText;
        }

        /// <summary>
        /// Sends data with GET method and retrieves response bytes
        /// </summary>
        /// <param name="additionalUrl">url after a base url (e.g subfolders)</param>
        /// <returns>raw bytes response</returns>
        public static async Task<byte[]> SendDownloadRequest(string additionalUrl)
        {
            HttpClient httpClient = new HttpClient();
            var request = new HttpRequestMessage(HttpMethod.Get, $"{BASE_ROUTE}{additionalUrl}");
            var response = await httpClient.SendAsync(request);

            var responseBytes = await response.Content.ReadAsByteArrayAsync();

            request.Dispose();
            response.Dispose();

            return responseBytes;
        }

        /// <summary>
        /// Opens a browser tab with url provided
        /// </summary>
        /// <param name="url">URL to open in default browser</param>
        public static void OpenBrowserURL(string url)
        {
            string urlEncoded = url;

            var builder = new System.UriBuilder(urlEncoded);
            Process.Start(builder.ToString());
        }
    }
}

