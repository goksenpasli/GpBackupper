using System.Windows.Controls;
using System.Windows.Data;

namespace GpBackupper
{
    /// <summary>
    /// Interaction logic for SearchWindowControl.xaml
    /// </summary>
    public partial class SearchWindowControl : UserControl
    {
        public SearchWindowControl()
        {
            InitializeComponent();
            Cvs = TryFindResource("Cvs") as CollectionViewSource;
        }

        public static CollectionViewSource Cvs { get; set; }
    }
}