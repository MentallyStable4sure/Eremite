using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;
using Eremite.Actions;
using Eremite.Base.Interfaces;
using Eremite.Builders;
using Eremite.Data;
using Eremite.Data.Localization;
using Eremite.Services;

namespace Eremite.SlashCommands
{
    public sealed class LanguageCommand : ApplicationCommandModule
    {
        public DataHandler DataHandler { get; set; }

        private readonly string langChanged = "localization.lang_changed";
        private readonly string langNotFound = "localization.lang_not_found";

        [SlashCommand("language", "Changes the bot language for corresponding user")]
        public async Task ChangeLanguage(InteractionContext context, [Option("languageName", "Language short name ('fr', 'en', 'ua', etc.)")]  string languageName)
        {
            var user = await DataHandler.GetData(context.User);
            new InfoAction(DataHandler, context, user);
            languageName = languageName.ToLower();
            var message = new DiscordFollowupMessageBuilder();

            Language newLanguage = user.Stats.Language;
            if (languageName.Contains("en")) newLanguage = Language.English;
            if (languageName.Contains("fr")) newLanguage = Language.French;
            if (languageName.Contains("ua")) newLanguage = Language.Ukrainian;
            if (languageName.Contains("ru")) newLanguage = Language.Russian;

            if (newLanguage == user.Stats.Language)
            {
                await context.FollowUpAsync(message.WithContent($"> {user.GetText(langNotFound)}"));
                return;
            }

            Services.Localization.ChangeLanugage(user, newLanguage);
            await context.FollowUpAsync(message.WithContent($"> {user.GetText(langChanged)}"));

            IQueryBuilder query = new UserUpdateQueryBuilder(user, QueryElement.Stats);
            await DataHandler.SendData(user, query.Build());
        }

        [SlashCommand("lang", "Changes the bot language for corresponding user")]
        public async Task ChangeLang(InteractionContext context, [Option("languageName", "Language short name ('fr', 'en', 'ua', etc.)")]  string languageName) => await ChangeLanguage(context, languageName);
    }
}
