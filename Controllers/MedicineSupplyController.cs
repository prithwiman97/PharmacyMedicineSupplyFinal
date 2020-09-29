using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PharmacyMedicineSupplyMicroservice.Models;
using PharmacyMedicineSupplyMicroservice.Providers;
using PharmacyMedicineSupplyMicroservice.Repository;

namespace PharmacyMedicineSupplyMicroservice.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MedicineSupplyController : ControllerBase
    {
        readonly log4net.ILog _log4net;
        private readonly MedicineSupplyProvider _providercontext;
        public MedicineSupplyController(MedicineSupplyProvider provider)
        {
            _providercontext = provider;
            _log4net = log4net.LogManager.GetLogger(typeof(MedicineSupplyController));
        }
      /// <summary>
      /// This function serves as the end point.The demand is fed in this fuction from the UI
      /// The getSupply method of the provider class is called from here,where further 
      /// calculations are done.The try catch blocks are handled accordingly.
      /// </summary>
      /// <param name="demand"></param>
      /// <returns>the status code whether or not this functions correctly as per the demand</returns>      
        [Authorize]
        [HttpPost]
        public IActionResult GetSupply([FromBody] MedicineDemand demand)
        {
            if (demand.MedicineName == null && demand.DemandCount<=0)
            {
                _log4net.Error("Null parameters passed from " + nameof(MedicineSupplyController));
                return BadRequest("Please enter atleast some MedicineName and DemandCount");
            }
            try
            {
                if (demand.DemandCount > 0)
                {
                    string token;
                    MedicineSupply medicinesupply;
                    if (string.IsNullOrEmpty(HttpContext.Request.Headers["Authorization"]))
                    {
                        token = "";
                    }
                    else
                     token = HttpContext.Request.Headers["Authorization"].FirstOrDefault().Split(" ")[1];
                     medicinesupply = _providercontext.GetSupply(demand, token);
                    if (medicinesupply != null)
                    {
                        _log4net.Info("Medicine successfully accessed from Medicine Stock from " + nameof(MedicineSupplyController));
                        return Ok(medicinesupply);
                    }
                    else
                    {
                        _log4net.Error("MedicineName not found in MedicineStock " + nameof(MedicineSupplyController));
                        return NotFound("Your entered medicine name does not exist in our MedicineStock");
                    } 
                }
                else 
                    {
                        _log4net.Error("DemandCount is 0 or negative from " + nameof(MedicineSupplyController));
                        return BadRequest("Invalid DemandCount");
                    }  
            }
            catch(Exception)
            {
                    _log4net.Error("Internal server error " + nameof(MedicineSupplyController));
                    return StatusCode(500);
            }
        }
    }
}
