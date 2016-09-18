using Microsoft.Win32;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace FileIoPerformanceComparer
{
    class MainWindowViewModel : ViewModelBase
    {
        private bool _IsRunning;

        public ObservableCollection<ResultViewModel> ResultList { get; }

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

        public MainWindowViewModel()
        {
            ResultList = new ObservableCollection<ResultViewModel>();
            StartCommand = new RelayCommand(Start, canExecute => !_IsRunning);
            OpenFileCommand = new RelayCommand(OpenFile, canExecute => !_IsRunning);
            ExportResultsCommand = new RelayCommand(ExportResults, canExecute => false );

            _Filename = @"F:\Diablo III\Data\data\data.000";
        }

        private void Start(object param)
        {
            ResultList.Clear();

            Task.Factory.StartNew(() => MeasureReadFilePerformance());
        }

        private void OpenFile(object param)
        {
            var openFileDialog = new OpenFileDialog {
                FileName = Path.GetFileName(_Filename),
                InitialDirectory = Path.GetDirectoryName(_Filename) };

            if (openFileDialog.ShowDialog() == true)
            {
                _Filename = openFileDialog.FileName;
            }
        }

        private void ExportResults(object param)
        {
            throw new NotImplementedException();
        }

        private void MeasureReadFilePerformance()
        {
            _IsRunning = true;

            try
            {
                for (int i = 0; i < 3; i++)
                {
                    var stopwatch = new Stopwatch();
                    stopwatch.Start();
                    ChunkedFileReader.ReadFile(_Filename);
                    stopwatch.Stop();
                    long timeMs = stopwatch.ElapsedMilliseconds;

                    int count = i + 1;
                    CallOnUiThread(() =>
                            ResultList.Add(new ResultViewModel
                            {
                                Count = count,
                                Identifier = "Chunked File Reader",
                                BytesRead = new FileInfo(_Filename).Length,
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
