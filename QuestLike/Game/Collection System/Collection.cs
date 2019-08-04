using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace QuestLike
{
    [JsonObject(IsReference = true, ItemTypeNameHandling = TypeNameHandling.All)]
    public class ICollection
    {
        public virtual dynamic GetTypedCollection()
        {
            throw new Exception("Should not reach this code.");
        }
        [JsonIgnore]
        public virtual bool ShowInLocate { get; }
        [JsonIgnore]
        public virtual GameObject Owner { get; }
    }

    class CollectionConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return (objectType == typeof(ICollection));
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            return serializer.Deserialize(reader, (existingValue as ICollection).GetType());
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            serializer.Serialize(writer, value, (value as ICollection).GetType());
        }
    }

    [JsonObject(MemberSerialization = MemberSerialization.OptIn, ItemTypeNameHandling = TypeNameHandling.All)]
    public class Collection<T> : ICollection where T : Collectable
    {
        [JsonProperty(IsReference = true)]
        public GameObject owner;
        [JsonProperty]
        public bool showInLocate = true;
        [JsonProperty]
        List<T> contents = new List<T>();

        public override dynamic GetTypedCollection()
        {
            return this;
        }

        [JsonIgnore]
        public override GameObject Owner
        {
            get
            {
                return owner;
            }
        }

        public Collection()
        {
        }

        [JsonIgnore]
        public T[] Objects
        {
            get
            {
                return contents.ToArray();
            }
        }

        [JsonIgnore]
        public List<T> ObjectList
        {
            get
            {
                return contents;
            }
        }

        [JsonIgnore]
        public override bool ShowInLocate => showInLocate;

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
