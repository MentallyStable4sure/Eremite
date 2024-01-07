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

        private readonly string characterNotFound = "setcharacter.not_found";
        private readonly string alreadyEquipped = "setcharacter.already_equipped";

        [Command("setcharacter"), Description("Sets character as a main equipped character")]
        public async Task SetCharacter(CommandContext context, string name, string lastname)
        {
            var user = await DataHandler.GetData(context.User);
            new InfoAction(DataHandler, context, user);

            var characters = CharactersHandler.ConvertIds(user.Characters);

            var matchingCharacter = characters.FirstOrDefault(character => 
                character.CharacterName.ToLower() == $"{name.ToLower()} {lastname.ToLower()}" 
                || character.CharacterName.ToLower().Contains(name.ToLower()));

            if(matchingCharacter == null)
            {
                await context.RespondAsync($"> {user.GetText(characterNotFound)}");
                return;
            }

            if(matchingCharacter.CharacterId == user.EquippedCharacter)
            {
                await context.RespondAsync($"> {user.GetText(alreadyEquipped)}");
                return;
            }

            SetCharacterAction.Equip(user, matchingCharacter);

            var updateQuery = new UserUpdateQueryBuilder(user, QueryElement.EquippedCharacter).Build();
            await DataHandler.SendData(user, updateQuery);

            await context.Message.RespondAsync(SetCharacterAction.GetEmbedWithCharacterInfo(user, matchingCharacter));
        }

        [Command("setcharacter"), Description("Sets character as a main equipped character")]
        public async Task SetCharacter(CommandContext context, string name) => await SetCharacter(context, name, string.Empty);

        [Command("set"), Description("Sets character as a main equipped character")]
        public async Task Set(CommandContext context, string name) => await SetCharacter(context, name, string.Empty);
    }
}
