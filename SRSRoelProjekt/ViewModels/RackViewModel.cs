using SRSRoelProjekt.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Media;

namespace SRSRoelProjekt.ViewModels
{
    public class RackViewModel : ViewModelBase
    {
        private RackStatus _status;
        private bool _isHighlighted;
      

    public Visibility RackVisibility =>
        IsVisible
            ? Visibility.Visible
            : Visibility.Hidden;

    public int RackNumber { get; set; }

        public string RenterName { get; set; }

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


        private bool _isVisible = true;

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
    }

}

