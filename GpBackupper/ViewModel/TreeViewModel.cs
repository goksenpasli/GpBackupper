using Extensions;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Windows.Data;

namespace GpBackupper
{
    public class DriveConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string drive)
            {
                try
                {
                    TreeViewModel treeView = new();
                    DirectoryInfo[] di = new DirectoryInfo(drive)?.GetDirectories();
                    for (int i = 0; i < di?.Length; i++)
                    {
                        TreeViewModel newFolder = new();
                        newFolder.FullPath = di[i].FullName;
                    }
                    return treeView.SubFolders;
                }
                catch (UnauthorizedAccessException)
                {
                }
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class TreeViewModel : InpcBase
    {
        private List<TreeViewModel> _subFolders;

        private IEnumerable<string> drives = DriveInfo.GetDrives().Where(z => z.DriveType == DriveType.Fixed).Select(z => z.Name);

        private DirectoryInfo folder = new(@"C:\");

        private string fullPath;

        private string name;

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