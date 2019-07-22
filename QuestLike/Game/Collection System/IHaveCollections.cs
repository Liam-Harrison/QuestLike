using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZorkLike
{
    public interface IHaveCollections
    {
        Collection<T> GetCollection<T>() where T : Collectable;
        void AddCollection<T>() where T : Collectable;
        Collection[] GetAllCollections();
        bool HasCollection<T>() where T : Collectable;
    }
}
