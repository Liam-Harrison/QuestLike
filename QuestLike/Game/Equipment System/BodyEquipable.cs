using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QuestLike.Organs;
using Newtonsoft.Json;

namespace QuestLike
{
    [JsonObject(MemberSerialization = MemberSerialization.OptOut)]
    public abstract class Equipable : Item
    {
        public Equipable() :base()
        {

        }

        public Equipable(string name, string[] ids) : base(name, ids)
        {
        }

        public Equipable(string name, string desc, string[] ids) : base(name, desc, ids)
        {
        }

        public Equipable(string name, string shortdesc, string desc, string[] ids) : base(name, shortdesc, desc, ids)
        {
        }

        public abstract bool CanBodyPartEquip(BodyPart part);

        [JsonProperty(IsReference = true)]
        public abstract GameObject EquipedTo { get; }

        public abstract bool IsEquiped { get; }

        public bool equiped = false;
    }

    [JsonObject(MemberSerialization = MemberSerialization.OptOut)]
    public class Equipable<T>: Equipable where T: Item
    {
        public Equipable() : base()
        {

        }

        public Equipable(string name, string[] ids) : base(name, ids)
        {
        }

        public Equipable(string name, string desc, string[] ids) : base(name, desc, ids)
        {
        }

        public Equipable(string name, string shortdesc, string desc, string[] ids) : base(name, shortdesc, desc, ids)
        {
        }

        public override GameObject EquipedTo
        {
            get
            {
                if (!IsEquiped) return null;
                return container.Owner;
            }
        }

        public override bool IsEquiped
        {
            get
            {
                return equiped;
            }
        }

        public override bool CanBodyPartEquip(BodyPart part)
        {
            return part is T;
        }
    }
}
