using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZorkLike
{
    class HoldableManager: IHoldable
    {
        private GameObject owner;
        private Item myItem;
        public HoldableManager(GameObject owner)
        {
            this.owner = owner;
        }

        public bool IsHoldingItem
        {
            get
            {
                return myItem != null;
            }
        }

        public Item GetHoldingItem()
        {
            return myItem;
        }

        public bool GetHoldingItemSafe(out Item item)
        {
            item = myItem;
            if (IsHoldingItem) return true;
            return false;
        }

        public Item PutItem(Item item)
        {
            myItem = item;
            return item;
        }

        public Item SwitchItems(Item item)
        {
            Item temp = myItem;
            myItem = item;
            return temp;
        }

        public Item TakeHoldingItem()
        {
            return SwitchItems(null);
        }
    }

    public interface IHoldable
    {
        bool IsHoldingItem { get; }
        Item PutItem(Item item);
        Item GetHoldingItem();
        bool GetHoldingItemSafe(out Item item);
        Item TakeHoldingItem();
        Item SwitchItems(Item item);
    }
}
