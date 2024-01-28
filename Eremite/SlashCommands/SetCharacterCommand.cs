using Eremite.Actions;
using Eremite.Services;
using Eremite.Data;
using Eremite.Builders;
using DSharpPlus.SlashCommands;
using DSharpPlus.Entities;

namespace Eremite.SlashCommands
{
    public sealed class SetCharacterCommand : ApplicationCommandModule
    {
        public DataHandler DataHandler { get; set; }

        private readonly string characterNotFound = "setcharacter.not_found";
        private readonly string alreadyEquipped = "setcharacter.already_equipped";

        [SlashCommand("setcharacterlong", "Sets character as a main equipped character with a long name")]
        public async Task SetCharacter(InteractionContext context, [Option("name", "first name of the character")] string name, [Option("lastname", "lastname of the character")] string lastname)
        {
            var user = await DataHandler.GetData(context.User);
            new InfoAction(DataHandler, context, user);
            var message = new DiscordInteractionResponseBuilder();

            var characters = CharactersHandler.ConvertIds(user.Characters);

            var matchingCharacter = characters.FirstOrDefault(character =>
                character.CharacterName.ToLower() == $"{name.ToLower()} {lastname.ToLower()}"
                || character.CharacterName.ToLower().Contains(name.ToLower()));

            if (matchingCharacter == null)
            {
                await context.CreateResponseAsync(message.WithContent($"> {user.GetText(characterNotFound)}"));
                return;
            }

            if (matchingCharacter.CharacterId == user.EquippedCharacter)
            {
                await context.CreateResponseAsync(message.WithContent($"> {user.GetText(alreadyEquipped)}"));
                return;
            }

            SetCharacterAction.Equip(user, matchingCharacter);

            var updateQuery = new UserUpdateQueryBuilder(user, QueryElement.EquippedCharacter).Build();
            await DataHandler.SendData(user, updateQuery);

            await context.CreateResponseAsync(message.AddEmbed(SetCharacterAction.GetEmbedWithCharacterInfo(user, matchingCharacter)));
        }

        [SlashCommand("setcharacter", "Sets character as a main equipped character")]
        public async Task SetCharacter(InteractionContext context, [Option("name", "first name of the character")] string name) => await SetCharacter(context, name, string.Empty);

        [SlashCommand("set", "Sets character as a main equipped character")]
        public async Task Set(InteractionContext context, [Option("name", "first name of the character")] string name) => await SetCharacter(context, name, string.Empty);
    }
}
