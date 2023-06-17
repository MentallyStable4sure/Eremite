using DSharpPlus.Entities;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using Eremite.Services;
using Eremite.Actions;

namespace Eremite.Commands
{
    public sealed class ShopCommand : BaseCommandModule
    {
        public DataHandler DataHandler { get; set; }

        [Command("shop"), Description("Shows the current shop lots and prices from Dori herself.")]
        public async Task ShowStats(CommandContext context)
        {
            var dropdown = CreateDropdown(context);
            var user = await DataHandler.GetData(context.User);

            var messageBuilder = new DiscordMessageBuilder()
                .AddComponents(dropdown)
                .WithEmbed(ShopAction.GetEmbedWithShopInfo());

            await context.RespondAsync(messageBuilder);
        }

        private DiscordSelectComponent CreateDropdown(CommandContext context)
        {
            var options = new List<DiscordSelectComponentOption>()
            {
                new DiscordSelectComponentOption(
                    "3000 Mora --> 100 Primogems",
                    "3000morainto100primogems",
                    "Thats a great deal, trust Dori!",
                    emoji: new DiscordComponentEmoji(1113103136991756328)),

                new DiscordSelectComponentOption(
                    "400 Pills --> Crimson Witch Hat",
                    "300pillsintosignora",
                    "Will make anyone a pyro witch!",
                    emoji: new DiscordComponentEmoji(725178213944262716)),

                new DiscordSelectComponentOption(
                    "2000 Pills --> Welkin Moon",
                    "2000pillsintowelkinmoon",
                    "Its 1000 Pills in MentallyStable launcher, but tell no one (u can download it from official site in bot info)",
                    emoji: new DiscordComponentEmoji(765128125301915658)),

                new DiscordSelectComponentOption(
                    "10000 Mora --> 100 Pills",
                    "10000morainto100pills",
                    "I love mora! And Mora loves me! Hehe~",
                    emoji : new DiscordComponentEmoji(772774919607025665))
            };

            return new DiscordSelectComponent("dropdown", "Dori will get your item from market", options, false, 1, 2);
        }
    }
}
