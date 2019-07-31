using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QuestLike.Combat;

namespace QuestLike
{
    public class Locator: ILocatable
    {
        private IHaveCollections owner;
        private bool freezeSearchesBelow = true;
        public Locator(IHaveCollections owner)
        {
            this.owner = owner;
        }

        public bool FreezeSearchesBelow
        {
            get
            {
                return freezeSearchesBelow;
            }
            set
            {
                freezeSearchesBelow = value;
            }
        }

        public GameObject[] Locate(string id, bool firstcall = true)
        {
            List<GameObject> toReturn = new List<GameObject>();

            if (owner is GameObject)
            {
                GameObject casted = owner as GameObject;
                if (casted.HasID(id) && casted.searchable) toReturn.Add(casted);
            }

            if (freezeSearchesBelow && firstcall) return toReturn.ToArray();
            foreach (var collection in owner.GetAllCollections())
            {
                dynamic typed = Convert.ChangeType(collection, collection.CollectionType);
                foreach (var obj in typed.GetAllObjects())
                {
                    toReturn.AddRange(obj.Locate(id, false));
                }
            }
            if (owner is IInventory)
            {
                var casted = owner as IInventory;
                foreach (var obj in casted.GetInventory.GetItems)
                {
                    toReturn.AddRange(obj.Locate(id, false));
                }
            }
            if (owner is IHoldable)
            {
                var casted = owner as IHoldable;
                if (casted.GetHoldingSafe(out Item temp))
                    toReturn.AddRange(temp.Locate(id, true));
            }
            if (owner is IEquipable)
            {
                var casted = owner as IEquipable;
                if (casted.HasItemEquiped)
                {
                    toReturn.AddRange(casted.EquipedItem.Locate(id, true));
                }
            }

            return toReturn.ToArray();
        }

        public T[] LocateObjectsWithType<T>(bool firstcall = true) where T : class
        {
            List<T> toReturn = new List<T>();

            bool search = true;
            if (owner is GameObject)
            {
                GameObject casted = owner as GameObject;
                if (!casted.searchable) search = false;
            }

            if (owner is T && search) toReturn.Add(owner as T);

            if (freezeSearchesBelow && firstcall) return toReturn.ToArray();
            foreach (var collection in owner.GetAllCollections())
            {
                dynamic typed = Convert.ChangeType(collection, collection.CollectionType);
                foreach (var obj in typed.GetAllObjects())
                {
                    toReturn.AddRange(obj.LocateObjectsWithType<T>(false));
                }
            }
            if (owner is IInventory)
            {
                var casted = owner as IInventory;
                foreach (var obj in casted.GetInventory.GetItems)
                {
                    toReturn.AddRange(obj.LocateObjectsWithType<T>(false));
                }
            }
            if (owner is IHoldable)
            {
                var casted = owner as IHoldable;
                if (casted.GetHoldingSafe(out Item temp))
                    toReturn.AddRange(temp.LocateObjectsWithType<T>(true));
            }
            if (owner is IEquipable)
            {
                var casted = owner as IEquipable;
                if (casted.HasItemEquiped)
                {
                    toReturn.AddRange(casted.EquipedItem.LocateObjectsWithType<T>(true));
                }
            }

            return toReturn.ToArray();
        }

        public T[] LocateObjectsWithType<T>(string id, bool firstcall = true) where T : class
        {
            List<T> toReturn = new List<T>();

            var search = true;
            if (owner is GameObject)
            {
                GameObject casted = owner as GameObject;
                if (!casted.searchable) search = false;
            }

            if (owner is Identifiable && search)
            {
                var idOwner = owner as Identifiable;
                if (idOwner.HasID(id) && owner is T) toReturn.Add(owner as T);
            }

            if (freezeSearchesBelow && firstcall) return toReturn.ToArray();
            foreach (var collection in owner.GetAllCollections())
            {
                dynamic typed = Convert.ChangeType(collection, collection.CollectionType);
                foreach (var obj in typed.GetAllObjects())
                {
                    toReturn.AddRange(obj.LocateObjectsWithType<T>(id, false));
                }
            }
            if (owner is IInventory)
            {
                var casted = owner as IInventory;
                foreach (var obj in casted.GetInventory.GetItems)
                {
                    toReturn.AddRange(obj.LocateObjectsWithType<T>(id, false));
                }
            }
            if (owner is IHoldable)
            {
                var casted = owner as IHoldable;
                if (casted.GetHoldingSafe(out Item temp))
                    toReturn.AddRange(temp.LocateObjectsWithType<T>(id, true));
            }
            if (owner is IEquipable)
            {
                var casted = owner as IEquipable;
                if (casted.HasItemEquiped)
                {
                    toReturn.AddRange(casted.EquipedItem.LocateObjectsWithType<T>(id, true));
                }
            }

            return toReturn.ToArray();
        }

        public void LocateSingleObjectOfType<T>(PromptObjectResponse<T> response, bool firstcall = true) where T : class
        {
            var results = Utilities.CastArray<GameObject, T>(LocateObjectsWithType<T>(false));

            if (results.Length == 0) response.Invoke(default, false);
            else if (results.Length == 1) response.Invoke(results[0] as T, false);
            else Utilities.PromptSelection<T>("Select an object from these options", results, response);
        }

        public void LocateSingleObject(string id, PromptObjectResponse<GameObject> response, bool firstcall = true)
        {
            var results = Locate(id, firstcall);

            if (results.Length == 0) response.Invoke(null, false);
            else if (results.Length == 1) response.Invoke(results[0], false);
            else Utilities.PromptSelection<GameObject>("Select an object with the matching tag \"" + id + "\"", results, response);
        }

        public void LocateSingleObjectOfType<T>(string id, PromptObjectResponse<T> response, bool firstcall = true) where T : class
        {
            var results = Utilities.CastArray<GameObject, T>(LocateObjectsWithType<T>(id, false));

            if (results.Length == 0) response.Invoke(default, false);
            else if (results.Length == 1) response.Invoke(results[0] as T, false);
            else Utilities.PromptSelection<T>("Select an object from these options", results, response);
        }

        public IHoldable[] LocateHoldables(string id, bool firstcall = true)
        {
            List<IHoldable> holdables = new List<IHoldable>();

            if (owner is GameObject && owner is IHoldable)
            {
                var casted = owner as GameObject;
                if (casted.HasID(id) && casted.searchable) holdables.Add(owner as IHoldable);
            }
            if (freezeSearchesBelow && firstcall) return holdables.ToArray();
            var found = Locate(id, false);
            foreach (var item in found)
            {
                if (item is IHoldable) holdables.Add(item as IHoldable);
            }
            return holdables.ToArray();
        }

        public void LocateSingleHoldableObject(string id, PromptObjectResponse<IHoldable> response, bool firstcall = true)
        {
            var holdables = LocateHoldables(id, false);

            if (holdables.Length == 0) response.Invoke(null, false);
            else if (holdables.Length == 1) response.Invoke(holdables[0], false);
            else Utilities.PromptSelection<IHoldable>("Select one of the following with the matching tag \"" + id + "\"", Utilities.CastArray<GameObject, IHoldable>(holdables), response);
        }

        public IInventory[] LocateInventories(string id, bool firstcall = true)
        {
            List<IInventory> inventories = new List<IInventory>();

            if (owner is GameObject && owner is IInventory)
            {
                var casted = owner as GameObject;
                if (casted.HasID(id) && casted.searchable) inventories.Add(owner as IInventory);
            }
            if (freezeSearchesBelow && firstcall) return inventories.ToArray();
            var found = Locate(id, false);
            foreach (var item in found)
            {
                if (item is IInventory) inventories.Add(item as IInventory);
            }
            return inventories.ToArray();
        }

        public void LocateSingleInventory(string id, PromptObjectResponse<IInventory> response, bool firstcall = true)
        {
            var holdables = LocateInventories(id, false);

            if (holdables.Length == 0) response.Invoke(null, false);
            else if (holdables.Length == 1) response.Invoke(holdables[0], false);
            else Utilities.PromptSelection("Select one of the following with the matching tag \"" + id + "\"", Utilities.CastArray<GameObject, IInventory>(holdables), response);
        }

        public GameObject LocateWithGameID(int gameID)
        {
            if (owner is GameObject)
            {
                var casted = owner as GameObject;
                if (casted.ID == gameID) return casted;
            }

            foreach (var collection in owner.GetAllCollections())
            {
                dynamic typed = Convert.ChangeType(collection, collection.CollectionType);
                foreach (var obj in typed.GetAllObjects())
                {
                    var found = obj.LocateWithGameID(gameID);
                    if (found != null) return found;
                }
            }
            if (owner is IInventory)
            {
                var casted = owner as IInventory;
                foreach (var obj in casted.GetInventory.GetItems)
                {
                    var found = obj.LocateWithGameID(gameID);
                    if (found != null) return found;
                }
            }
            if (owner is IHoldable)
            {
                var casted = owner as IHoldable;
                if (casted.GetHoldingSafe(out Item temp))
                {
                    var found = temp.LocateWithGameID(gameID);
                    if (found != null) return found;
                }
            }
            if (owner is IEquipable)
            {
                var casted = owner as IEquipable;
                if (casted.HasItemEquiped)
                {
                    var found = casted.EquipedItem.LocateWithGameID(gameID);
                    if (found != null) return found;
                }
            }

            return null;
        }
    }
}
