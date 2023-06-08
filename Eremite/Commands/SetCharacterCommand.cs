﻿using Eremite.Actions;
using Eremite.Services;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;

namespace Eremite.Commands
{
    public sealed class SetCharacterCommand : BaseCommandModule
    {
        public DataHandler DataHandler { get; set; }

        [Command("setcharacter"), Description("Sets character as a main equipped character")]
        public async Task SetCharacter(CommandContext context, string name, string lastname)
        {
            var user = await DataHandler.GetData(context.User);

            var matchingCharacter = user
                .Characters
                .FirstOrDefault(character => 
                character.CharacterName.ToLower() == $"{name.ToLower()} {lastname.ToLower()}" 
                || character.CharacterName.ToLower().Contains(name.ToLower()));

            if(matchingCharacter == null)
            {
                await context.RespondAsync($"> I didnt find this character in your character list, try calling it by name?");
                return;
            }

            if(matchingCharacter == user.EquippedCharacter)
            {
                await context.RespondAsync($"> You already equipped this character, try !akasha to see more info");
                return;
            }

            SetCharacterAction.Equip(user, matchingCharacter);
            await DataHandler.SendData(user, QueryHandler.GetUserUpdateCharactersQuery(user));

            await context.Message.RespondAsync(SetCharacterAction.GetEmbedWithEquippedCharacter(matchingCharacter));
        }

        [Command("setcharacter"), Description("Sets character as a main equipped character")]
        public async Task SetCharacter(CommandContext context, string name) => await SetCharacter(context, name, string.Empty);
    }
}