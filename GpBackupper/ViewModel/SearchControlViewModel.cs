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
    public class SearchControlViewModel : MainViewModel
    {
        private static long fileSizeSum;

        private ObservableCollection<Files> foundFiles;

        private string searchFileName;

        public SearchControlViewModel()
        {
            CompressSelectedFiles = new RelayCommand<object>(parameter =>
            {
                CompressorViewModel compressorViewModel = new();
                compressorViewModel.CompressorView.Dosyalar = new();
                compressorViewModel.CompressorView.KayıtYolu = $@"{Data.DataSavePath}\{Guid.NewGuid()}.zip";
                foreach (Files item in FoundFiles.Where(z => z.IsChecked))
                {
                    compressorViewModel.CompressorView.Dosyalar.Add(item.FileName);
                }
                Compress(compressorViewModel);
            }, parameter => !string.IsNullOrWhiteSpace(Data.DataSavePath) && FoundFiles.Any(z => z.IsChecked));

            SearchComputerFiles = new RelayCommand<object>(parameter =>
            {
                Task.Factory.StartNew(() =>
                {
                    Data.Active = false;
                    FoundFiles = new ObservableCollection<Files>();
                    foreach (string item in Data.SelectedDrive.DirSearch(parameter as string))
                    {
                        Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                        {
                            Files file = new Files() { FileName = item, FileSize = new FileInfo(item).Length / 1024 };
                            FoundFiles.Add(file);
                        }));
                    }
                    Data.Active = true;
                }, CancellationToken.None, TaskCreationOptions.None, TaskScheduler.Default);
            }, parameter => !string.IsNullOrEmpty(Data.SelectedDrive));

            SelectAllFiles = new RelayCommand<object>(parameter =>
            {
                foreach (Files item in FoundFiles)
                {
                    if (parameter is bool menuchecked)
                    {
                        item.IsChecked = menuchecked;
                    }
                }
            }, parameter => FoundFiles.Any());

            PropertyChanged += SearchControlViewModel_PropertyChanged;
        }

        public static new event EventHandler<PropertyChangedEventArgs> StaticPropertyChanged;

        public static long FileSizeSum
        {
            get => fileSizeSum;

            set
            {
                if (fileSizeSum != value)
                {
                    fileSizeSum = value;
                    StaticPropertyChanged?.Invoke(null, new(nameof(FileSizeSum)));
                }
            }
        }

        public ICommand CompressSelectedFiles { get; }

        public ObservableCollection<Files> FoundFiles
        {
            get => foundFiles;

            set
            {
                if (foundFiles != value)
                {
                    foundFiles = value;
                    OnPropertyChanged(nameof(FoundFiles));
                }
            }
        }

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

        public ICommand SelectAllFiles { get; }

        private void SearchControlViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "SearchFileName" && FoundFiles?.Any() == true)
            {
                CollectionViewSource.GetDefaultView(FoundFiles).Filter = string.IsNullOrWhiteSpace(SearchFileName) ? null : item => (item as Files)?.FileName.ToLower().Contains(SearchFileName) ?? false;
            }
            if (e.PropertyName == "IsChecked" && sender is Files file)
            {
                if (file.IsChecked)
                {
                    FileSizeSum += file.FileSize;
                }
                else
                {
                    FileSizeSum -= file.FileSize;
                }
            }
        }
    }
}