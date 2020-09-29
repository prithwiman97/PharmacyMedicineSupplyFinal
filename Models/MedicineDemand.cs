using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PharmacyMedicineSupplyMicroservice.Models
{
    public class MedicineDemand
    {
        public string MedicineName { get; set; }

        public int DemandCount { get; set; }
    }
}
