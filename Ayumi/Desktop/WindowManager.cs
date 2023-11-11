using System;
using System.Windows.Forms;

namespace Ayumi.Desktop
{
    public static class WindowManager
    {
        public static void OpenChildWindow<T>(IWin32Window owner, params Object[] args) where T : Form
        {
            using (T childWindow = (T)Activator.CreateInstance(typeof(T), args))
                childWindow.ShowDialog(owner);
        }

        public static Object OpenValueReturnableChildWindow<T>(IWin32Window owner, params Object[] args) where T : Form, IValueReturnable
        {
            using (T childWindow = (T)Activator.CreateInstance(typeof(T), args))
            {
                childWindow.ShowDialog(owner);
                return childWindow.Result;
            }
        }
    }
}