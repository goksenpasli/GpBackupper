using Extensions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;

namespace GpBackupper.View
{
    public enum CommonFolders
    {
        Desktop = 0,

        MyDocuments = 5,

        MyMusic = 13,

        MyPictures = 39,

        MyVideos = 14,
    }

    public class Data : InpcBase, IDataErrorInfo
    {
        private bool active = true;

        private ObservableCollection<Data> backupExtensions = new();

        private ObservableCollection<Data> backupfolders = new();

        private int biçim;

        private string dataSavePath;

        private string extension;

        private double fileCount;

        private string fileName;

        private int? fileSize;

        private IEnumerable<object> folderExtensions;

        private int folderFileCount;

        private string folderName;

        private double oran;

        public bool Active
        {
            get => active;

            set
            {
                if (active != value)
                {
                    active = value;
                    OnPropertyChanged(nameof(Active));
                }
            }
        }

        public ObservableCollection<Data> BackupExtensions
        {
            get => backupExtensions;

            set
            {
                if (backupExtensions != value)
                {
                    backupExtensions = value;
                    OnPropertyChanged(nameof(BackupExtensions));
                }
            }
        }

        public ObservableCollection<Data> BackupFolders
        {
            get => backupfolders;

            set
            {
                if (backupfolders != value)
                {
                    backupfolders = value;
                    OnPropertyChanged(nameof(BackupFolders));
                }
            }
        }

        public int Biçim
        {
            get => biçim;

            set
            {
                if (biçim != value)
                {
                    biçim = value;
                    OnPropertyChanged(nameof(Biçim));
                }
            }
        }

        public IList<CommonFolders> CommonFolders => Enum.GetValues(typeof(CommonFolders)).Cast<CommonFolders>().ToList();

        public string DataSavePath
        {
            get => dataSavePath;

            set
            {
                if (dataSavePath != value)
                {
                    dataSavePath = value;
                    OnPropertyChanged(nameof(DataSavePath));
                }
            }
        }

        public string Error => string.Empty;

        public string Extension
        {
            get => extension;

            set
            {
                if (extension != value)
                {
                    extension = value;
                    OnPropertyChanged(nameof(Extension));
                }
            }
        }

        public double FileCount
        {
            get => fileCount;

            set
            {
                if (fileCount != value)
                {
                    fileCount = value;
                    OnPropertyChanged(nameof(FileCount));
                }
            }
        }

        public string FileName
        {
            get => fileName;

            set
            {
                if (fileName != value)
                {
                    fileName = value;
                    OnPropertyChanged(nameof(FileName));
                }
            }
        }

        public int? FileSize
        {
            get => fileSize;

            set
            {
                if (fileSize != value)
                {
                    fileSize = value;
                    OnPropertyChanged(nameof(FileSize));
                }
            }
        }

        public IEnumerable<object> FolderExtensions
        {
            get => folderExtensions;

            set
            {
                if (folderExtensions != value)
                {
                    folderExtensions = value;
                    OnPropertyChanged(nameof(FolderExtensions));
                }
            }
        }

        public int FolderFileCount
        {
            get { return folderFileCount; }

            set
            {
                if (folderFileCount != value)
                {
                    folderFileCount = value;
                    OnPropertyChanged(nameof(FolderFileCount));
                }
            }
        }

        public string FolderName
        {
            get => folderName;

            set
            {
                if (folderName != value)
                {
                    folderName = value;
                    try
                    {
                        string[] files = Directory.GetFiles(folderName);
                        FolderFileCount = files.Length;
                        FolderExtensions = files.Select(z => Path.GetExtension(z).TrimStart('.').ToLower()).GroupBy(x => x, (ext, extCnt) => new
                        {
                            Extension = ext,
                            Count = extCnt.Count()
                        });
                    }
                    catch (UnauthorizedAccessException)
                    {
                    }
                    OnPropertyChanged(nameof(FolderName));
                }
            }
        }

        public double Oran
        {
            get => oran;

            set
            {
                if (oran != value)
                {
                    oran = value;
                    OnPropertyChanged(nameof(Oran));
                }
            }
        }

        public string this[string columnName] => columnName switch
        {
            "Extension" when string.IsNullOrEmpty(Extension) || !Extension.StartsWith("*.") => "Uzantı Boş Olmamalı Ve *. İle Başlamalıdır. Silip Tekrar Ekleyin.",
            _ => null
        };
    }
}