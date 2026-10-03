using System;
using System.Collections.Generic;
using System.Text;

namespace SRSRoelProjekt.Core.Models
{
    public class Product
    {
        public int ProductNumber { get; set; }
        public string ProductName { get; set; }
        public string ProductDescription { get; set; }
        public decimal Price { get; set; }
        public bool IsSold { get; set; }
        public string EAN13Number { get; set; }
        public int RackNumber { get; set; }


        /// <summary>
        /// Genererer og gemmer en gyldig EAN-13 stregkode på produktet
        /// baseret på landekode 57 og virksomhedsnummer 23987.
        /// </summary>
        public void GenerateEan13()
        {
            string prefix = "57";
            string companyNumber = "23987";

            // :D5 formaterer ProductNumber, så det altid fylder 5 cifre med foranstillede nuller (f.eks. 1 bliver til 00001)
            string first12Digits = $"{prefix}{companyNumber}{ProductNumber:D5}";

            int sum = 0;
            for (int i = 0; i < 12; i++)
            {
                int digit = int.Parse(first12Digits[i].ToString());
                // Ganger skiftevis med 1 og 3 (EAN-13 standard)
                sum += (i % 2 == 0) ? digit * 1 : digit * 3;
            }

            int remainder = sum % 10;
            int checkDigit = (remainder == 0) ? 0 : 10 - remainder;

            // Sætter den fulde 13-cifrede kode på produktet
            this.EAN13Number = first12Digits + checkDigit;
        }
    }

            int remainder = sum % 10;
            int checkDigit = (remainder == 0) ? 0 : 10 - remainder;

            // Sætter den fulde 13-cifrede kode på produktet
            this.EAN13Number = first12Digits + checkDigit;
        }


    }


}
