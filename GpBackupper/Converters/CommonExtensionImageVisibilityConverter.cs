using System;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Data;

namespace GpBackupper
{
    public class CommonExtensionImageVisibilityConverter : IValueConverter
    {
        private readonly string CommonExtensions = "*.aac;*.ac3;*.m3u;*.m4a;*.mid;*.midi;*.mka;*.mp3;*.ogg;*.wma;*.csv;*.doc;*.docx;*.odt;*.pdf;*.ppsx;*.pps;*.ppt;*.pptx;*.txt;*.xls;*.xlsx;*.jpeg;*.jpg;*.png;*.tif;*.tiff;*.3gp;*.3gpp;*.amr;*.avi;*.m2ts;*.mkv;*.mp4;*.mpeg;*.mpg;*.mov;*.mts;*.ogm;*.ogv;*.rmvb;*.ts;*.vob;*.webm;*.wmv";

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value is string extension && CommonExtensions.Split(';').Contains(extension.ToLower())
                ? Visibility.Visible
                : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}