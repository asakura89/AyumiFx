using System;
using System.Collections.Generic;
using Ayumi.Data;

namespace Ayumi.Desktop
{
    public class DesktopMultiSelectList : IMultiSelectList
    {


        public IList<int> SelectedIndexes
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public IList<NameValueItem> SelectedItems
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public void Add(NameValueItem nvi)
        {
            throw new NotImplementedException();
        }

        public void AddRange(IList<NameValueItem> nviList)
        {
            throw new NotImplementedException();
        }

        public void Clear()
        {
            throw new NotImplementedException();
        }

        public void Remove(int idx)
        {
            throw new NotImplementedException();
        }

        public void RemoveRange(Func<NameValueItem, bool> predicate)
        {
            throw new NotImplementedException();
        }
    }
}