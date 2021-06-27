using Extensions;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;

namespace GpBackupper
{
    public class Files : InpcBase
    {
        private string fileName;

        private bool ısChecked;

        public string FileName
        {
            get => fileName;

            set
            {
                if (fileName != value)
                {
                    fileName = value;
                    OnPropertyChanged(nameof(FileName));
                }
            }
        }

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

    public class SearchControlViewModel : MainViewModel
    {
        private string searchFileName;

        public SearchControlViewModel()
        {
            CompressSelectedFiles = new RelayCommand<object>(parameter =>
            {
                CompressorViewModel compressorViewModel = new();
                compressorViewModel.CompressorView.Dosyalar = new();
                compressorViewModel.CompressorView.KayıtYolu = $@"{Data.DataSavePath}\{Guid.NewGuid()}.zip";
                foreach (Files item in Data.SelectedFiles.Where(z => z.IsChecked))
                {
                    compressorViewModel.CompressorView.Dosyalar.Add(item.FileName);
                }
                Compress(compressorViewModel);
            }, parameter => !string.IsNullOrWhiteSpace(Data.DataSavePath) && Data.SelectedFiles.Any(z => z.IsChecked));

            SearchComputerFiles = new RelayCommand<object>(parameter =>
            {
                Task.Factory.StartNew(() =>
                {
                    Data.Active = false;
                    Data.FoundFiles = new ObservableCollection<Files>();
                    foreach (string item in Data.SelectedDrive.DirSearch(parameter as string))
                    {
                        Application.Current.Dispatcher.BeginInvoke(new Action(() => Data.FoundFiles.Add(new Files() { FileName = item })));
                    }
                    Data.Active = true;
                }, CancellationToken.None, TaskCreationOptions.None, TaskScheduler.Default);
            }, parameter => !string.IsNullOrEmpty(Data.SelectedDrive));

            SelectAllFiles = new RelayCommand<object>(parameter =>
            {
                foreach (Files item in Data.FoundFiles)
                {
                    if (parameter is bool menuchecked)
                    {
                        item.IsChecked = menuchecked;
                    }
                }
            }, parameter => Data.FoundFiles.Any());

            PropertyChanged += SearchControlViewModel_PropertyChanged;
        }

        public ICommand CompressSelectedFiles { get; }

        public ICommand SearchComputerFiles { get; }
        public ICommand SelectAllFiles { get; }

        public string SearchFileName
        {
            get => searchFileName;

            set
            {
                if (searchFileName != value)
                {
                    searchFileName = value;
                    OnPropertyChanged(nameof(SearchFileName));
                }
            }
        }

        private void SearchControlViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "SearchFileName" && Data.FoundFiles?.Any() == true)
            {
                CollectionViewSource.GetDefaultView(Data.FoundFiles).Filter = string.IsNullOrWhiteSpace(SearchFileName) ? null : item => (item as Files)?.FileName.ToLower().Contains(SearchFileName) ?? false;
            }
        }
    }
}