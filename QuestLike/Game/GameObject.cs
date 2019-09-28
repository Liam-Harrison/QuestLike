using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QuestLike.Combat;
using Microsoft.Xna.Framework;
using SadConsole;
using Console = SadConsole.Console;
using Global = SadConsole.Global;
using Microsoft.Xna.Framework.Design;
using Microsoft.Xna.Framework.Graphics;
using SadConsole.Input;
using QuestLike;
using Game = QuestLike.Game;
using Newtonsoft.Json;

namespace QuestLike
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
        
        internal Point position = new Point(-1, -1);
        
        internal Cell screenCell = new Cell(Color.White);
        
        internal char screenChar;
        
        public bool blocking = false;
        
        public bool searchable = true;

        public GameObject() : base()
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

        
        public Cell ScreenCell
        {
            get
            {
                return screenCell;
            }
        }

        
        public virtual string ScreenChar
        {
            get
            {
                return screenChar.ToString();
            }
        }

        
        public Point Position
        {
            get
            {
                return position;
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

        public virtual void Move(Direction direction)
        {
            var move = Pathfinding.DirectionToPoint(direction);
            var newposition = Position + move;
            position = newposition;
        }

        
        protected string equipedDescription
        {
            get
            {
                string text = "";
                bool wrote = false;
                if (this is Equipable)
                {
                    var casted = this as Equipable;
                    if (casted.IsEquiped)
                    {
                        text += $"\n\nEquiped to <{Color.Cyan.ToInteger()},look at {casted.EquipedTo.ID}>{casted.EquipedTo.Name}@";
                        wrote = true;
                    }
                }
                if (this is IEquipable)
                {
                    if (!wrote) text += "\n";
                    var casted = this as IEquipable;
                    if (casted.HasItemEquiped)
                    {
                        text += $"\nHas <{Color.Cyan.ToInteger()},look at {casted.EquipedItem.ID}>{casted.EquipedItem.Name}@ equiped.";
                    }
                }
                return text;
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
                        return $"\nHolding a <{Color.Cyan.ToInteger()},look at {item.ID}>{item.Name}@";
                    }
                    return "\nHolding no items.";
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
                    if (item.searchable) text += $"\n - <{Color.Cyan.ToInteger()},look at {item.ID}>{item.Name}@";
                }
                return text;
            }
        }

        
        protected virtual string interactionString
        {
            get
            {
                string text = "";

                if (this is Item)
                {
                    if ((this as Item).Grabable) text += $"[<{Color.Cyan.ToInteger()},grab {ID}>Grab@] ";
                }
                if (this is IUseable) text += $"[<{Color.Orange.ToInteger()},use {ID}>Use@] ";
                if (this is Equipable) text += $"[<{Color.Yellow.ToInteger()},equip {ID}>Equip@] ";
                if (this is ITalkable) text += $"[<{Color.Orange.ToInteger()},converse {ID}>Converse With@] ";

                if (text == "") return text;
                else return "\n\n" + text;
            }
        }

        
        protected string gameobjectString
        {
            get
            {
                return Name + ((ShortDescription == "") ? "" : " - " + ShortDescription) + ((Description == "") ? "" : "\n" + Description)
                    + equipedDescription + holdingDescription + inventoryDescription + objectsDescription + interactionString;
            }
        }

        public virtual void Update()
        {

        }

        public int AbsDistanceTo(GameObject other, int xScale = 2, int yScale = 1)
        {
            if ((Game.GetRoom.IsObjectInRoomDirectly(this) || this is Player) && 
                (Game.GetRoom.IsObjectInRoomDirectly(other) || other is Player))
            {
                int xDistance = Math.Abs(other.Position.X - Position.X) / xScale;
                int yDistance = Math.Abs(other.Position.Y - Position.Y) / yScale;
                return xDistance + yDistance;
            }
            else return 0;
        }

        public bool IsInRangeOf(GameObject other, Range range)
        {
            if ((Game.GetRoom.IsObjectInRoomDirectly(this) || this is Player) &&
                (Game.GetRoom.IsObjectInRoomDirectly(other) || other is Player))
            {
                var area = Pathfinding.SpillGetPoints(Position, range.X, range.Y, true, true);
                return Pathfinding.IsPointInBFSList(other.Position, area);
            }
            else return true;
        }

        public Collection<T> GetCollection<T>() where T : Collectable
        {
            return collectionManager.GetCollection<T>();
        }

        public void AddCollection<T>() where T : Collectable
        {
            collectionManager.AddCollection<T>();
        }

        public ICollection[] GetAllCollections()
        {
            return collectionManager.GetAllCollections();
        }

        public bool HasCollection<T>() where T : Collectable
        {
            return collectionManager.HasCollection<T>();
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
