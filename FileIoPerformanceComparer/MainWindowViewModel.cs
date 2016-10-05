using Microsoft.Win32;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace FileIoPerformanceComparer
{
    internal class MainWindowViewModel : ViewModelBase
    {
        private bool _IsRunning;

        public ObservableCollection<ResultViewModel> ResultList { get; }

        public ObservableCollection<BinaryReaderViewModel> BinaryReaderMethodList { get; }

        private BinaryReaderViewModel _SelectedBinaryReaderMethod;

        public BinaryReaderViewModel SelectedBinaryReaderMethod
        {
            get { return _SelectedBinaryReaderMethod; }
            set
            {
                _SelectedBinaryReaderMethod = value;
                InvokePropertyChanged();
            }
        }

        public ICommand StartCommand { get; }
        public ICommand OpenFileCommand { get; }
        public ICommand ExportResultsCommand { get; }

        private string _Filename;

        public string Filename
        {
            get { return _Filename; }
            set
            {
                _Filename = value;
                InvokePropertyChanged();
            }
        }

        private int _Repetitions;

        public int Repetitions
        {
            get { return _Repetitions; }
            set
            {
                _Repetitions = value;
                InvokePropertyChanged();
            }
        }

        public MainWindowViewModel()
        {
            ResultList = new ObservableCollection<ResultViewModel>();
            BinaryReaderMethodList = new ObservableCollection<BinaryReaderViewModel>();
            StartCommand = new RelayCommand(Start, canExecute => !_IsRunning);
            OpenFileCommand = new RelayCommand(OpenFile, canExecute => !_IsRunning);
            ExportResultsCommand = new RelayCommand(ExportResults, canExecute => false);

            Filename = @"F:\Diablo III\Data\data\data.000";
            Repetitions = 3;

            PrepareBinaryReaderMethodList();
        }

        private void Start(object param)
        {
            ResultList.Clear();

            Task.Factory.StartNew(() => MeasureReadFilePerformance());
        }

        private void OpenFile(object param)
        {
            var openFileDialog = new OpenFileDialog {
                FileName = Path.GetFileName(Filename),
                InitialDirectory = Path.GetDirectoryName(Filename) };

            if (openFileDialog.ShowDialog() == true)
            {
                Filename = openFileDialog.FileName;
            }
        }

        private void ExportResults(object param)
        {
            throw new NotImplementedException();
        }

        private void PrepareBinaryReaderMethodList()
        {
            AddBinaryReaderMethod(
                "Chunked File Reader", (filename) => ChunkedFileReader.ReadFile(filename));
            AddBinaryReaderMethod(
                "Pinned Buffer Chunked File Reader", (filename) => PinnedBufferChunkedFileReader.ReadFile(filename));
            AddBinaryReaderMethod(
                "File Stream File Reader", (filename) => FileStreamFileReader.ReadFile(filename));
            AddBinaryReaderMethod(
                "File Read All Bytes File Reader", (filename) => FileReadAllBytesFileReader.ReadFile(filename));

            SelectedBinaryReaderMethod = BinaryReaderMethodList.First();
        }

        private void AddBinaryReaderMethod(string name, Action<string> readFileAction)
        {
            BinaryReaderMethodList.Add(
                new BinaryReaderViewModel
                {
                    Name = name,
                    ReadFileAction = readFileAction
                });
        }

        private void MeasureReadFilePerformance()
        {
            _IsRunning = true;

            try
            {
                for (int i = 0; i < Repetitions; i++)
                {
                    var stopwatch = new Stopwatch();
                    stopwatch.Start();
                    SelectedBinaryReaderMethod.ReadFileAction(Filename);
                    stopwatch.Stop();
                    long timeMs = stopwatch.ElapsedMilliseconds;

                    int count = i + 1;
                    CallOnUiThread(() =>
                            ResultList.Add(new ResultViewModel
                            {
                                Count = count,
                                Identifier = _SelectedBinaryReaderMethod.Name,
                                BytesRead = new FileInfo(Filename).Length,
                                AverageTimeMs = timeMs
                            }));
                }
            }
            finally
            {
                _IsRunning = false;

                UpdateCommandsEnablement();
            }
        }

        private void UpdateCommandsEnablement()
        {
            CallOnUiThread(() => ((RelayCommand)StartCommand).RaiseCanExecuteChanged());
            CallOnUiThread(() => ((RelayCommand)OpenFileCommand).RaiseCanExecuteChanged());
            CallOnUiThread(() => ((RelayCommand)ExportResultsCommand).RaiseCanExecuteChanged());
        }

        private void CallOnUiThread(Action action)
        {
            Application.Current.Dispatcher.BeginInvoke(action);
        }
    }
}
