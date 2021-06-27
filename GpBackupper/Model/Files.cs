namespace GpBackupper
{
    public class Files : SearchControlViewModel
    {
        private string fileName;

        private long fileSize;

        private bool ısChecked;

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

        public long FileSize
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

        public bool IsChecked
        {
            get => ısChecked; set

            {
                if (ısChecked != value)
                {
                    ısChecked = value;
                    OnPropertyChanged(nameof(IsChecked));
                }
            }
        }
    }
}