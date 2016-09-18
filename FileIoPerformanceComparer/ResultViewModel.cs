namespace FileIoPerformanceComparer
{
    class ResultViewModel : ViewModelBase
    {
        private string _Identifier;

        public string Identifier
        {
            get { return _Identifier; }
            set { _Identifier = value; InvokePropertyChanged(); }
        }

        private int _Count;

        public int Count
        {
            get { return _Count; }
            set { _Count = value; InvokePropertyChanged(); }
        }

        private long _BytesRead;

        public long BytesRead
        {
            get { return _BytesRead; }
            set { _BytesRead = value; InvokePropertyChanged(); }
        }

        private long _AverageTimeMs;

        public long AverageTimeMs
        {
            get { return _AverageTimeMs; }
            set { _AverageTimeMs = value; InvokePropertyChanged(); }
        }
    }
}
