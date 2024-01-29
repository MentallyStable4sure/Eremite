using Eremite.Data;
using Eremite.Data.DiscordData;

namespace Eremite.Actions
{
    public class InventoryAction
    {
        private UserData user;

        public InventoryAction(UserData user)
        {
            this.user = user;
        }

        public void AddItem(UserItem item)
        {
            if (item == null) return;
            var foundItem = user.Inventory.FindIndex(itemToSearch => item.ItemId == itemToSearch.ItemId);

            if (foundItem == -1) user.Inventory.Add(item);
            else user.Inventory[foundItem].Amount += 1;
        }

        public void AddItem(int itemId) => AddItem(ItemsDb.GetItemById(itemId));

        public void RemoveItem(UserItem item)
        {
            if (item == null) return;
            var foundItem = user.Inventory.FindIndex(itemToSearch => item.ItemId == itemToSearch.ItemId);

            if (foundItem == -1) return;
            user.Inventory[foundItem].Amount -= 1;
            
            if(user.Inventory[foundItem].Amount <= 0) user.Inventory.RemoveAt(foundItem);
        }
    }
}
