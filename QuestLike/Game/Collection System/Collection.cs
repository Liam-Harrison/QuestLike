using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuestLike
{
    public interface ICollection
    {
        dynamic GetTypedCollection();
        bool ShowInLocate { get; }
        GameObject Owner { get; }
    }

    public class Collection<T> : ICollection where T : Collectable
    {
        public GameObject owner;
        public bool showInLocate = true;
        List<T> contents = new List<T>();

        public dynamic GetTypedCollection()
        {
            return this;
        }

        public GameObject Owner
        {
            get
            {
                return owner;
            }
        }

        public Collection()
        {
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

        public bool ShowInLocate => showInLocate;

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
