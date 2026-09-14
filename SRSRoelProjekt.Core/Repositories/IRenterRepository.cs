using SRSRoelProjekt.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace SRSRoelProjekt.Core.Repositories
{
    public interface IRenterRepository
    {
        void Add(Renter renter);
        List<Renter> GetAll(); 

    }


    
    
}
