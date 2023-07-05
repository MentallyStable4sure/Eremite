using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text;

namespace Eremite.Actions
{
    internal class SellerAction
    {

        public async static Task BuyWelkin(string playerUID) //my UID to check things: 708617087 //TODO delete later
        {

            //Obtain Product ID from Product Detail API
            string payload2 = @"
            {
              ""path"": ""order/create_order"",
              ""data"": {
                ""category"": ""1"",
                ""product-id"": ""428075"",
                ""quantity"": ""1"",
                ""User ID"": ' ""USER_ID"",
                ""Server"": ""os_euro""
              }
            }";

            string payload = "{\r\n\"path\": \"user/balance\"\r\n}";

            string jsonData = payload.Replace("playerID", playerUID);

            var timestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
            var path = "user/balance";

            //Signature Generation (It is same for every API method)
            var STRING_TO_SIGN = jsonData + timestamp + path;
            var secretKeyBytes = Encoding.UTF8.GetBytes("SECRET_KEY");
            using (var hmac = new HMACSHA256(secretKeyBytes))
            {
                var hashBytes = hmac.ComputeHash(Encoding.UTF8.GetBytes(STRING_TO_SIGN));
                var auth = Convert.ToBase64String(hashBytes);
                var auth_basic = Convert.ToBase64String(Encoding.UTF8.GetBytes("PARTNER_ID:SECRET_KEY"));

                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri("https://moogold.com/");
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    client.DefaultRequestHeaders.Add("timestamp", timestamp.ToString());
                    client.DefaultRequestHeaders.Add("auth", auth);
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", auth_basic);

                    var stringContent = new StringContent(payload, Encoding.UTF8, "application/json");
                    var response = await client.PostAsync(path, stringContent);

                    Console.WriteLine(response); //response to make sure
                    if (response.IsSuccessStatusCode)
                    {
                        var responseString = await response.Content.ReadAsStringAsync();
                        Console.WriteLine(responseString);
                    }
                }
            }
        }
    }
}
