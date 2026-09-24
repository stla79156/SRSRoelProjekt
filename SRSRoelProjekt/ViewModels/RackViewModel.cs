using SRSRoelProjekt.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using SRSRoelProjekt.Commands;
using SRSRoelProjekt.Views.Windows;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Media;

namespace SRSRoelProjekt.ViewModels
{
    public class RackViewModel : ViewModelBase
    {
        private RackStatus _status;
        private bool _isHighlighted;
        private bool _isVisible = true;

        public int RackNumber { get; set; }
        public string? RenterName { get; set; }
        public DateTime? EndDate { get; set; }
        public DateTime? AvailableFrom { get; set; }

        public RackStatus Status
        {
            get => _status;
            set
            {
                _status = value;

                OnPropertyChanged();
                OnPropertyChanged(nameof(BackgroundColor));
            }
        }
        public bool WithHanger { get; set; }

        public string DisplayText => WithHanger
        ? $"{RackNumber}\nb"
        : RackNumber.ToString();


        public bool IsHighlighted
        {
            get => _isHighlighted;
            set
            {
                _isHighlighted = value;

                OnPropertyChanged();
                OnPropertyChanged(nameof(BorderBrush));
                OnPropertyChanged(nameof(BorderThickness));
            }
        }

        public bool IsVisible
        {
            get => _isVisible;
            set
            {
                _isVisible = value;

                OnPropertyChanged();
                OnPropertyChanged(nameof(RackVisibility));
            }
        }

        public Visibility RackVisibility =>
            IsVisible
                ? Visibility.Visible
                : Visibility.Hidden;

        public Brush BackgroundColor =>
            Status switch
            {
                RackStatus.Available => Brushes.LightGreen,
                RackStatus.Selected => Brushes.Blue,
                RackStatus.Reserved => Brushes.Red,
                RackStatus.EndingSoon => Brushes.Yellow,
                _ => Brushes.Gray
            };

        public Brush BorderBrush =>
            IsHighlighted
                ? Brushes.Blue
                : Brushes.Black;

        public double BorderThickness =>
            IsHighlighted ? 3 : 1;

        private bool _canShowInfo;

        public bool CanShowInfo
        {
            get => _canShowInfo;
            set
            {
                _canShowInfo = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(TooltipText));
            }
        }

        public string TooltipText =>
            CanShowInfo ? "Klik for info" : null;
       
    }
}
