using System;
using System.Collections.Generic;
using System.Text;

namespace SRSRoelProjekt.Core.Models
{
    public class Payment
    {
        public int PaymentId { get; set; } //Hvis dagsudtræk over alle betalinger den da, ville man bruge PaymentId.
        public DateTime PaymentDate { get; set; } = DateTime.Now;
        public decimal Amount { get; set; }
        public int ShoppingCartId { get; set; }
        public int PaymentMethodId { get; set; }

    }
}
