using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QuestLike.Organs;
using Newtonsoft.Json;

namespace QuestLike
{
    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public class EquipmentManager : IEquipable, IHaveCollections
    {
        [JsonProperty]
        private CollectionManager collectionManager;
        [JsonProperty(IsReference = true)]
        private BodyPart owner;
        public EquipmentManager(BodyPart owner)
        {
            collectionManager = new CollectionManager(owner);
            AddCollection<Item>();
            this.owner = owner;
        }

        [JsonIgnore]
        public Item EquipedItem
        {
            get
            {
                return GetCollection<Item>().Objects.FirstOrDefault();
            }
        }

        public void AddCollection<T>() where T : Collectable
        {
            ((IHaveCollections)collectionManager).AddCollection<T>();
        }

        public bool CanEquipItem(Equipable equipable)
        {
            return equipable.CanBodyPartEquip(owner);
        }

        public ICollection[] GetAllCollections()
        {
            return ((IHaveCollections)collectionManager).GetAllCollections();
        }

        public Collection<T> GetCollection<T>() where T : Collectable
        {
            return ((IHaveCollections)collectionManager).GetCollection<T>();
        }

        public bool HasCollection<T>() where T : Collectable
        {
            return ((IHaveCollections)collectionManager).HasCollection<T>();
        }

        [JsonIgnore]
        public bool HasItemEquiped
        {
            get
            {
                return EquipedItem != null;
            }
        }

        public Item TakeEquipedItem()
        {
            var item = EquipedItem;
            (item as Equipable).equiped = false;
            GetCollection<Item>().RemoveObject(EquipedItem);
            return item;
        }

        public Item EquipItem(Equipable equipable)
        {
            equipable.container.GetTypedCollection().RemoveObject(equipable);
            if (!HasItemEquiped) GetCollection<Item>().AddObject(equipable);
            equipable.equiped = true;
            return equipable;
        }
    }
}
