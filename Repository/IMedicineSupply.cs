using Microsoft.AspNetCore.Mvc;
using PharmacyMedicineSupplyMicroservice.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PharmacyMedicineSupplyMicroservice.Repository
{
   public interface IMedicineSupply
    {
        public IEnumerable<MedicineStock> GetSupply(string token);
    }
}
