using System;
using System.Collections.Generic;

namespace WebApp {
    public interface IRepository<T> where T : class {
        IList<T> GetAll(String tenant);
        T Get(String tenant, String id);
        Boolean Find(String tenant, String id);
        void Add(String tenant, T data);
        void Update(String tenant, String id, T data);
        void Remove(String tenant, String id);
    }
}