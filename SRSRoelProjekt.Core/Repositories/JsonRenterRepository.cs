using SRSRoelProjekt.Core.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Text.Json;
using System.Windows;

namespace SRSRoelProjekt.Core.Repositories
{
    public class JsonRenterRepository : IRenterRepository
    {
            private readonly string _filePath;

            public JsonRenterRepository(string? filePath = null)
            {
                if (filePath != null)
                {
                    _filePath = filePath;
                }
                else
                {
                    DirectoryInfo? directory =
                        new DirectoryInfo(AppContext.BaseDirectory);

                    while (directory != null)
                    {
                        string dataDirectory =
                            Path.Combine(directory.FullName,
                                         "SRSRoelProjekt.Core",
                                         "JsonData");

                        if (Directory.Exists(dataDirectory))
                        {
                            _filePath = Path.Combine(dataDirectory, "renters.json");
                            break;
                        }

                        directory = directory.Parent;
                    }

                    if (directory == null)
                    {
                        throw new DirectoryNotFoundException(
                            "Kunne ikke finde SRSRoelProjekt.Core/JsonData.");
                    }
                }

                Directory.CreateDirectory(Path.GetDirectoryName(_filePath)!);

                if (!File.Exists(_filePath))
                {
                    File.WriteAllText(_filePath, "[]");
                }
            }

            public List<Renter> GetRenters()
            {
                string json = File.ReadAllText(_filePath);

                if (string.IsNullOrWhiteSpace(json))
                    return new List<Renter>();

                return JsonSerializer.Deserialize<List<Renter>>(json)
                       ?? new List<Renter>();
            }

            public void SaveRenters(List<Renter> renters)
            {
                var json = JsonSerializer.Serialize(
                    renters,
                    new JsonSerializerOptions
                    {
                        WriteIndented = true
                    });

                File.WriteAllText(_filePath, json);

            }
        
         
    }
}
