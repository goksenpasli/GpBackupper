using Extensions;
using GpBackupper.View;
using SharpCompress.Common;
using SharpCompress.Writers;
using SharpCompress.Writers.Zip;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;

namespace GpBackupper
{
    public class MainViewModel : InpcBase
    {
        private Data data;

        public MainViewModel()
        {
            Data = new Data();

            AddBackupFileExtensions = new RelayCommand<object>(parameter =>
            {
                if (parameter is string fileextensions)
                {
                    foreach (string extension in fileextensions.Split(';'))
                    {
                        Data.BackupExtensions.Add(new Data() { Extension = extension });
                    }
                }
            }, parameter => parameter is string fileextensions && !string.IsNullOrWhiteSpace(fileextensions));

            AddBackupFolder = new RelayCommand<object>(parameter =>
            {
                if (parameter is TreeViewModel treeViewModel)
                {
                    if (treeViewModel.IsChecked)
                    {
                        Data data = new() { FolderName = treeViewModel.Folder.FullName };
                        Data.BackupFolders.Add(data);
                    }
                    else
                    {
                        Data.BackupFolders.RemoveAt(0);
                    }
                }
            }, parameter => true);

            SetSaveLocation = new RelayCommand<object>(parameter =>
            {
                FolderBrowserDialog dialog = new();
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    Data.DataSavePath = dialog.SelectedPath;
                }
            }, parameter => true);

            StartCompress = new RelayCommand<object>(parameter =>
            {
                IEnumerable<string> backupfolders = Data.BackupFolders.Select(z => z.FolderName);
                IEnumerable<string> backupextensions = Data.BackupExtensions.Select(z => z.Extension);
                string savefolderpath = Data.DataSavePath;
                List<string> files = new();
                foreach (string folder in backupfolders)
                {
                    foreach (string item in folder.FilterFiles(backupextensions.ToArray()))
                    {
                        files.Add(item);
                    }
                }

                CompressorViewModel compressorViewModel = new();
                compressorViewModel.CompressorView.Dosyalar = new();
                compressorViewModel.CompressorView.KayıtYolu = $@"{savefolderpath}\{Guid.NewGuid()}.zip";
                foreach (string item in files)
                {
                    compressorViewModel.CompressorView.Dosyalar.Add(item);
                }

                void WriteData(IWriter writer)
                {
                    foreach (string dosya in compressorViewModel.CompressorView.Dosyalar)
                    {
                        writer.Write(Path.GetFileName(dosya), dosya);

                        compressorViewModel.CompressorView.Oran++;
                        compressorViewModel.CompressorView.DosyaAdı = Path.GetFileName(dosya);
                    }
                }

                Task.Factory.StartNew(() =>
                {
                    try
                    {
                        Data.Active = false;
                        using FileStream stream = File.OpenWrite(compressorViewModel.CompressorView.KayıtYolu);
                        using ZipWriter writer = new(stream, new ZipWriterOptions(CompressionType.Deflate) { UseZip64 = true, DeflateCompressionLevel = (SharpCompress.Compressors.Deflate.CompressionLevel)compressorViewModel.CompressorView.SıkıştırmaDerecesi });
                        WriteData(writer);
                        Data.Active = true;
                    }
                    catch (Exception Ex)
                    {
                        System.Windows.MessageBox.Show(Ex.Message, "YEDEKLEYİCİ", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    }
                }, CancellationToken.None, TaskCreationOptions.None, TaskScheduler.Default).ContinueWith(_ => compressorViewModel.CompressorView.Dosyalar.Clear(), TaskScheduler.FromCurrentSynchronizationContext());
            }, parameter => Data.BackupExtensions.Any() && Data.BackupFolders.Any() && !string.IsNullOrWhiteSpace(Data.DataSavePath));

            RemoveFileExtension = new RelayCommand<object>(parameter =>
            {
                if (parameter is Data data)
                {
                    Data.BackupExtensions.Remove(data);
                }
            }, parameter => true);
        }

        public ICommand AddBackupFileExtensions { get; }

        public ICommand AddBackupFolder { get; }

        public Data Data
        {
            get => data;

            set
            {
                if (data != value)
                {
                    data = value;
                    OnPropertyChanged(nameof(Data));
                }
            }
        }

        public ICommand RemoveFileExtension { get; }

        public ICommand SetSaveLocation { get; }

        public ICommand StartCompress { get; }
    }
}