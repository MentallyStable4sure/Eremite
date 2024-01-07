using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using Eremite.Actions;
using Eremite.Base.Interfaces;
using Eremite.Builders;
using Eremite.Data;
using Eremite.Data.Localization;
using Eremite.Services;

namespace Eremite.Commands
{
    public sealed class LanguageCommand : BaseCommandModule
    {
        public DataHandler DataHandler { get; set; }

        private readonly string langChanged = "localization.lang_changed";
        private readonly string langNotFound = "localization.lang_not_found";

        [Command("language"), Description("Changes the bot language for corresponding user")]
        public async Task ChangeLanguage(CommandContext context, string languageName)
        {
            var user = await DataHandler.GetData(context.User);
            new InfoAction(DataHandler, context, user);
            languageName = languageName.ToLower();

            Language newLanguage = user.Stats.Language;
            if (languageName.Contains("en")) newLanguage = Language.English;
            if (languageName.Contains("fr")) newLanguage = Language.French;
            if (languageName.Contains("ua")) newLanguage = Language.Ukrainian;
            if (languageName.Contains("ru")) newLanguage = Language.Russian;

            if (newLanguage == user.Stats.Language)
            {   
                await context.RespondAsync($"> {user.GetText(langNotFound)}");
                return;
            }

            Localization.ChangeLanugage(user, newLanguage);
            await context.RespondAsync($"> {user.GetText(langChanged)}");

            IQueryBuilder query = new UserUpdateQueryBuilder(user, QueryElement.Stats);
            await DataHandler.SendData(user, query.Build());
        }

        [Command("lang"), Description("Changes the bot language for corresponding user")]
        public async Task ChangeLang(CommandContext context, string languageName) => await ChangeLanguage(context, languageName);
    }
}
