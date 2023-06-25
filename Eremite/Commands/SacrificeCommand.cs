using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using Eremite.Actions;
using Eremite.Services;
using Eremite.Data;
using Eremite.Data.DiscordData;
using Eremite.Builders;

namespace Eremite.Commands
{
    public sealed class SacrificeCommand : BaseCommandModule
    {
        public DataHandler DataHandler { get; set; }

        private readonly string sacrificeCharacterNotFound = "sacrifice.character_not_found";
        private readonly string cantSacrificeCharacterError = "sacrifice.error";
        private readonly string sacrificed = "sacrifice.sacrificed";

        [Command("sacrifice"), Description("Sacrifice character for some pills")]
        public async Task Sacrifice(CommandContext context, string name, string lastname)
        {
            var user = await DataHandler.GetData(context.User);
            var characters = CharactersHandler.ConvertIds(user.Characters);
            var currentCharacter = CharactersHandler.ConvertId(user.EquippedCharacter);

            var matchingCharacter = characters.FirstOrDefault(character =>
                character.CharacterName.ToLower() == $"{name.ToLower()} {lastname.ToLower()}"
                || character.CharacterName.ToLower().Contains(name.ToLower()));

            if (matchingCharacter == null)
            {
                await context.RespondAsync($"> {Localization.GetText(sacrificeCharacterNotFound)}");
                return;
            }

            if(matchingCharacter.SellPrice <= 0)
            {
                await context.RespondAsync($">{Localization.GetText(cantSacrificeCharacterError)}");
                return;
            }

            if (matchingCharacter.CharacterName == currentCharacter.CharacterName) SetCharacterAction.Dequip(user);
            user.RemovePulledCharacter(matchingCharacter);

            user.Stats.TotalCharactersSacrificed += 1;
            user.Stats.TotalPillsEarned += matchingCharacter.SellPrice;

            var award = new Award(new DiscordWallet(0, 0, matchingCharacter.SellPrice));
            PerkAction.ApplyPerk(user, TimeGatedEventType.None, award);

            user.AddAward(award);

            var updateQuery = new UserUpdateQueryBuilder(user, QueryElement.EquippedCharacter, QueryElement.Characters, QueryElement.Wallet, QueryElement.Stats).Build();
            await DataHandler.SendData(user, updateQuery);

            await context.RespondAsync($"{user.Username} {Localization.GetText(sacrificed)} {matchingCharacter.CharacterName} [{matchingCharacter.SellPrice} {Localization.GetText(Localization.PillsKey)}]");
        }

        [Command("sacrifice"), Description("Sacrifice character for some pills")]
        public async Task Sacrifice(CommandContext context, string name) => await Sacrifice(context, name, string.Empty);
    }
}
