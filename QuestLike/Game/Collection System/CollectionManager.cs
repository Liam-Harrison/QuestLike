﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace QuestLike
{
    
    public class CollectionManager: IHaveCollections
    {
        
        private List<ICollection> collections = new List<ICollection>();
        
        private GameObject owner;

        public CollectionManager(GameObject owner)
        {
            this.owner = owner;
        }

        public CollectionManager()
        {

        }

        
        public GameObject Owner
        {
            get
            {
                return owner;
            }
        }

        public void AddCollection<T>() where T : Collectable
        {
            if (HasCollection<T>()) return;
            var collection = new Collection<T>
            {
                owner = owner
            };
            collections.Add(collection);
        }

        public Collection<T> GetCollection<T>() where T : Collectable
        {
            foreach (var i in collections)
            {
                if (i is Collection<T>) return i as Collection<T>;
            }
            return null;
        }

        public bool HasCollection<T>() where T : Collectable
        {
            foreach (var i in collections)
            {
                if (i is Collection<T>) return true;
            }
            return false;
        }

        public ICollection[] GetAllCollections()
        {
            return collections.ToArray();
        }
    }
}
