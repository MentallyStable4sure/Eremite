using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using Eremite.Actions;
using Eremite.Services;
using Eremite.Data;
using Eremite.Data.DiscordData;

namespace Eremite.Commands
{
    public sealed class SacrificeCommand : BaseCommandModule
    {
        public DataHandler DataHandler { get; set; }

        [Command("sacrifice"), Description("Sacrifice character for some pills")]
        public async Task Sacrifice(CommandContext context, string name, string lastname)
        {
            var user = await DataHandler.GetData(context.User);

            var matchingCharacter = user
                .Characters
            .FirstOrDefault(character =>
                character.CharacterName.ToLower() == $"{name.ToLower()} {lastname.ToLower()}"
                || character.CharacterName.ToLower().Contains(name.ToLower()));

            if (matchingCharacter == null)
            {
                await context.RespondAsync($"> I didnt find this character in your character list, try calling it by name?");
                return;
            }

            if(matchingCharacter.SellPrice <= 0)
            {
                await context.RespondAsync($"> Cant sacrifice this character or sacrifice price is zero");
                return;
            }

            if (matchingCharacter.CharacterName == user.EquippedCharacter.CharacterName) SetCharacterAction.Dequip(user);
            user.Characters.Remove(matchingCharacter);

            user.Stats.TotalCharactersSacrificed += 1;
            user.Stats.TotalPillsEarned += matchingCharacter.SellPrice;

            var award = new Award(new DiscordWallet(0, 0, matchingCharacter.SellPrice));
            PerkAction.ApplyPerk(user, TimeGatedEventType.None, award);

            user.AddAward(award);

            var updateQuery = new QueryBuilder(user, QueryElement.EquippedCharacter, QueryElement.Characters, QueryElement.Wallet, QueryElement.Stats).BuildUpdateQuery();
            await DataHandler.SendData(user, updateQuery);

            await context.RespondAsync($"{user.Username} sacrificed {matchingCharacter.CharacterName} for {matchingCharacter.SellPrice} 💊");
        }

        [Command("sacrifice"), Description("Sacrifice character for some pills")]
        public async Task Sacrifice(CommandContext context, string name) => await Sacrifice(context, name, string.Empty);
    }
}
