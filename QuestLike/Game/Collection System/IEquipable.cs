using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuestLike
{
    internal interface IEquipable
    {
        bool HasItemEquiped { get; }
        Item EquipedItem { get; }
        bool CanEquipItem(Equipable equipable);
        Item EquipItem(Equipable equipable);
    }
}
