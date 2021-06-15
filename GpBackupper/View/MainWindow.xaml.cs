using System.Windows;

namespace GpBackupper
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new MainViewModel();
        }

        private void Compressor_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
        }
    }
}