using System.Collections.Generic;

namespace POData
{
    public interface IRepository<T>
    {
        bool Lisaa(T o);
        bool Poista(string id);
        bool Muuta(T o);
        T Hae(string o);
        List<T> HaeKaikki();
    }
}
