using Eremite.Data.DiscordData;
using System.Collections.ObjectModel;

namespace Eremite.Data
{
    public class ItemsDb
    {
        public static ReadOnlyDictionary<int, UserItem> FishingRods = new ReadOnlyDictionary<int, UserItem>(
            new Dictionary<int, UserItem>() 
            { 
                { 1, new UserItem(1, "<:Windtangler:1201123354380337183>", new Status(new Award(new DiscordWallet()),eventTypeToProc: TimeGatedEventType.Fishblasting, isDestroyable: false), new DiscordWallet(0, 5000), new DiscordWallet(0, 7500)) },
                { 2, new UserItem(2, "<:Serendipity:1201123350601273476>", new Status(new Award(new DiscordWallet()),eventTypeToProc: TimeGatedEventType.Fishblasting, isDestroyable: false), new DiscordWallet(0, 10000), new DiscordWallet(0, 18000)) },
                { 3, new UserItem(3, "<:Narukawa_Ukai:1201123348281823313>", new Status(new Award(new DiscordWallet()),eventTypeToProc: TimeGatedEventType.Fishblasting, isDestroyable : false), new DiscordWallet(0, 22000), new DiscordWallet(0, 44500)) },
                { 4, new UserItem(4, "<:Wavepiercer:1201123351968612362>", new Status(new Award(new DiscordWallet()),eventTypeToProc: TimeGatedEventType.Fishblasting, isDestroyable: false), new DiscordWallet(0, 35000), new DiscordWallet(0, 75500)) },
                { 5, new UserItem(5, "<:Moonstringer:1201123346688004136>", new Status(new Award(new DiscordWallet()),eventTypeToProc: TimeGatedEventType.Fishblasting, isDestroyable : false), new DiscordWallet(0, 60000), new DiscordWallet(0, 220000)) },
            });

        public static ReadOnlyDictionary<int, UserItem> Fishes = new ReadOnlyDictionary<int, UserItem>(
            new Dictionary<int, UserItem>()
            {
                { 6, new UserItem(6, "<:Aizen_Medaka:1201123112813609080>", new Status(new Award(new DiscordWallet())), new DiscordWallet(40, 200), new DiscordWallet(10, 450)) },
                { 7, new UserItem(7, "<:Colored_Shirakodai:1201123116324245524>", new Status(new Award(new DiscordWallet())), new DiscordWallet(85, 340, 10), new DiscordWallet(10, 850)) },
                { 8, new UserItem(8, "<:Rusty_Koi:1201123208301137950>", new Status(new Award(new DiscordWallet())), new DiscordWallet(125, 480, 25), new DiscordWallet(10, 1240)) },
                { 9, new UserItem(9, "<:Crystalfish:1201123120220733450>", new Status(new Award(new DiscordWallet())), new DiscordWallet(400, 870, 55), new DiscordWallet(10, 2700)) },
                { 10, new UserItem(10, "<:Golden_Koi:1201123125543305267>", new Status(new Award(new DiscordWallet())), new DiscordWallet(1200, 2550, 120), new DiscordWallet(10, 5500)) },
                { 11, new UserItem(11, "<:Dawncatcher:1201123121818763335>", new Status(new Award(new DiscordWallet())), new DiscordWallet(2600, 7000, 170), new DiscordWallet(10, 22200)) },
                { 12, new UserItem(12, "<:Akai_Maou:1201123114008981595>", new Status(new Award(new DiscordWallet())), new DiscordWallet(4400, 16800, 285), new DiscordWallet(10, 62000)) },
                { 13, new UserItem(13, "<:Peach_of_the_Deep_Waves:1201123133961277490>", new Status(new Award(new DiscordWallet())), new DiscordWallet(7800, 34400, 475), new DiscordWallet(10, 198000)) },
                { 14, new UserItem(14, "<:melusine_sticker:1200997903901134938>", new Status(new Award(new DiscordWallet(12200, 56800, 640))), new DiscordWallet(10, 10, 10), new DiscordWallet(1000, 640000)) },
            });

        //TODO: add items
        public static ReadOnlyDictionary<int, UserItem> Items = new ReadOnlyDictionary<int, UserItem>(
            new Dictionary<int, UserItem>()
            {
                { 15, new UserItem(15, "<:Jueyun_Chili:1201123127359447180>", new Status(new Award(new DiscordWallet()), eventCooldownDecrease: new TimeGatedEventType[2] { TimeGatedEventType.Daily, TimeGatedEventType.Adventure }, timeToDecrease: TimeSpan.FromHours(3)), new DiscordWallet(400, 2200), new DiscordWallet(200, 4400)) },

                //{ 16, new UserItem(16, "<:Colored_Shirakodai:1201123116324245524>", new Status(new Award(new DiscordWallet())), new DiscordWallet(85, 340, 10), new DiscordWallet(10, 850)) },
                //{ 17, new UserItem(17, "<:Rusty_Koi:1201123208301137950>", new Status(new Award(new DiscordWallet())), new DiscordWallet(125, 480, 25), new DiscordWallet(10, 1240)) },
                //{ 18, new UserItem(18, "<:Crystalfish:1201123120220733450>", new Status(new Award(new DiscordWallet())), new DiscordWallet(400, 870, 55), new DiscordWallet(10, 2700)) },
                //{ 19, new UserItem(19, "<:Golden_Koi:1201123125543305267>", new Status(new Award(new DiscordWallet())), new DiscordWallet(1200, 2550, 120), new DiscordWallet(10, 5500)) },
                //{ 20, new UserItem(20, "<:Dawncatcher:1201123121818763335>", new Status(new Award(new DiscordWallet())), new DiscordWallet(2600, 7000, 170), new DiscordWallet(10, 22200)) },
                //{ 21, new UserItem(21, "<:Akai_Maou:1201123114008981595>", new Status(new Award(new DiscordWallet())), new DiscordWallet(4400, 16800, 285), new DiscordWallet(10, 62000)) },
                //{ 22, new UserItem(22, "<:Peach_of_the_Deep_Waves:1201123133961277490>", new Status(new Award(new DiscordWallet())), new DiscordWallet(7800, 34400, 475), new DiscordWallet(10, 198000)) },
                //{ 23, new UserItem(23, "<:melusine_sticker:1200997903901134938>", new Status(new Award(new DiscordWallet(12200, 56800, 640))), new DiscordWallet(10, 10, 10), new DiscordWallet(1000, 640000)) },
            });

        internal static UserItem GetItemById(int itemId)
        {
            if(FishingRods.ContainsKey(itemId)) return FishingRods[itemId];
            if(Fishes.ContainsKey(itemId)) return Fishes[itemId];
            if(Items.ContainsKey(itemId)) return Items[itemId];

            return null;
        }
    }
}
