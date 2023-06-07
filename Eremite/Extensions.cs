using Eremite.Data.DiscordData;

namespace Eremite
{
    public static class Extensions
    {

        public static bool IsValid(this UserData user) => user.UserId != null && user.UserId != string.Empty;

        public static void LogStatus(this string rawJson, string fileName = "")
        {
            bool isCorrupted = rawJson == null || rawJson.Length <= 0;

            string corruptedMessage = $"[ERROR] Couldnt load {fileName}";
            string successMessage = $"[SUCCESS] {fileName} loaded successfully";

            Console.WriteLine(isCorrupted ? corruptedMessage : successMessage);
        }
    }
}
