using Extensions;
using System;
using System.Collections.Generic;
using System.IO;

namespace GpBackupper
{
    public class TreeViewModel : InpcBase
    {
        private List<TreeViewModel> _subFolders;

        private DirectoryInfo folder = new(@"C:\");

        private string fullPath;

        private string name;

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
                    }
                }
                return _subFolders;
            }
        }
    }
}