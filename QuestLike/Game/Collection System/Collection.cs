using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace QuestLike
{
    [JsonObject(ItemTypeNameHandling = TypeNameHandling.All, ItemConverterType = typeof(CollectionConverter))]
    public interface ICollection
    {
        dynamic GetTypedCollection();
        [JsonIgnore]
        bool ShowInLocate { get; }
        [JsonIgnore]
        GameObject Owner { get; }
    }

    public class CollectionConverter : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {

        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (!CanConvert(objectType))
                throw new Exception(string.Format("This converter is not for {0}.", objectType));

            var keyType = objectType.GetGenericArguments()[0];
            var valueType = objectType.GetGenericArguments()[1];
            var dictionaryType = typeof(Dictionary<,>).MakeGenericType(keyType, valueType);
            var result = (ICollection)Activator.CreateInstance(dictionaryType);

            if (reader.TokenType == JsonToken.Null)
                return null;

            while (reader.Read())
            {
                if (reader.TokenType == JsonToken.EndObject)
                {
                    return result;
                }
            }

            return result;
        }

        public override bool CanConvert(Type objectType)
        {
            return objectType.IsGenericType && (objectType.GetGenericTypeDefinition() == typeof(Collection<>) || objectType.GetGenericTypeDefinition() == typeof(Collection<>));
        }
    }



    [JsonObject(MemberSerialization = MemberSerialization.OptIn, ItemTypeNameHandling = TypeNameHandling.All)]
    public class Collection<T> : ICollection where T : Collectable
    {
        [JsonProperty(IsReference = true)]
        public GameObject owner;
        [JsonProperty]
        public bool showInLocate = true;
        [JsonProperty(ItemTypeNameHandling = TypeNameHandling.All)]
        List<T> contents = new List<T>();

        public dynamic GetTypedCollection()
        {
            return this;
        }

        [JsonIgnore]
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
