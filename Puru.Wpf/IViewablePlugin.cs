using System.Windows.Controls;

namespace Puru.Wpf {
    public interface IViewablePlugin : IPlugin {
        UserControl View { get; }
    }
}
