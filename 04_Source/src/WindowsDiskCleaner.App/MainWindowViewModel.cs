using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;
using WindowsDiskCleaner.Core;

namespace WindowsDiskCleaner.App
{
    public sealed class MainWindowViewModel : INotifyPropertyChanged
    {
        private readonly FileScanner _scanner;
        private readonly FileFilter _filter;
        private readonly FileDeletionService _deletionService;
        private readonly List<FileEntry> _allFiles;
        private CancellationTokenSource _cancellation;
        private FileEntryViewModel _selectedFile;
        private string _rootPath;
        private string _statusText;
        private string _keywordFilter;
        private string _extensionFilter;
        private string _minimumSizeMbFilter;
        private DateTime? _modifiedBeforeFilter;
        private QuickFilterOption _selectedQuickFilter;
        private bool _isScanning;

        public MainWindowViewModel()
        {
            _scanner = new FileScanner();
            _filter = new FileFilter();
            _deletionService = new FileDeletionService(new RecycleBinDeleteAdapter());
            _allFiles = new List<FileEntry>();
            Files = new ObservableCollection<FileEntryViewModel>();
            QuickFilters = new ObservableCollection<QuickFilterOption>(QuickFilterOption.CreateDefaults());
            _selectedQuickFilter = QuickFilters[0];
            BrowseCommand = new RelayCommand(Browse, () => !IsScanning);
            ScanCommand = new RelayCommand(StartScan, CanStartScan);
            CancelCommand = new RelayCommand(CancelScan, () => IsScanning);
            ClearFiltersCommand = new RelayCommand(ClearFilters, () => !IsScanning);
            DeleteSelectedCommand = new RelayCommand(DeleteSelectedFile, CanDeleteSelectedFile);
            StatusText = "\u8bf7\u9009\u62e9\u4e00\u4e2a\u76ee\u5f55\u5f00\u59cb\u626b\u63cf\u3002";
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public ObservableCollection<FileEntryViewModel> Files { get; private set; }

        public ObservableCollection<QuickFilterOption> QuickFilters { get; private set; }

        public ICommand BrowseCommand { get; private set; }

        public ICommand ScanCommand { get; private set; }

        public ICommand CancelCommand { get; private set; }

        public ICommand ClearFiltersCommand { get; private set; }

        public ICommand DeleteSelectedCommand { get; private set; }

        public FileEntryViewModel SelectedFile
        {
            get { return _selectedFile; }
            set
            {
                if (_selectedFile != value)
                {
                    _selectedFile = value;
                    OnPropertyChanged("SelectedFile");
                    RefreshCommands();
                }
            }
        }

        public string RootPath
        {
            get { return _rootPath; }
            set
            {
                if (_rootPath != value)
                {
                    _rootPath = value;
                    OnPropertyChanged("RootPath");
                    RefreshCommands();
                }
            }
        }

        public string StatusText
        {
            get { return _statusText; }
            private set
            {
                if (_statusText != value)
                {
                    _statusText = value;
                    OnPropertyChanged("StatusText");
                }
            }
        }

        public string KeywordFilter
        {
            get { return _keywordFilter; }
            set
            {
                if (_keywordFilter != value)
                {
                    _keywordFilter = value;
                    OnPropertyChanged("KeywordFilter");
                    ApplyFilters();
                }
            }
        }

        public string ExtensionFilter
        {
            get { return _extensionFilter; }
            set
            {
                if (_extensionFilter != value)
                {
                    _extensionFilter = value;
                    OnPropertyChanged("ExtensionFilter");
                    ApplyFilters();
                }
            }
        }

        public string MinimumSizeMbFilter
        {
            get { return _minimumSizeMbFilter; }
            set
            {
                if (_minimumSizeMbFilter != value)
                {
                    _minimumSizeMbFilter = value;
                    OnPropertyChanged("MinimumSizeMbFilter");
                    ApplyFilters();
                }
            }
        }

        public DateTime? ModifiedBeforeFilter
        {
            get { return _modifiedBeforeFilter; }
            set
            {
                if (_modifiedBeforeFilter != value)
                {
                    _modifiedBeforeFilter = value;
                    OnPropertyChanged("ModifiedBeforeFilter");
                    ApplyFilters();
                }
            }
        }

        public QuickFilterOption SelectedQuickFilter
        {
            get { return _selectedQuickFilter; }
            set
            {
                if (_selectedQuickFilter != value && value != null)
                {
                    _selectedQuickFilter = value;
                    OnPropertyChanged("SelectedQuickFilter");
                    ApplyFilters();
                }
            }
        }

        public bool IsScanning
        {
            get { return _isScanning; }
            private set
            {
                if (_isScanning != value)
                {
                    _isScanning = value;
                    OnPropertyChanged("IsScanning");
                    RefreshCommands();
                }
            }
        }

        private void Browse()
        {
            using (var dialog = new FolderBrowserDialog())
            {
                dialog.Description = "\u9009\u62e9\u8981\u626b\u63cf\u7684\u76ee\u5f55";
                dialog.ShowNewFolderButton = false;

                if (!string.IsNullOrWhiteSpace(RootPath))
                {
                    dialog.SelectedPath = RootPath;
                }

                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    RootPath = dialog.SelectedPath;
                }
            }
        }

        private bool CanStartScan()
        {
            return !IsScanning && !string.IsNullOrWhiteSpace(RootPath);
        }

        private async void StartScan()
        {
            if (!CanStartScan())
            {
                return;
            }

            _allFiles.Clear();
            Files.Clear();
            SelectedFile = null;
            IsScanning = true;
            _cancellation = new CancellationTokenSource();
            StatusText = "\u6b63\u5728\u626b\u63cf...";

            try
            {
                IProgress<ScanProgress> progress = new Progress<ScanProgress>(UpdateProgress);
                var token = _cancellation.Token;
                var rootPath = RootPath;

                var result = await Task.Run(
                    () => _scanner.Scan(new ScanOptions(rootPath), token, progress.Report),
                    token);

                _allFiles.Clear();
                _allFiles.AddRange(result.Files);
                RebuildVisibleFiles(result.Files);

                StatusText = result.WasCancelled
                    ? BuildStatus("\u626b\u63cf\u5df2\u53d6\u6d88", result.Files.Count, result.Files.Count, result.Files.Sum(file => file.SizeBytes), result.SkippedCount)
                    : BuildStatus("\u626b\u63cf\u5b8c\u6210", result.Files.Count, result.Files.Count, result.Files.Sum(file => file.SizeBytes), result.SkippedCount);
            }
            catch (OperationCanceledException)
            {
                StatusText = "\u626b\u63cf\u5df2\u53d6\u6d88\u3002";
            }
            catch (Exception exception)
            {
                StatusText = "\u626b\u63cf\u5931\u8d25\uff1a" + exception.Message;
            }
            finally
            {
                IsScanning = false;
                if (_cancellation != null)
                {
                    _cancellation.Dispose();
                    _cancellation = null;
                }
            }
        }

        private void CancelScan()
        {
            if (_cancellation != null)
            {
                _cancellation.Cancel();
                StatusText = "\u6b63\u5728\u53d6\u6d88\u626b\u63cf...";
            }
        }

        private void UpdateProgress(ScanProgress progress)
        {
            StatusText = "\u6b63\u5728\u626b\u63cf\uff1a" + progress.CurrentPath
                + " | \u5df2\u53d1\u73b0 " + progress.FileCount + " \u4e2a\u6587\u4ef6"
                + " | \u8df3\u8fc7 " + progress.SkippedCount + " \u4e2a\u8def\u5f84";
        }

        private void ApplyFilters()
        {
            if (IsScanning || _allFiles.Count == 0)
            {
                return;
            }

            var options = BuildFilterOptions();
            var visible = _filter.Apply(_allFiles, options).ToList();
            RebuildVisibleFiles(visible);
            StatusText = BuildStatus("\u7b5b\u9009\u7ed3\u679c", visible.Count, _allFiles.Count, visible.Sum(file => file.SizeBytes), 0);
        }

        private FileFilterOptions BuildFilterOptions()
        {
            var options = new FileFilterOptions
            {
                Keyword = KeywordFilter,
                Extension = ExtensionFilter,
                ModifiedBefore = ModifiedBeforeFilter,
                QuickFilter = SelectedQuickFilter == null ? QuickFileFilter.All : SelectedQuickFilter.Value
            };

            double minimumMb;
            if (double.TryParse(MinimumSizeMbFilter, NumberStyles.Number, CultureInfo.CurrentCulture, out minimumMb) && minimumMb > 0)
            {
                options.MinimumSizeBytes = (long)(minimumMb * 1024D * 1024D);
            }

            return options;
        }

        private void ClearFilters()
        {
            KeywordFilter = string.Empty;
            ExtensionFilter = string.Empty;
            MinimumSizeMbFilter = string.Empty;
            ModifiedBeforeFilter = null;
            SelectedQuickFilter = QuickFilters[0];
            ApplyFilters();
        }

        private bool CanDeleteSelectedFile()
        {
            return !IsScanning && SelectedFile != null;
        }

        private void DeleteSelectedFile()
        {
            if (!CanDeleteSelectedFile())
            {
                return;
            }

            var selected = SelectedFile;
            var firstConfirm = System.Windows.MessageBox.Show(
                "\u786e\u8ba4\u8981\u5c06\u8be5\u6587\u4ef6\u79fb\u5165\u56de\u6536\u7ad9\u5417\uff1f\n\n"
                    + selected.Name + "\n" + selected.FullPath + "\n\n"
                    + "\u6587\u4ef6\u7ea7\u522b\uff1a" + selected.RiskLevelDisplay + "\n"
                    + selected.RiskReason,
                "\u786e\u8ba4\u5220\u9664\u6587\u4ef6",
                MessageBoxButton.YesNo,
                MessageBoxImage.Warning);

            if (firstConfirm != MessageBoxResult.Yes)
            {
                StatusText = "\u5df2\u53d6\u6d88\u5220\u9664\u3002";
                return;
            }

            var confirmation = FileDeleteConfirmation.Confirmed;

            if (selected.IsHighRisk)
            {
                var secondConfirm = System.Windows.MessageBox.Show(
                    "\u8fd9\u662f\u9ad8\u98ce\u9669\u6587\u4ef6\uff0c\u5220\u9664\u53ef\u80fd\u5bfc\u81f4\u7cfb\u7edf\u6216\u7a0b\u5e8f\u5f02\u5e38\u3002\n\n"
                        + "\u5982\u679c\u4f60\u4ecd\u7136\u8981\u5220\u9664\uff0c\u8bf7\u518d\u6b21\u786e\u8ba4\u3002",
                    "\u9ad8\u98ce\u9669\u6587\u4ef6\u4e8c\u6b21\u786e\u8ba4",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Stop);

                if (secondConfirm != MessageBoxResult.Yes)
                {
                    StatusText = "\u5df2\u53d6\u6d88\u9ad8\u98ce\u9669\u6587\u4ef6\u5220\u9664\u3002";
                    return;
                }

                confirmation = FileDeleteConfirmation.HighRiskConfirmed;
            }

            var result = _deletionService.DeleteToRecycleBin(selected.Entry, confirmation);
            if (!result.Succeeded)
            {
                StatusText = "\u5220\u9664\u5931\u8d25\uff1a" + result.Message;
                return;
            }

            _allFiles.RemoveAll(file => string.Equals(file.FullPath, selected.FullPath, StringComparison.OrdinalIgnoreCase));
            SelectedFile = null;
            ApplyFilters();
            StatusText = "\u5df2\u79fb\u5165\u56de\u6536\u7ad9\uff1a" + result.DeletedPath;
        }

        private void RebuildVisibleFiles(IEnumerable<FileEntry> files)
        {
            Files.Clear();
            foreach (var file in files)
            {
                Files.Add(new FileEntryViewModel(file));
            }
        }

        private static string BuildStatus(string prefix, int visibleCount, int totalCount, long visibleBytes, int skippedCount)
        {
            return prefix
                + "\u3002\u663e\u793a " + visibleCount + " / " + totalCount + " \u4e2a\u6587\u4ef6"
                + "\uff0c\u5f53\u524d\u7ed3\u679c\u5408\u8ba1 " + FormatSize(visibleBytes)
                + "\uff0c\u8df3\u8fc7 " + skippedCount + " \u4e2a\u8def\u5f84\u3002";
        }

        private static string FormatSize(long bytes)
        {
            string[] units = { "B", "KB", "MB", "GB", "TB" };
            var size = (double)bytes;
            var unit = 0;

            while (size >= 1024 && unit < units.Length - 1)
            {
                size = size / 1024;
                unit++;
            }

            return unit == 0 ? bytes + " " + units[unit] : size.ToString("0.##") + " " + units[unit];
        }

        private void RefreshCommands()
        {
            ((RelayCommand)BrowseCommand).RaiseCanExecuteChanged();
            ((RelayCommand)ScanCommand).RaiseCanExecuteChanged();
            ((RelayCommand)CancelCommand).RaiseCanExecuteChanged();
            ((RelayCommand)ClearFiltersCommand).RaiseCanExecuteChanged();
            ((RelayCommand)DeleteSelectedCommand).RaiseCanExecuteChanged();
        }

        private void OnPropertyChanged(string propertyName)
        {
            var handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
