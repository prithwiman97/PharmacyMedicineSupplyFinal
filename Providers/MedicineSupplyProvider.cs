using PharmacyMedicineSupplyMicroservice.Models;
using PharmacyMedicineSupplyMicroservice.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using log4net;

namespace PharmacyMedicineSupplyMicroservice.Providers
{
    public class MedicineSupplyProvider
    {
        private readonly IMedicineSupply _repocontext;
        readonly log4net.ILog _log4net;
        public MedicineSupplyProvider(IMedicineSupply repo)
        {
            _repocontext = repo;
            _log4net = log4net.LogManager.GetLogger(typeof(MedicineSupplyProvider));
        }
        /// <summary>
        /// This is the function where we get the medicinestock(Called from MedicineRepo) and 
        /// perform some functionalities to fetch the numberoftablets of a particular medicine
        /// and check what should be the supplycount(i.e how many medicines can be supplied to user).
        /// Try and catch blocks are handled accordingly. 
        /// </summary>
        /// <param name="demand"></param>
        /// <returns>MedicineSupply or null value depending on the type of demand</returns>
        public  MedicineSupply GetSupply(MedicineDemand demand,string token)
        {
            IEnumerable<MedicineStock> medicinestock = _repocontext.GetSupply(token);
            medicinestock = from m in medicinestock where m.Name == demand.MedicineName select m;
            MedicineStock medicine = medicinestock.FirstOrDefault();
            try
            {
                if (medicine != null)
                {
                    int medicinecount = ((medicine.numberOfTabletsInStock) / (MedicineSupplyRepo.pharmacies.Count()));
                    MedicineSupply medicinesupply = new MedicineSupply();
                    medicinesupply.MedicineName = medicine.Name;
                    medicinesupply.DemandCount = demand.DemandCount;
                    medicinesupply.SupplyCount = medicinecount;
                    return medicinesupply;
                }
                else
                {
                    _log4net.Error("MedicineName not found in medicinestock "+nameof(MedicineSupplyProvider));
                    return null;
                }
            }
            catch (Exception e)
            {
                _log4net.Error(e.Message+" from " +nameof(MedicineSupplyProvider));
                throw e;
            }
        }
    }
}
