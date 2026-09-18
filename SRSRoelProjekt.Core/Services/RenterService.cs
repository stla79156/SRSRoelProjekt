using SRSRoelProjekt.Core.Models;
using SRSRoelProjekt.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Collections.ObjectModel;
using System.Linq;
using SRSRoelProjekt.Core.Models;


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
            return _repository.LoadRenters();
        }

        public int GenerateNewId(ObservableCollection<Renter> renters)
        {
            if (renters.Count == 0)
                return 1;

            return renters.Max(r => r.Id) + 1;
        }

        public void AddRenter(Renter renter)
        {
            var renters = _repository.LoadRenters();
            renters.Add(renter);
            _repository.SaveRenters(renters);
        }

        public void RemoveRenter(int id)
        {
            var renters = _repository.LoadRenters();
            var renter = renters.FirstOrDefault(r => r.Id == id);

            if (renter != null)
            {
                renters.Remove(renter);
                _repository.SaveRenters(renters);
            }
        }
    }
}