using SRSRoelProjekt.Core.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace SRSRoelProjekt.Core.Repositories
{
    public interface IRenterRepository
    {
        List<Renter> GetRenters();

        //void SaveRenters(List<Renter> renters);

        void AddRenter(Renter renter);
        void RemoveRenter(Renter renter);
        void UpdateRenter(Renter renter);

    }


    
    
}
