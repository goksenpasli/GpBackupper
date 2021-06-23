using Extensions;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Input;

namespace GpBackupper
{
    public class TreeViewModel : MainViewModel
    {
        private List<TreeViewModel> _subFolders;

        private IEnumerable<string> drives = DriveInfo.GetDrives().Where(z => z.DriveType == DriveType.Fixed).Select(z => z.Name);

        private bool filteredExtensionCompress;

        private DirectoryInfo folder = new(@"C:\");

        private string fullPath;

        private bool ısChecked;

        private bool isaccessible = true;

        private string name;

        public TreeViewModel()
        {
            CompressAllSubFiles = new RelayCommand<object>(parameter =>
            {
                if (parameter is object[] datacontext)
                {
                    ConcurrentBag<string> files = (datacontext[0] as TreeViewModel).FullPath.DirSearch();
                    CompressorViewModel compressorViewModel = new();
                    compressorViewModel.CompressorView.Dosyalar = new();
                    compressorViewModel.CompressorView.KayıtYolu = $@"{(datacontext[0] as TreeViewModel)?.FullPath}\{Guid.NewGuid()}.zip";
                    if (FilteredExtensionCompress)
                    {
                        IEnumerable<string> backupextensions = (datacontext[1] as MainViewModel).Data.BackupExtensions.Select(z => z.Extension.ToLower());
                        foreach (string item in files.Where(z => backupextensions.Contains($"*{Path.GetExtension(z)}")))
                        {
                            compressorViewModel.CompressorView.Dosyalar.Add(item);
                        }
                    }
                    else
                    {
                        foreach (string item in files)
                        {
                            compressorViewModel.CompressorView.Dosyalar.Add(item);
                        }
                    }
                    Compress(compressorViewModel);
                }
            }, parameter => true);
        }

        public ICommand CompressAllSubFiles { get; }

        public IEnumerable<string> Drives
        {
            get => drives;

            set
            {
                if (drives != value)
                {
                    drives = value;
                    OnPropertyChanged(nameof(Drives));
                }
            }
        }

        public bool FilteredExtensionCompress
        {
            get => filteredExtensionCompress;

            set
            {
                if (filteredExtensionCompress != value)
                {
                    filteredExtensionCompress = value;
                    OnPropertyChanged(nameof(FilteredExtensionCompress));
                }
            }
        }

        public DirectoryInfo Folder
        {
            get => folder;

            set
            {
                if (folder != value)
                {
                    folder = value;
                    OnPropertyChanged(nameof(Folder));
                }
            }
        }

        public string FullPath
        {
            get => fullPath;

            set
            {
                if (fullPath != value)
                {
                    fullPath = value;
                    Folder = new DirectoryInfo(value);
                    OnPropertyChanged(nameof(FullPath));
                }
            }
        }

        public bool IsAccessible
        {
            get => isaccessible;

            set
            {
                if (isaccessible != value)
                {
                    isaccessible = value;
                    OnPropertyChanged(nameof(IsAccessible));
                }
            }
        }

        public bool IsChecked
        {
            get => ısChecked;

            set
            {
                if (ısChecked != value)
                {
                    ısChecked = value;
                    OnPropertyChanged(nameof(IsChecked));
                }
            }
        }

        public string Name
        {
            get => Folder.Name;

            set
            {
                if (name != value)
                {
                    name = value;
                    OnPropertyChanged(nameof(Name));
                }
            }
        }

        public List<TreeViewModel> SubFolders
        {
            get
            {
                if (_subFolders == null)
                {
                    _subFolders = new List<TreeViewModel>();
                    try
                    {
                        DirectoryInfo[] di = Folder?.GetDirectories();
                        for (int i = 0; i < di?.Length; i++)
                        {
                            TreeViewModel newFolder = new();
                            newFolder.FullPath = di[i].FullName;
                            _subFolders.Add(newFolder);
                        }
                    }
                    catch (UnauthorizedAccessException)
                    {
                        IsAccessible = false;
                        IsChecked = false;
                    }
                }
                return _subFolders;
            }
        }
    }
}