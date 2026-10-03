using SRSRoelProjekt.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace SRSRoelProjekt.Core.Repositories
{
    public interface IPaymentRepository
    {
        void CreatePayment(Payment payment);
    }
}
