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
    public class SearchControlViewModel : MainViewModel
    {
        private string searchFileName;

        public SearchControlViewModel()
        {
            CompressSelectedFiles = new RelayCommand<object>(parameter =>
            {
                CompressorViewModel compressorViewModel = new();
                compressorViewModel.CompressorView.KayıtYolu = $@"{Data.DataSavePath}\{Guid.NewGuid()}.zip";
                compressorViewModel.CompressorView.Dosyalar = Data.SelectedFiles;
                Compress(compressorViewModel);
            }, parameter => !string.IsNullOrWhiteSpace(Data.DataSavePath) && Data.SelectedFiles.Any());

            SearchComputerFiles = new RelayCommand<object>(parameter =>
            {
                Task.Factory.StartNew(() =>
                {
                    Data.Active = false;
                    Data.FoundFiles = new ObservableCollection<string>();
                    foreach (string item in Data.SelectedDrive.DirSearch(parameter as string))
                    {
                        Application.Current.Dispatcher.BeginInvoke(new Action(() => Data.FoundFiles.Add(item)));
                    }
                    Data.Active = true;
                }, CancellationToken.None, TaskCreationOptions.None, TaskScheduler.Default);
            }, parameter => !string.IsNullOrEmpty(Data.SelectedDrive));

            PropertyChanged += SearchControlViewModel_PropertyChanged;
        }

        public ICommand CompressSelectedFiles { get; }

        public ICommand SearchComputerFiles { get; }

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
                CollectionViewSource.GetDefaultView(Data.FoundFiles).Filter = string.IsNullOrWhiteSpace(SearchFileName) ? null : item => (item as string)?.ToLower().Contains(SearchFileName) ?? false;
            }
        }
    }
}