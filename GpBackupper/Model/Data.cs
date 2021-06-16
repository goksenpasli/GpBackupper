using Extensions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;

namespace GpBackupper.View
{
    public class Data : InpcBase, IDataErrorInfo
    {
        private bool active = true;

        private string audioExtensions = "*.aac;*.ac3;*.aif;*.aifc;*.aiff;*.au;*.cda;*.dts;*.fla;*.flac;*.it;*.m1a;*.m2a;*.m3u;*.m4a;*.mid;*.midi;*.mka;*.mod;*.mp2;*.mp3;*.mpa;*.ogg;*.ra;*.rmi;*.spc;*.rmi;*.snd;*.umx;*.voc;*.wav;*.wma;*.xm";

        private ObservableCollection<Data> backupExtensions = new();

        private ObservableCollection<Data> backupfolders = new();

        private string customExtensions;

        private string dataSavePath;

        private string documentExtensions = "*.c;*.chm;*.cpp;*.csv;*.cxx;*.doc;*.docm;*.docx;*.dot;*.dotm;*.dotx;*.h;*.hpp;*.htm;*.html;*.hxx;*.ini;*.java;*.lua;*.mht;*.mhtml;*.odt;*.pdf;*.potx;*.potm;*.ppam;*.ppsm;*.ppsx;*.pps;*.ppt;*.pptm;*.pptx;*.rtf;*.sldm;*.sldx;*.thmx;*.txt;*.vsd;*.wpd;*.wps;*.wri;*.xlam;*.xls;*.xlsb;*.xlsm;*.xlsx;*.xltm;*.xltx;*.xml";

        private string extension;

        private int fileSize;

        private IEnumerable<object> folderExtensions;

        private int folderFileCount;

        private string folderName;

        private string pictureExtensions = "*.ani;*.bmp;*.gif;*.ico;*.jpe;*.jpeg;*.jpg;*.pcx;*.png;*.psd;*.tga;*.tif;*.tiff;*.webp;*.wmf";

        private string videoExtensions = "*.3g2;*.3gp;*.3gp2;*.3gpp;*.amr;*.amv;*.asf;*.avi;*.bdmv;*.bik;*.d2v;*.divx;*.drc;*.dsa;*.dsm;*.dss;*.dsv;*.evo;*.f4v;*.flc;*.fli;*.flic;*.flv;*.hdmov;*.ifo;*.ivf;*.m1v;*.m2p;*.m2t;*.m2ts;*.m2v;*.m4b;*.m4p;*.m4v;*.mkv;*.mp2v;*.mp4;*.mp4v;*.mpe;*.mpeg;*.mpg;*.mpls;*.mpv2;*.mpv4;*.mov;*.mts;*.ogm;*.ogv;*.pss;*.pva;*.qt;*.ram;*.ratdvd;*.rm;*.rmm;*.rmvb;*.roq;*.rpm;*.smil;*.smk;*.swf;*.tp;*.tpr;*.ts;*.vob;*.vp6;*.webm;*.wm;*.wmp;*.wmv";

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

        public string AudioExtensions
        {
            get => audioExtensions; set

            {
                if (audioExtensions != value)
                {
                    audioExtensions = value;
                    OnPropertyChanged(nameof(AudioExtensions));
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

        public string CustomExtensions
        {
            get => customExtensions;

            set
            {
                if (customExtensions != value)
                {
                    customExtensions = value;
                    OnPropertyChanged(nameof(CustomExtensions));
                }
            }
        }

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

        public string DocumentExtensions
        {
            get => documentExtensions;

            set
            {
                if (documentExtensions != value)
                {
                    documentExtensions = value;
                    OnPropertyChanged(nameof(DocumentExtensions));
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

        public int FileSize
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
            get { return folderExtensions; }

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

        public string PictureExtensions
        {
            get => pictureExtensions;

            set
            {
                if (pictureExtensions != value)
                {
                    pictureExtensions = value;
                    OnPropertyChanged(nameof(PictureExtensions));
                }
            }
        }

        public string VideoExtensions
        {
            get => videoExtensions; set

            {
                if (videoExtensions != value)
                {
                    videoExtensions = value;
                    OnPropertyChanged(nameof(VideoExtensions));
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