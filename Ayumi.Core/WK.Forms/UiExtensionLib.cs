using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using System.Linq;

namespace WK.Forms
{
    public static class UiExtensionLib
    {
        public static BindingList<T> ToBindingList<T>(this IEnumerable<T> enumerableItems)
        {
            if (enumerableItems == null) throw new ArgumentNullException("enumerableItems");

            var bindingList = new BindingList<T>();
            foreach (var item in enumerableItems)
                bindingList.Add(item);

            return bindingList;
        }

        public static void SetBindingDataSource<T>(this DataGridView dataGrid, IEnumerable<T> dataSource)
        {
            var listToBind = dataSource.ToBindingList();
            dataGrid.DataSource = listToBind;
        }

        public static IEnumerable<T> GetBoundDataSource<T>(this DataGridView dataGrid)
        {
            var boundDataSource = (BindingList<T>)dataGrid.DataSource;
            return boundDataSource.AsEnumerable();
        }

    }
}
