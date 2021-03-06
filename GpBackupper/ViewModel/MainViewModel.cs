using Extensions;
using GpBackupper.View;
using SharpCompress.Common;
using SharpCompress.Writers;
using SharpCompress.Writers.Zip;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Shell;

namespace GpBackupper
{
    public class MainViewModel : InpcBase
    {
        private static Task task;

        private ObservableCollection<string> files;

        public MainViewModel()
        {
            Data = new Data();
            AddBackupFileExtensions = new RelayCommand<object>(parameter =>
            {
                if (parameter is string fileextensions)
                {
                    foreach (string extension in fileextensions.Split(';'))
                    {
                        if (!Data.BackupExtensions.Any(z => z.Extension == extension))
                        {
                            Data.BackupExtensions.Add(new Data() { Extension = extension });
                        }
                    }
                }
            }, parameter => parameter is string fileextensions && ValidateExtensions(fileextensions) || ValidateExtensions(Properties.Settings.Default.CustomExtensions));

            Data data = null;
            AddBackupFolder = new RelayCommand<object>(parameter =>
            {
                if (parameter is TreeViewModel treeViewModel)
                {
                    if (treeViewModel.IsChecked)
                    {
                        data = new() { FolderName = treeViewModel.Folder.FullName };
                        if (!Data.BackupFolders.Any(z => z.FolderName == data.FolderName))
                        {
                            Data.BackupFolders.Add(data);
                        }
                    }
                    else
                    {
                        _ = Data.BackupFolders.Remove(data);
                    }
                }
            }, parameter => true);

            RemoveBackupFolder = new RelayCommand<object>(parameter =>
             {
                 if (parameter is Data selecteditem)
                 {
                     _ = Data.BackupFolders.Remove(selecteditem);
                 }
             }, parameter => true);

            AddCommonFoldersBackupFolder = new RelayCommand<object>(parameter =>
            {
                if (parameter is CommonFolders folders)
                {
                    data = new() { FolderName = Environment.GetFolderPath((Environment.SpecialFolder)(int)folders) };
                    if (!Data.BackupFolders.Any(z => z.FolderName == data.FolderName))
                    {
                        Data.BackupFolders.Add(data);
                    }
                }
            }, parameter => true);

            SetSaveLocation = new RelayCommand<object>(parameter =>
            {
                FolderBrowserDialog dialog = new();
                dialog.Description = "Kayıt Klasörünü Seçin.";
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    Data.DataSavePath = dialog.SelectedPath;
                }
            }, parameter => true);

            OpenFile = new RelayCommand<object>(parameter => Process.Start(parameter as string), parameter => true);

            ExploreFile = new RelayCommand<object>(parameter => ExtensionMethods.OpenFolderAndSelectItem(Path.GetDirectoryName(parameter as string), Path.GetFileName(parameter as string)), parameter => true);

            StartCompress = new RelayCommand<object>(parameter =>
            {
                IEnumerable<string> backupfolders = Data.BackupFolders.Select(z => z.FolderName);
                IEnumerable<string> backupextensions = Data.BackupExtensions.Select(z => z.Extension);
                string savefolderpath = Data.DataSavePath;
                Files = new();
                foreach (string folder in backupfolders)
                {
                    foreach (string item in folder.FilterFiles(backupextensions.ToArray()))
                    {
                        Files.Add(item);
                    }
                }

                CompressorViewModel compressorViewModel = new();
                compressorViewModel.CompressorView.Dosyalar = new();
                compressorViewModel.CompressorView.KayıtYolu = $@"{savefolderpath}\{Guid.NewGuid()}.zip";
                foreach (string item in Files)
                {
                    compressorViewModel.CompressorView.Dosyalar.Add(item);
                }
                Compress(compressorViewModel);
            }, parameter => Data.BackupExtensions.Any() && Data.BackupFolders.Any() && !string.IsNullOrWhiteSpace(Data.DataSavePath));

            RemoveFileExtension = new RelayCommand<object>(parameter =>
            {
                if (parameter is Data data)
                {
                    _ = Data.BackupExtensions.Remove(data);
                }
            }, parameter => true);

            UpdateBuildInFileExtension = new RelayCommand<object>(parameter => Properties.Settings.Default.Save(), parameter =>
            {
                return ValidateExtensions(Properties.Settings.Default.DocumentExtensions) &&
                ValidateExtensions(Properties.Settings.Default.AudioExtensions) &&
                ValidateExtensions(Properties.Settings.Default.VideoExtensions) &&
                ValidateExtensions(Properties.Settings.Default.PictureExtensions);
            });
            ResetBuildInFileExtension = new RelayCommand<object>(parameter => Properties.Settings.Default.Reset(), parameter => true);
        }

        public ICommand AddBackupFileExtensions { get; }

        public ICommand AddBackupFolder { get; }

        public ICommand AddCommonFoldersBackupFolder { get; }

        public Data Data { get; set; }

        public ICommand ExploreFile { get; }

        public ObservableCollection<string> Files
        {
            get => files;

            set
            {
                if (files != value)
                {
                    files = value;
                    OnPropertyChanged(nameof(Files));
                }
            }
        }

        public ICommand OpenFile { get; }

        public ICommand RemoveBackupFolder { get; }

        public ICommand RemoveFileExtension { get; }

        public ICommand ResetBuildInFileExtension { get; }

        public ICommand SetSaveLocation { get; }

        public ICommand StartCompress { get; }

        public ICommand UpdateBuildInFileExtension { get; }

        public void Compress(CompressorViewModel compressorViewModel)
        {
            if (task?.IsCompleted == false)
            {
                System.Windows.MessageBox.Show("Başka Bir Görev Çalışıyor.", "YEDEKLEYİCİ", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return;
            }
            task = Task.Factory.StartNew(() =>
              {
                  try
                  {
                      Data.Active = false;
                      Data.ProgressState = TaskbarItemProgressState.Normal;
                      using FileStream stream = File.OpenWrite(compressorViewModel.CompressorView.KayıtYolu);
                      switch (Data.Biçim)
                      {
                          case 0:
                              {
                                  using ZipWriter writer = new(stream, new ZipWriterOptions(CompressionType.Deflate) { UseZip64 = true, DeflateCompressionLevel = (SharpCompress.Compressors.Deflate.CompressionLevel)compressorViewModel.CompressorView.SıkıştırmaDerecesi });
                                  WriteData(writer, compressorViewModel);
                                  break;
                              }

                          case 1:
                              {
                                  using IWriter writer = WriterFactory.Open(stream, ArchiveType.Zip, CompressionType.LZMA);
                                  WriteData(writer, compressorViewModel);
                                  break;
                              }
                      }
                      Data.Active = true;
                  }
                  catch (Exception Ex)
                  {
                      Data.ProgressState = TaskbarItemProgressState.Error;
                      Data.Active = true;
                      _ = System.Windows.MessageBox.Show(Ex.Message, "YEDEKLEYİCİ", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                  }
              }, CancellationToken.None, TaskCreationOptions.None, TaskScheduler.Default).ContinueWith(task =>
              {
                  compressorViewModel.CompressorView.Dosyalar.Clear();
                  if (task.IsCompleted)
                  {
                      if (Properties.Settings.Default.ShutDownMode == 1)
                      {
                          Shutdown.DoExitWin(Shutdown.EWX_SHUTDOWN);
                      }
                      if (Properties.Settings.Default.ShutDownMode == 2)
                      {
                          Shutdown.DoExitWin(Shutdown.EWX_REBOOT);
                      }
                  }
              });
        }

        private bool ValidateExtensions(string documentExtensions)
        {
            return new Regex(@"\*\.(?=.)").Match(documentExtensions).Success;
        }

        private void WriteData(IWriter writer, CompressorViewModel compressorViewModel)
        {
            double currentfile = 1;
            Data.FileCount = compressorViewModel.CompressorView.Dosyalar.Count;
            foreach (string dosya in compressorViewModel.CompressorView.Dosyalar)
            {
                Data.FileName = Path.GetFileName(dosya);
                Data.FileSize = (int)new FileInfo(dosya).Length;
                writer.Write(Path.GetFileName(dosya), dosya);
                Data.Oran = currentfile++ / Data.FileCount;
            }
        }
    }
}