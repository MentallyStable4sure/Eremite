using Eremite.Builders;
using Eremite.Data.DiscordData;
using Eremite.Data;
using Eremite.Services;
using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;

namespace Eremite.Actions
{
    public class InfoAction
    {
        private DataHandler data;
        private UserData? userAffected;
        private InteractionContext currentContext;

        public InfoAction(DataHandler data, InteractionContext context, UserData? user = null)
        {
            this.data = data;
            userAffected = user;
            currentContext = context;

            ShowInfoMessage().ConfigureAwait(false);
        }

        public bool CanShowMessage(UserData user) => CheckMessageTimeStamp(user.Stats.NotifyShowTimestamp);

        private bool CheckMessageTimeStamp(DateTime timestampUTC) => IsTickAllowedToday(timestampUTC, data.Config.NotifyOfTheDayCooldown);

        public bool IsTickAllowedToday(DateTime eventTriggered, TimeSpan cooldown)
        {
            if (data.Config.NotifyOfTheDay == string.Empty || data.Config.NotifyOfTheDay.Length <= 1) return false;

            var dayTickAllowed = eventTriggered.Add(cooldown); //start date + time needed
            return DateTime.Compare(DateTime.UtcNow, dayTickAllowed) >= 0;
        }

        public async Task ShowInfoMessage()
        {
            if (userAffected == null) userAffected = await data.GetData(currentContext.User);
            if (!CanShowMessage(userAffected)) return;

            userAffected.Stats.NotifyShowTimestamp = DateTime.UtcNow;

            var updateQuery = new UserUpdateQueryBuilder(userAffected, QueryElement.Stats).Build();
            await data.SendData(userAffected, updateQuery);
            await currentContext.CreateResponseAsync(new DiscordInteractionResponseBuilder().AddEmbed(new DiscordEmbedBuilder()
            {
                Color = DiscordColor.Yellow,
                ImageUrl = data.Config.NotifyImage,
                Description = data.Config.NotifyOfTheDay
            }.Build()));
        }
    }
}
