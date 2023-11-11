using System;

namespace WebLib.Web
{
    public interface IControlModel<T>
    {
        T GetData(String id);
        void SetData(T data);
    }
}