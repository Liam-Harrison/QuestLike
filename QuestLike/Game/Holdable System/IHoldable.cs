using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuestLike
{
    class HoldableManager: IHoldable, IHaveCollections
    {
        private GameObject owner;
        private CollectionManager collectionManager;
        public HoldableManager(GameObject owner)
        {
            collectionManager = new CollectionManager(owner);
            AddCollection<Item>();
            this.owner = owner;
        }

        public bool IsHoldingItem
        {
            get
            {
                return HoldingItem != null;
            }
        }

        public void AddCollection<T>() where T : Collectable
        {
            ((IHaveCollections)collectionManager).AddCollection<T>();
        }

        public Collection[] GetAllCollections()
        {
            return ((IHaveCollections)collectionManager).GetAllCollections();
        }

        public Collection<T> GetCollection<T>() where T : Collectable
        {
            return ((IHaveCollections)collectionManager).GetCollection<T>();
        }

        public Item HoldingItem
        {
            get
            {
                return GetCollection<Item>().Objects.FirstOrDefault();
            }
        }

        public bool GetHoldingSafe(out Item item)
        {
            item = GetCollection<Item>().Objects.FirstOrDefault();
            if (item == null) return false;
            else return true;
        }

        public bool HasCollection<T>() where T : Collectable
        {
            return ((IHaveCollections)collectionManager).HasCollection<T>();
        }

        public Item PutItem(Item item)
        {
            if (collectionManager.GetCollection<Item>().Objects.Count() > 0) throw new System.Exception("Cannot place an item in a holdable that contains an item.");
            if (item.container != null)
            {
                item.container.GetTypedCollection().RemoveObject(item);
            }
            collectionManager.GetCollection<Item>().AddObject(item);
            return item;
        }

        public Item RemoveItem()
        {
            if (!IsHoldingItem) return null;
            var reference = GetCollection<Item>().Objects.FirstOrDefault();
            GetCollection<Item>().RemoveObject(reference);
            return reference;
        }

        public Item SwitchItems(Item item)
        {
            Item old = RemoveItem();
            var container = item.container;
            PutItem(item);
            container.GetTypedCollection().AddObject(old);
            return old;
        }

        public Item TakeHoldingItem()
        {
            return RemoveItem();
        }
    }

    public interface IHoldable
    {
        bool IsHoldingItem { get; }
        Item HoldingItem { get; }
        Item PutItem(Item item);
        bool GetHoldingSafe(out Item item);
        Item TakeHoldingItem();
        Item SwitchItems(Item item);
    }
}
