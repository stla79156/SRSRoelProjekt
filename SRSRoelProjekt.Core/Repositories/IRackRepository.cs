using SRSRoelProjekt.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace SRSRoelProjekt.Core.Repositories
{
    public interface IRackRepository
    {
        List<Rack> GetRacks();

        void StartRental(int rackNumber, int renterId);

        void StopRental(int rackNumber);
    }
}
