
using DynamicData.Samplz.Examples;
using DynamicData.Samplz.Infrastructure;
using MahApps.Metro.Controls;

namespace DynamicData.Samplz
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new SelectableItemCollection();
        }
    }
}
