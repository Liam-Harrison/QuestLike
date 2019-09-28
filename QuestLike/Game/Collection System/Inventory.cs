using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace QuestLike
{
    
    public class Inventory: IHaveCollections, ILocatable
    {
        [JsonRequired]
        CollectionManager manager = new CollectionManager();
        [JsonRequired]
        Locator locator;
        
        GameObject owner;
        [JsonRequired]
        int space = 10;

        public int MaxSpace
        {
            get
            {
                return space;
            }
        }

        public Inventory(GameObject owner)
        {
            this.owner = owner;
            locator = new Locator(owner);
            manager.AddCollection<Item>();
        }

        public Item[] GetItems
        {
            get
            {
                return manager.GetCollection<Item>().Objects;
            }
        }

        public bool EnoughSpace(Item item)
        {
            return EnoughSpace(item.Size);
        }

        public bool EnoughSpace(int size)
        {
            return space - UsedSpace - size >= 0;
        }

        public int UsedSpace
        {
            get
            {
                return GetCollection<Item>().ObjectList.Sum((a) => { return a.Size; });
            }
        }

        public Item AddItem(Item item)
        {
            if (EnoughSpace(item)) manager.GetCollection<Item>().AddObject(item);
            return item;
        }

        public bool AddItem(Item item, out Item back)
        {
            back = item;
            if (EnoughSpace(item))
            {
                manager.GetCollection<Item>().AddObject(item);
                return true;
            }
            return false;
        }

        public Item RemoveItem(Item item)
        {
            GetCollection<Item>().RemoveObject(item);
            return item;
        }

        public Collection<T> GetCollection<T>() where T : Collectable
        {
            return ((IHaveCollections)manager).GetCollection<T>();
        }

        public void AddCollection<T>() where T : Collectable
        {
            ((IHaveCollections)manager).AddCollection<T>();
        }

        public ICollection[] GetAllCollections()
        {
            return ((IHaveCollections)manager).GetAllCollections();
        }

        public bool HasCollection<T>() where T : Collectable
        {
            return ((IHaveCollections)manager).HasCollection<T>();
        }

        public GameObject[] Locate(string id, bool firstcall = true, bool overrideShow = false)
        {
            return ((ILocatable)locator).Locate(id, firstcall, overrideShow);
        }

        public void LocateSingleObject(string id, PromptObjectResponse<GameObject> response, bool firstcall = true, bool overrideShow = false)
        {
            ((ILocatable)locator).LocateSingleObject(id, response, firstcall, overrideShow);
        }

        public T[] LocateObjectsWithType<T>(bool firstcall = true, bool overrideShow = false) where T : class
        {
            return ((ILocatable)locator).LocateObjectsWithType<T>(firstcall, overrideShow);
        }

        public void LocateSingleObjectOfType<T>(PromptObjectResponse<T> response, bool firstcall = true, bool overrideShow = false) where T : class
        {
            ((ILocatable)locator).LocateSingleObjectOfType(response, firstcall, overrideShow);
        }

        public T[] LocateObjectsWithType<T>(string id, bool firstcall = true, bool overrideShow = false) where T : class
        {
            return ((ILocatable)locator).LocateObjectsWithType<T>(id, firstcall, overrideShow);
        }

        public void LocateSingleObjectOfType<T>(string id, PromptObjectResponse<T> response, bool firstcall = true, bool overrideShow = false) where T : class
        {
            ((ILocatable)locator).LocateSingleObjectOfType(id, response, firstcall, overrideShow);
        }

        public GameObject LocateWithGameID(int gameID)
        {
            return ((ILocatable)locator).LocateWithGameID(gameID);
        }
    }
}
