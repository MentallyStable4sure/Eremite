using Eremite.Services;

namespace Eremite.Data.DiscordData
{
    [Serializable]
    public class Status
    {
        public Award AwardToGive = new Award(new DiscordWallet());
        public Perk OnPerkProc = Perk.NO_BUFF;
        public TimeGatedEventType EventTypeToProc = TimeGatedEventType.None;

        public bool IsDestroyable = true;
        public bool IsImmediateUse = false;
    }
}
