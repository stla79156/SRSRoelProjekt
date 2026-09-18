using SRSRoelProjekt.Core.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Text.Json;

namespace SRSRoelProjekt.Core.Repositories
{
    public class JsonRenterRepository : IRenterRepository
    {
        private readonly string _filePath = "renters.json";

        public ObservableCollection<Renter> LoadRenters()
        {
            if (!File.Exists(_filePath))
                return new ObservableCollection<Renter>();

            var json = File.ReadAllText(_filePath);
            return JsonSerializer.Deserialize<ObservableCollection<Renter>>(json)
                   ?? new ObservableCollection<Renter>();
        }

        public void SaveRenters(ObservableCollection<Renter> renters)
        {
            var json = JsonSerializer.Serialize(renters, new JsonSerializerOptions
            {
                WriteIndented = true
            });

            File.WriteAllText(_filePath, json);
        }
    }
}
