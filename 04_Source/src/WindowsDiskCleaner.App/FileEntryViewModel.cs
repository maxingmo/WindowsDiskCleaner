using System;
using System.ComponentModel;
using WindowsDiskCleaner.Core;

namespace WindowsDiskCleaner.App
{
    public sealed class FileEntryViewModel : INotifyPropertyChanged
    {
        private readonly FileEntry _entry;
        private readonly FileRiskAssessment _riskAssessment;
        private readonly FileRiskVisualProfile _riskVisualProfile;
        private bool _isSelected;

        public FileEntryViewModel(FileEntry entry)
        {
            _entry = entry;
            _riskAssessment = new FileRiskClassifier().Classify(entry);
            _riskVisualProfile = FileRiskVisualProfile.ForLevel(_riskAssessment.Level);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public bool IsSelected
        {
            get { return _isSelected; }
            set
            {
                if (_isSelected != value)
                {
                    _isSelected = value;
                    OnPropertyChanged("IsSelected");
                }
            }
        }

        public string Name
        {
            get { return _entry.Name; }
        }

        public string FullPath
        {
            get { return _entry.FullPath; }
        }

        public long SizeBytes
        {
            get { return _entry.SizeBytes; }
        }

        public string SizeDisplay
        {
            get { return FormatSize(_entry.SizeBytes); }
        }

        public string RiskLevelDisplay
        {
            get { return _riskAssessment.DisplayName; }
        }

        public string RiskReason
        {
            get { return _riskAssessment.Reason; }
        }

        public string RiskBadgeText
        {
            get { return _riskVisualProfile.BadgeText; }
        }

        public string RiskForegroundHex
        {
            get { return _riskVisualProfile.ForegroundHex; }
        }

        public string RiskBackgroundHex
        {
            get { return _riskVisualProfile.BackgroundHex; }
        }

        public string RiskBorderHex
        {
            get { return _riskVisualProfile.BorderHex; }
        }

        public bool IsHighRisk
        {
            get { return _riskVisualProfile.IsHighRisk; }
        }

        public FileEntry Entry
        {
            get { return _entry; }
        }

        public DateTime CreatedAt
        {
            get { return _entry.CreatedAt; }
        }

        public DateTime ModifiedAt
        {
            get { return _entry.ModifiedAt; }
        }

        public string Extension
        {
            get { return string.IsNullOrEmpty(_entry.Extension) ? "(\u65e0)" : _entry.Extension; }
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

            return unit == 0
                ? bytes + " " + units[unit]
                : size.ToString("0.##") + " " + units[unit];
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
