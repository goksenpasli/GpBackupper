﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace GpBackupper.Properties {
    
    
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "16.10.0.0")]
    internal sealed partial class Settings : global::System.Configuration.ApplicationSettingsBase {
        
        private static Settings defaultInstance = ((Settings)(global::System.Configuration.ApplicationSettingsBase.Synchronized(new Settings())));
        
        public static Settings Default {
            get {
                return defaultInstance;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("*.aac;*.ac3;*.aif;*.aifc;*.aiff;*.au;*.cda;*.dts;*.fla;*.flac;*.it;*.m1a;*.m2a;*." +
            "m3u;*.m4a;*.mid;*.midi;*.mka;*.mod;*.mp2;*.mp3;*.mpa;*.ogg;*.ra;*.rmi;*.spc;*.rm" +
            "i;*.snd;*.umx;*.voc;*.wav;*.wma;*.xm")]
        public string AudioExtensions {
            get {
                return ((string)(this["AudioExtensions"]));
            }
            set {
                this["AudioExtensions"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute(@"*.c;*.chm;*.cpp;*.csv;*.cxx;*.doc;*.docm;*.docx;*.dot;*.dotm;*.dotx;*.h;*.hpp;*.htm;*.html;*.hxx;*.ini;*.java;*.lua;*.mht;*.mhtml;*.odt;*.pdf;*.potx;*.potm;*.ppam;*.ppsm;*.ppsx;*.pps;*.ppt;*.pptm;*.pptx;*.rtf;*.sldm;*.sldx;*.thmx;*.txt;*.vsd;*.wpd;*.wps;*.wri;*.xlam;*.xls;*.xlsb;*.xlsm;*.xlsx;*.xltm;*.xltx;*.xml")]
        public string DocumentExtensions {
            get {
                return ((string)(this["DocumentExtensions"]));
            }
            set {
                this["DocumentExtensions"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("*.ani;*.bmp;*.gif;*.ico;*.jpe;*.jpeg;*.jpg;*.pcx;*.png;*.psd;*.tga;*.tif;*.tiff;*" +
            ".webp;*.wmf")]
        public string PictureExtensions {
            get {
                return ((string)(this["PictureExtensions"]));
            }
            set {
                this["PictureExtensions"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute(@"*.3g2;*.3gp;*.3gp2;*.3gpp;*.amr;*.amv;*.asf;*.avi;*.bdmv;*.bik;*.d2v;*.divx;*.drc;*.dsa;*.dsm;*.dss;*.dsv;*.evo;*.f4v;*.flc;*.fli;*.flic;*.flv;*.hdmov;*.ifo;*.ivf;*.m1v;*.m2p;*.m2t;*.m2ts;*.m2v;*.m4b;*.m4p;*.m4v;*.mkv;*.mp2v;*.mp4;*.mp4v;*.mpe;*.mpeg;*.mpg;*.mpls;*.mpv2;*.mpv4;*.mov;*.mts;*.ogm;*.ogv;*.pss;*.pva;*.qt;*.ram;*.ratdvd;*.rm;*.rmm;*.rmvb;*.roq;*.rpm;*.smil;*.smk;*.swf;*.tp;*.tpr;*.ts;*.vob;*.vp6;*.webm;*.wm;*.wmp;*.wmv")]
        public string VideoExtensions {
            get {
                return ((string)(this["VideoExtensions"]));
            }
            set {
                this["VideoExtensions"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("")]
        public string CustomExtensions {
            get {
                return ((string)(this["CustomExtensions"]));
            }
            set {
                this["CustomExtensions"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("0")]
        public int ShutDownMode {
            get {
                return ((int)(this["ShutDownMode"]));
            }
            set {
                this["ShutDownMode"] = value;
            }
        }
    }
}
