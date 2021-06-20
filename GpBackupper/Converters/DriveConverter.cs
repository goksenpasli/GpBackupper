using System;
using System.Globalization;
using System.IO;
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
                    treeView.Folder = new DirectoryInfo(drive);
                    DirectoryInfo[] di = treeView.Folder?.GetDirectories();
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
}