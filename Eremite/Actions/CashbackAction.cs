using Eremite.Services;
using Eremite.Data.DiscordData;
using Eremite.Base.Interfaces;

namespace Eremite.Actions
{
    public class CashbackAction : IEremiteService
    {
        public DataHandler DataHandler { get; set; }

        private readonly UserData user;

        public CashbackAction(DataHandler dataHandler, UserData user)
        {
            DataHandler = dataHandler;
            this.user = user;
        }

        private const string cashbackKey = "pull.cashback";


        public int GetCashbackForCharacter(int characterId)
        {
            if (!user.IsUserHasThisCharacter(characterId)) return 0;

            return Random.Shared.Next(0, DataHandler.Config.MaxCashback);
        }

        public string GetCashbackMessage(int cashback) => GetCashbackMessage(user, cashback);

        public static string GetCashbackMessage(UserData user, int cashback)
        {
            if (cashback <= 0) return string.Empty;

            return $"> {Localization.GetText(user.Stats.Language, cashbackKey)} {cashback} {Localization.MoraEmoji}";
        }
    }
}
