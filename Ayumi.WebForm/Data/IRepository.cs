using System;
using System.Collections.Generic;

namespace WebLib.Data
{
    public interface IRepository<T> where T : class
    {
        T GetById(String id);
        IEnumerable<String> Insert(T data);
        IEnumerable<String> Update(T data);
        IEnumerable<String> Delete(T data);
    }
}