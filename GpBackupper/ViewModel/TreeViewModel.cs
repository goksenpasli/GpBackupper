﻿using Extensions;
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

        private DirectoryInfo folder = new(@"C:\");

        private string fullPath;

        private bool ısChecked;

        private bool isaccessible = true;

        private string name;

        public TreeViewModel()
        {
            CompressAllSubFiles = new RelayCommand<object>(parameter =>
            {
                if (parameter is string fullpath)
                {
                    ConcurrentBag<string> files = fullpath.DirSearch();
                    CompressorViewModel compressorViewModel = new();
                    compressorViewModel.CompressorView.Dosyalar = new();
                    compressorViewModel.CompressorView.KayıtYolu = $@"{fullpath}\{Guid.NewGuid()}.zip";
                    foreach (string item in files)
                    {
                        compressorViewModel.CompressorView.Dosyalar.Add(item);
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