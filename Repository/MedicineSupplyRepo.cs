using Microsoft.AspNetCore.Mvc;
using PharmacyMedicineSupplyMicroservice.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Runtime.CompilerServices;
using log4net;
using System.Net.Http.Headers;
using Microsoft.Extensions.Configuration;

namespace PharmacyMedicineSupplyMicroservice.Repository
{
    public class MedicineSupplyRepo:IMedicineSupply
    {
        readonly log4net.ILog _log4net;
        IConfiguration config;
        public static List<string> pharmacies = new List<string> {
            "Pharmacy1", "Pharmacy2","Pharmacy3","Pharmacy4","Pharmacy5",
            "Pharmacy6", "Pharmacy7","Pharmacy8","Pharmacy9","Pharmacy10",
            "Pharmacy11", "Pharmacy12","Pharmacy13","Pharmacy14","Pharmacy15",
            "Pharmacy16", "Pharmacy17","Pharmacy18","Pharmacy19","Pharmacy20"};
        Uri baseaddress;
        HttpClient client;
        public MedicineSupplyRepo(IConfiguration config)
        {
            baseaddress = new Uri(config["Links:MedicineStock"]);
            client = new HttpClient();
            client.BaseAddress = baseaddress;
            this.config = config;
            _log4net = log4net.LogManager.GetLogger(typeof(MedicineSupplyRepo));
        }
        /// <summary>
        /// In this method we fetch the entire medicinestock from a externl microservice
        /// </summary>
        /// <returns>MedicineStock</returns>
        public IEnumerable<MedicineStock> GetSupply(string token)
        {
            try
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                HttpResponseMessage response = client.GetAsync(client.BaseAddress).Result;
                if (response.IsSuccessStatusCode)
                {
                    List<MedicineStock> medicinestock = new List<MedicineStock>();
                    string data = response.Content.ReadAsStringAsync().Result;
                    medicinestock = JsonConvert.DeserializeObject<List<MedicineStock>>(data);
                    return medicinestock;
                }
                _log4net.Info("Reponse Code is "+ response.StatusCode +" from "+ nameof(MedicineSupplyRepo));
                return null;
            }
            catch(Exception e)
            {
                _log4net.Error("Internal Server Error"+ " from "+nameof(MedicineSupplyRepo));
                throw e;
            }
        }
    }
}
