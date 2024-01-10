using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Text;

namespace ParkingLotSimulator.Business
{
    internal class VehicleDataParser
    {
        private readonly string vehicleDataPath = ConfigurationManager.AppSettings.Get("VehicleListFilePath");
        private IEnumerable<Vehicle> vehicleList;

        internal IEnumerable<Vehicle> VehicleList
        {
            get
            {
                return this.vehicleList;
            }
            set
            {
                vehicleList = value;
            }
        }

        internal VehicleDataParser()
        {
            VehicleList = LoadVehicleList();
        }

        private IEnumerable<Vehicle> LoadVehicleList()
        {
            using (StreamReader file = File.OpenText(vehicleDataPath))
            {
                JsonSerializer serializer = new JsonSerializer();
                IEnumerable<Vehicle> list = (IEnumerable<Vehicle>)serializer.Deserialize(file, typeof(IEnumerable<Vehicle>));
                return list;
            }
        }
    }
}

