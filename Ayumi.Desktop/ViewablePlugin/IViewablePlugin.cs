using System.Windows.Controls;
using Ayumi.Plugin;

namespace Ayumi.Desktop.ViewablePlugin {
    public interface IViewablePlugin : IPlugin {
        UserControl View { get; }
    }
}
