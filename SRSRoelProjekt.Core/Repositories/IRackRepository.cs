using SRSRoelProjekt.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace SRSRoelProjekt.Core.Repositories
{
    public interface IRackRepository
    {
        void UpdateRack(Rack rack);
        List<Rack> GetRacks();
    }
}
