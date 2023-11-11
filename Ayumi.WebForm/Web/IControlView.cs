using System;

namespace WebLib.Web
{
    public interface IControlView<T>
    {
        T GetData();
        void SetData(T data);
        void ResetView(Boolean isEdit);
    }
}