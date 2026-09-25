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
        private bool _isSelected;
        private bool _isHighlighted;
        private bool _isVisible = true;

        public int RackNumber { get; set; }
        public string? RenterName { get; set; }
        public int? RenterId { get; set; }
        public DateTime? EndDate { get; set; }
        public DateTime? AvailableFrom { get; set; }

        private string _tooltipText;

        public string TooltipText
        {
            get => _tooltipText;
            set
            {
                _tooltipText = value;
                OnPropertyChanged();
            }
        }

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

        public bool IsSelected
        {
            get => _isSelected;
            set
            {
                _isSelected = value;
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

            IsSelected 
                ? Brushes.Blue
                :   
            Status switch
            {
                RackStatus.Available => Brushes.LightGreen,
               //RackStatus.Selected => Brushes.Blue
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

        /*public bool IsReservedByAnotherRenter =>
           _main.SelectedRenter != null &&
           Status == RackStatus.Reserved &&
           RenterId != _main.SelectedRenter.RenterId;*/

        /*public string TooltipText
        {
            get
            {
                if (_main.SelectedRenter == null)
                    return "Klik for info";

                if (IsReservedByAnotherRenter)
                    return "Reolen er reserveret af en anden lejer";

                return null;
            }
        }*/

        /*private bool _isReservedByAnotherRenter;

        public bool IsReservedByAnotherRenter
        {
            get => _isReservedByAnotherRenter;
            set
            {
                _isReservedByAnotherRenter = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(ToolTipTextReservedRack));
            }
        }*/

        /*public string TooltipText =>
            _main.SelectedRenter == null ? "Klik for info" : "Reolen er reserveret af en anden lejer";

        public string ToolTipTextReservedRack =>
            IsReservedByAnotherRenter ? "Reolen er reserveret af en anden lejer" : null;*/


    }
}
