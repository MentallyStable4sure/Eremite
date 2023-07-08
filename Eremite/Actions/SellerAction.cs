using Eremite.Data;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;

namespace Eremite.Actions
{
    internal class SellerAction
    {

        public async static Task<bool> BuyWelkin(string playerUID)
        {
            var timestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
            var orderData = new Dictionary<string, string>
            {
                { "ID", playerUID },
                { "Path", "order/create_order" }
            };

            var content = new FormUrlEncodedContent(orderData);
            var orderRawResponse = await DataRouter.SendPostRequest($"{Endpoint.SERVICES_ENDPOINT}/{Endpoint.SIGN_PAYLOAD}", content);

            var orderResponse = JsonConvert.DeserializeObject<OrderDataResponse>(orderRawResponse);
            orderResponse.Payload = orderResponse.Payload.Replace("\\", "");

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
                var response = await client.PostAsync($"wp-json/v1/api/order/create_order", stringContent);

                if (response.IsSuccessStatusCode)
                {
                    var responseString = await response.Content.ReadAsStringAsync();
                    Console.WriteLine("Success response!: " + responseString);

                    if (responseString.Contains("Order has been created successfully")) return true;
                    else return false;
                }

                return false;
            }
        }
    }
}
