using Eremite.Actions;
using Eremite.Services;
using Eremite.Data;
using Eremite.Data.DiscordData;
using Eremite.Builders;
using DSharpPlus.SlashCommands;
using DSharpPlus.Entities;

namespace Eremite.SlashCommands
{
    public sealed class SacrificeCommand : ApplicationCommandModule
    {
        public DataHandler DataHandler { get; set; }

        private readonly string sacrificeCharacterNotFound = "sacrifice.character_not_found";
        private readonly string cantSacrificeCharacterError = "sacrifice.error";
        private readonly string sacrificed = "sacrifice.sacrificed";

        [SlashCommand("sacrificelong", "Sacrifice character for some pills with long name")]
        public async Task Sacrifice(InteractionContext context, [Option("name", "first name of the character")] string name, [Option("lastname", "lastname of the character")]  string lastname)
        {
            var user = await DataHandler.GetData(context.User);
            new InfoAction(DataHandler, context, user);
            var message = new DiscordInteractionResponseBuilder();

            var characters = CharactersHandler.ConvertIds(user.Characters);
            var currentCharacter = CharactersHandler.ConvertId(user.EquippedCharacter);

            var matchingCharacter = characters.FirstOrDefault(character =>
                character.CharacterName.ToLower() == $"{name.ToLower()} {lastname.ToLower()}"
                || character.CharacterName.ToLower().Contains(name.ToLower()));

            if (matchingCharacter == null)
            {
                await context.CreateResponseAsync(message.WithContent($"> {user.GetText(sacrificeCharacterNotFound)}"));
                return;
            }

            if (matchingCharacter.SellPrice <= 0)
            {
                await context.CreateResponseAsync(message.WithContent($">{user.GetText(cantSacrificeCharacterError)}"));
                return;
            }

            if (matchingCharacter.CharacterName == currentCharacter.CharacterName) SetCharacterAction.Dequip(user);
            user.RemovePulledCharacter(matchingCharacter);

            user.Stats.TotalCharactersSacrificed += 1;
            user.Stats.TotalPillsEarned += matchingCharacter.SellPrice;

            var award = new Award(new DiscordWallet(0, 0, matchingCharacter.SellPrice));
            var perkAction = new PerkHandler(DataHandler);
            string additionalMessage = perkAction.ApplyPerk(user, new TimeGatedEvent(TimeGatedEventType.Sacrifice, TimeSpan.Zero), award);

            user.AddAward(award);

            var updateQuery = new UserUpdateQueryBuilder(user, QueryElement.EquippedCharacter, QueryElement.Characters, QueryElement.Wallet, QueryElement.Stats, QueryElement.Events).Build();
            await DataHandler.SendData(user, updateQuery);

            await context.CreateResponseAsync(message.WithContent($"{user.Username} {user.GetText(sacrificed)} {matchingCharacter.CharacterName} [{matchingCharacter.SellPrice} {Services.Localization.PillsEmoji}]\n{additionalMessage}"));
        }

        [SlashCommand("sacrifice", "Sacrifice character for some pills")]
        public async Task Sacrifice(InteractionContext context, [Option("name", "first name of the character")] string name) => await Sacrifice(context, name, string.Empty);
    }
}
