using System;

namespace Extensions
{
    public class ArchiveData : InpcBase
    {
        private long boyut;

        private string crc;

        private string dosyaAdı;

        private DateTime? düzenlenmeZamanı;

        private double oran;

        private long sıkıştırılmışBoyut;

        public long Boyut
        {
            get => boyut;

            set
            {
                if (boyut != value)
                {
                    boyut = value;
                    OnPropertyChanged(nameof(Boyut));
                }
            }
        }

        public string Crc
        {
            get => crc;

            set
            {
                if (crc != value)
                {
                    crc = value;
                    OnPropertyChanged(nameof(Crc));
                }
            }
        }

        public string DosyaAdı
        {
            get => dosyaAdı;

            set
            {
                if (dosyaAdı != value)
                {
                    dosyaAdı = value;
                    OnPropertyChanged(nameof(DosyaAdı));
                }
            }
        }

        public DateTime? DüzenlenmeZamanı
        {
            get => düzenlenmeZamanı;

            set
            {
                if (düzenlenmeZamanı != value)
                {
                    düzenlenmeZamanı = value;
                    OnPropertyChanged(nameof(DüzenlenmeZamanı));
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

        public long SıkıştırılmışBoyut
        {
            get => sıkıştırılmışBoyut;

            set
            {
                if (sıkıştırılmışBoyut != value)
                {
                    sıkıştırılmışBoyut = value;
                    OnPropertyChanged(nameof(SıkıştırılmışBoyut));
                }
            }
        }
    }
}