using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace XemPhim.Interfaces.CRUD
{
    public class ListOf<T>
    {
        public T[] Data { get; set; }

        public int Count
        {
            get
            {
                return Data.Length;
            }
        }

        public Paging Paging { get; set; }

        public static ListOf<X> From<X>(IOrderedQueryable<X> dbSet, Paging paging) where X : class
        {
            List<X> data = dbSet.Skip((paging.Page - 1) * paging.Size).Take(paging.Size).ToList();
            return new ListOf<X>()
            {
                Data = data.ToArray(),
                Paging = paging,
            };
        }
    }
}