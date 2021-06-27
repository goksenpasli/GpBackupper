using Extensions;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;

namespace GpBackupper
{
    public class Files : Data
    {
        private bool ısChecked;

        public bool IsChecked
        {
            get => ısChecked; set

            {
                if (ısChecked != value)
                {
                    ısChecked = value;
                    OnPropertyChanged(nameof(IsChecked));
                }
            }
        }
    }
}