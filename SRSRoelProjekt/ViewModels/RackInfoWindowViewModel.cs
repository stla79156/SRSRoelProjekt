using SRSRoelProjekt.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Controls;
using SRSRoelProjekt.Views;
using SRSRoelProjekt.Views.Windows;

namespace SRSRoelProjekt.ViewModels
{
    public class RackInfoWindowViewModel
    {
            public string RackNumber { get; }
            public string RenterName { get; }
            public string Status { get; }
            public string Type { get; }
            public string EndDate { get; }
            public string AvailableFrom { get; }

            public RackInfoWindowViewModel(RackViewModel rack)
            {
                RackNumber = $"Reol: {rack.RackNumber}";
                RenterName = $"Lejer: {rack.RenterName ?? "Ingen"}";
                Status = $"Status: {rack.Status}";

                Type = rack.WithHanger
                    ? "Reoltype: Med bøjlestang"
                    : "Reoltype: Uden bøjlestang";

                EndDate = $"Slutdato: {rack.EndDate?.ToShortDateString() ?? "N/A"}";
                AvailableFrom = $"Ledig fra: {rack.AvailableFrom?.ToShortDateString() ?? "N/A"}";
            }

        
    }
}

