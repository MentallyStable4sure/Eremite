using Eremite.Actions;
using Eremite.Services;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using Eremite.Data;
using Eremite.Builders;

namespace Eremite.Commands
{
    public sealed class SetCharacterCommand : BaseCommandModule
    {
        public DataHandler DataHandler { get; set; }
        public LocalizationHandler LocalizationHandler { get; set; }

        [Command("setcharacter"), Description("Sets character as a main equipped character")]
        public async Task SetCharacter(CommandContext context, string name, string lastname)
        {
            var user = await DataHandler.GetData(context.User);
            var characters = CharactersHandler.ConvertIds(user.Characters);

            var matchingCharacter = characters.FirstOrDefault(character => 
                character.CharacterName.ToLower() == $"{name.ToLower()} {lastname.ToLower()}" 
                || character.CharacterName.ToLower().Contains(name.ToLower()));

            if(matchingCharacter == null)
            {
                await context.RespondAsync($"> I didnt find this character in your character list, try calling it by name?");
                return;
            }

            if(matchingCharacter.CharacterId == user.EquippedCharacter)
            {
                await context.RespondAsync($"> You already equipped this character, try !akasha to see more info");
                return;
            }

            SetCharacterAction.Equip(user, matchingCharacter);

            var updateQuery = new UserUpdateQueryBuilder(user, QueryElement.EquippedCharacter).Build();
            await DataHandler.SendData(user, updateQuery);

            await context.Message.RespondAsync(SetCharacterAction.GetEmbedWithCharacterInfo(matchingCharacter));
        }

        [Command("setcharacter"), Description("Sets character as a main equipped character")]
        public async Task SetCharacter(CommandContext context, string name) => await SetCharacter(context, name, string.Empty);

        [Command("set"), Description("Sets character as a main equipped character")]
        public async Task Set(CommandContext context, string name) => await SetCharacter(context, name, string.Empty);
    }
}
