using System;
using System.Collections.Generic;
using System.Text;

namespace SRSRoelProjekt.Core.Models
{
    public class Renter
    {
        public int RenterId { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }

        public string PhoneNumber { get; set; }

        public string Username { get; set; }

        //Bliver brugt i xaml
        public string DisplayText => $"{RenterId} - {Name}";

        
    }
}
