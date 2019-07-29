using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuestLike
{

    public abstract class Collection
    {
        protected Type type;
        public GameObject owner;
        public bool treeSearchable = true;

        public Type CollectionType
        {
            get
            {
                return type;
            }
        }

        public dynamic GetTypedCollection()
        {
            return Convert.ChangeType(this, type);
        }

        public GameObject Owner
        {
            get
            {
                return owner;
            }
        }
    }

    public class Collection<T> : Collection where T : Collectable
    {
        List<T> contents = new List<T>();

        public Collection()
        {
            type = GetType();
        }

        public T[] Objects
        {
            get
            {
                return contents.ToArray();
            }
        }

        public List<T> ObjectList
        {
            get
            {
                return contents;
            }
        }

        public T AddObject(T item)
        {
            contents.Add(item);
            item.container = this;
            return item;
        }

        public T GetObjectWithID(string id)
        {
            foreach (var i in contents)
            {
                if (i.HasID(id)) return i;
            }
            return null;
        }

        public T[] GetObjectsWithID(string id)
        {
            List<T> toReturn = new List<T>();
            foreach (var i in contents)
            {
                if (i.HasID(id)) toReturn.Add(i);
            }
            return toReturn.ToArray();
        }

        public T[] GetAllObjects()
        {
            return contents.ToArray();
        }

        public int ObjectsWithID(string id)
        {
            int j = 0;
            foreach (var i in contents)
            {
                if (i.HasID(id)) j++;
            }
            return j;
        }

        public void RemoveObject(T item)
        {
            contents.Remove(item);
        }

        public bool HasObjectWithID(string id)
        {
            return GetObjectWithID(id) != null;
        }

        public bool HasObject(T item)
        {
            return contents.Contains(item);
        }

    }
}
