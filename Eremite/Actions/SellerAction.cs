using Eremite.Data;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace Eremite.Actions
{
    internal class SellerAction
    {

        public async static Task<bool> BuyWelkin(string playerUID)
        {
            var orderData = new OrderData() { ID = playerUID };

            var timestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
            string jsonData = JsonSerializer.Serialize<OrderData>(orderData);
            var orderResponse = JsonSerializer.Deserialize<OrderDataResponse>(await DataRouter.SendPostRequest($"{Endpoint.SERVICES_ENDPOINT}/{Endpoint.SIGN_PAYLOAD}", jsonData));

            var auth = orderResponse.Sign; //await DataRouter.SendGetRequest($"{Endpoint.SERVICES_ENDPOINT}/{Endpoint.SIGN_PAYLOAD}");
            Console.WriteLine(auth);
            var auth_basic = Convert.ToBase64String(Encoding.UTF8.GetBytes("partnerid:secret"));

            Console.WriteLine(auth_basic);
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://moogold.com/");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Add("timestamp", timestamp.ToString());
                client.DefaultRequestHeaders.Add("auth", auth);
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", auth_basic);

                var stringContent = new StringContent(orderResponse.Payload, Encoding.UTF8, "application/json");
                var response = await client.PostAsync($"wp-json/v1/api/{orderData.Path}", stringContent);

                if (response.IsSuccessStatusCode)
                {
                    var responseString = await response.Content.ReadAsStringAsync();
                    Console.WriteLine("Success response!: " + responseString);

                    if (responseString.Contains("200")) return true;
                    else return false;
                }

                return false;
            }
        }
    }
}
