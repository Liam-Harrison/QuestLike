using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZorkLike.Combat;
using Microsoft.Xna.Framework;

namespace ZorkLike
{
    public abstract class GameObject : Collectable, IHaveCollections, ILocatable
    {
        protected CollectionManager collectionManager;
        protected Locator locator;
        private string name = "";
        private string description = "";
        private string shortDescription = "";
        private int size = 1;
        private static uint nextID = 0;
        private uint id;
        internal Point screenposition = new Point(-1, -1);
        internal char screenChar;

        public string ScreenChar
        {
            get
            {
                return screenChar.ToString();
            }
        }

        public bool PrintOnScreen
        {
            get
            {
                return screenposition.X >= 0 && screenposition.Y >= 0;
            }
        }

        public Point ScreenPosition
        {
            get
            {
                return screenposition;
            }
        }

        public int ID
        {
            get
            {
                return (int)id;
            }
        }

        public string Name
        {
            get
            {
                return name;
            }
            set
            {
                name = value;
            }
        }

        public int Size
        {
            get
            {
                return size;
            }
            set
            {
                size = value;
            }
        }

        public string Description
        {
            get
            {
                return description;
            }
            set
            {
                description = value;
            }
        }

        public string ShortDescription
        {
            get
            {
                return shortDescription;
            }
            set
            {
                shortDescription = value;
            }
        }

        public virtual string Examine
        {
            get
            {
                return gameobjectString;
            }
        }

        protected string holdingDescription
        {
            get
            {
                if (this is IHoldable)
                {
                    var casted = this as IHoldable;
                    if (casted.GetHoldingSafe(out Item item))
                    {
                        return $"\n\nHolding a <{Color.Cyan.ToInteger()},look at {item.ID}>{item.Name}@";
                    }
                    return "Holding no items.";
                }
                return "";
            }
        }

        protected string inventoryDescription
        {
            get
            {
                if (this is IInventory)
                {
                    var casted = this as IInventory;
                    var text = $"\n\nThese items are in this inventory ({casted.GetInventory.UsedSpace}/{casted.GetInventory.MaxSpace} space used):";
                    if (casted.GetInventory.GetItems.Count() > 0)
                    {
                        foreach (var item in casted.GetInventory.GetItems)
                        {
                            text += $"\n - <{Color.Cyan.ToInteger()},look at {item.ID}>{item.Name}@";
                        }
                        return text;
                    }
                    return "\n\nNo items in this inventory.";
                }
                return "";
            }
        }

        public string objectsDescription
        {
            get
            {
                if (!HasCollection<GameObject>()) return "";
                string text = "\n\nHas these items:";
                if (GetCollection<GameObject>().Objects.Count() == 0) return "\n\nContains nothing.";
                foreach (var item in GetCollection<GameObject>().Objects)
                {
                    text += $"\n - <{Color.Cyan.ToInteger()},look at {item.ID}>{item.Name}@";
                }
                return text;
            }
        }

        protected virtual string interactionString
        {
            get
            {
                string text = "";

                if (this is Item) text += $"[<{Color.Cyan.ToInteger()},grab {ID}>Grab@] ";
                if (this is IUseable) text += $"[<{Color.Orange.ToInteger()},use {ID}>Use@] ";
                if (this is BodyEquipable) text += $"[<{Color.Yellow.ToInteger()},equip {ID}>Equip@] ";

                if (text == "") return text;
                else return "\n\n" + text;
            }
        }

        protected string gameobjectString
        {
            get
            {
                return Name + ((ShortDescription == "") ? "" : " - " + ShortDescription) + ((Description == "") ? "" : "\n" + Description)
                    + holdingDescription + inventoryDescription + objectsDescription + interactionString;
            }
        }

        public virtual void Update()
        {

        }

        public GameObject(string name, string[] ids) : base(ids)
        {
            locator = new Locator(this);
            collectionManager = new CollectionManager(this);
            this.name = name;
            id = nextID++;
        }

        public GameObject(string name, string shortdesc, string desc, string[] ids) : this(name, ids)
        {
            this.shortDescription = shortdesc;
            this.description = desc;
        }

        public GameObject(string name, string desc, string[] ids) : this(name, ids)
        {
            this.description = desc;
        }

        public Collection<T> GetCollection<T>() where T : Collectable
        {
            return collectionManager.GetCollection<T>();
        }

        public void AddCollection<T>() where T : Collectable
        {
            collectionManager.AddCollection<T>();
        }

        public Collection[] GetAllCollections()
        {
            return collectionManager.GetAllCollections();
        }

        public bool HasCollection<T>() where T : Collectable
        {
            return collectionManager.HasCollection<T>();
        }

        public GameObject[] Locate(string id, bool firstcall = true)
        {
            return ((ILocatable)locator).Locate(id, firstcall);
        }

        public void LocateSingleObject(string id, PromptObjectResponse<GameObject> response, bool firstcall = true)
        {
            ((ILocatable)locator).LocateSingleObject(id, response, firstcall);
        }

        public T[] LocateObjectsWithType<T>(bool firstcall = true) where T : class
        {
            return ((ILocatable)locator).LocateObjectsWithType<T>(firstcall);
        }

        public void LocateSingleObjectOfType<T>(PromptObjectResponse<T> response, bool firstcall = true) where T : class
        {
            ((ILocatable)locator).LocateSingleObjectOfType(response, firstcall);
        }

        public T[] LocateObjectsWithType<T>(string id, bool firstcall = true) where T : class
        {
            return ((ILocatable)locator).LocateObjectsWithType<T>(id, firstcall);
        }

        public void LocateSingleObjectOfType<T>(string id, PromptObjectResponse<T> response, bool firstcall = true) where T : class
        {
            ((ILocatable)locator).LocateSingleObjectOfType(id, response, firstcall);
        }

        public IHoldable[] LocateHoldables(string id, bool firstcall = true)
        {
            return ((ILocatable)locator).LocateHoldables(id, firstcall);
        }

        public void LocateSingleHoldableObject(string id, PromptObjectResponse<IHoldable> response, bool firstcall = true)
        {
            ((ILocatable)locator).LocateSingleHoldableObject(id, response, firstcall);
        }

        public IInventory[] LocateInventories(string id, bool firstcall = true)
        {
            return ((ILocatable)locator).LocateInventories(id, firstcall);
        }

        public void LocateSingleInventory(string id, PromptObjectResponse<IInventory> response, bool firstcall = true)
        {
            ((ILocatable)locator).LocateSingleInventory(id, response, firstcall);
        }

        public GameObject LocateWithGameID(int gameID)
        {
            return ((ILocatable)locator).LocateWithGameID(gameID);
        }
    }
}
