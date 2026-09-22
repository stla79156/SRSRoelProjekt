using SRSRoelProjekt.Core.Models;
using SRSRoelProjekt.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Linq;


namespace SRSRoelProjekt.Core.Services
{
    public class RenterService
    {
        private readonly IRenterRepository _repository;

        public RenterService(IRenterRepository repository)
        {
            _repository = repository;
        }

        public ObservableCollection<Renter> GetRenters()
        {
            return new ObservableCollection<Renter>(_repository.GetRenters());
        }

        public void AddRenter(Renter renter)
        {
            _repository.AddRenter(renter);
        }

        public void RemoveRenter(int id)
        {
            _repository.RemoveRenter(id);
        }

        public void UpdateRenter(Renter renter)
        {
            _repository.UpdateRenter(renter);
        }

        public int GenerateNewId(IEnumerable<Renter> renters)
        {
            if (!renters.Any())
                return 1;

            return renters.Max(r => r.Id) + 1;
        }
    }
}