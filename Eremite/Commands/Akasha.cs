using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using Eremite.Data.DiscordData;
using Eremite.Services;

namespace Eremite.Commands
{
    public sealed class Akasha : BaseCommandModule
    {
        public DataHandler DataHandler { get; set; }

        public const string StarSign = ":star:";

        [Command("akasha"), Description("Shows the current user profile with the current equipped character, mora and primos")]
        public async Task ShowProfile(CommandContext context)
        {
            var userId = context.User.Id;
            var buttons = CreateButtons();
            var user = await DataHandler.GetData(userId.ToString());

            var currentCharacter = user.EquippedCharacter;

            string currentCharacterName = currentCharacter == null ? "None, use !pull to get one :)" : currentCharacter.CharacterName;
            string charactersInInventory = GetCharactersFromInventory(user);

            string characterBuffInfo = user.EquippedCharacter == null ? "None, use !setcharacter [name] or !pull to get one :)" : user.EquippedCharacter.PerkInfo;

            var messageBuilder = new DiscordMessageBuilder()
                .WithContent($"```elm\n[{user.Username}] [ID:{user.UserId}]\n\nMain Character: {currentCharacterName}" +
                $"\nCharacter Buff: {characterBuffInfo}\n\nTimes traveled: {user.Stats.TimesTraveled} | Teapot visited: {user.Stats.TimesTeapotVisited} times" +
                $"\n\nMora: {user.Wallet.Mora} | Primos: {user.Wallet.Primogems}\n\nCharacters Obtained: {charactersInInventory}```")
                .AddComponents(buttons.Keys);

            await context.RespondAsync(messageBuilder);
        }

        private Dictionary<DiscordButtonComponent, string> CreateButtons()
        {
            var shopGuid = Guid.NewGuid().ToString();
            var pullGuid = Guid.NewGuid().ToString();

            var pullButton = new DiscordButtonComponent(ButtonStyle.Success, pullGuid, "Pull");
            var shopButton = new DiscordButtonComponent(ButtonStyle.Secondary, shopGuid, "Mora Shop");

            var buttons = new Dictionary<DiscordButtonComponent, string>();
            buttons.Add(shopButton, shopGuid);
            buttons.Add(pullButton, pullGuid);

            return buttons;
        }

        private string GetCharactersFromInventory(UserData user)
        {
            string charactersInInventory = string.Empty;

            if(user.Characters.Count <= 0)
            {
                charactersInInventory = "None, use !pull to get one :)";
                return charactersInInventory;
            }

            foreach (var character in user.Characters)
            {
                charactersInInventory = $"{charactersInInventory} {character.CharacterName}<{character.StarsRarity}{StarSign}> ";
            }

            return charactersInInventory;
        }
    }
}
