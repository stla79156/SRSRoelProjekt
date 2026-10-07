using System;
using System.Collections.Generic;
using System.Text;

namespace SRSRoelProjekt.Core.Models
{
    public class PaymentMethod
    {
        public int PaymentMethodId { get; set; }
        public string PaymentMethodName { get; set; } //Bliver brugt i database, men ville blive brugt, hvis vi yderligere implimenterede daglige udtræk af betalinger.
    }
}
