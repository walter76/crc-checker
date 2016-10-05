using System;

namespace FileIoPerformanceComparer
{
    internal class BinaryReaderViewModel : ViewModelBase
    {
        private string _Name;

        public string Name
        {
            get { return _Name; }
            set
            {
                _Name = value;
                InvokePropertyChanged();
            }
        }

        public Action<string> ReadFileAction { get; set; }
    }
}
