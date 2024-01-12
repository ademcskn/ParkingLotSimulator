using System;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.IO;
using System.Linq;

namespace ParkingLotSimulator.Business
{
    public sealed class VehicleRepository : IRepository<Vehicle, string>
    {
        private static readonly object padLock = new object();
        private static VehicleRepository current = null;

        /// <summary>
        /// Public singleton readonly instance.
        /// </summary>
        public static VehicleRepository Current
        {
            get
            {
                if (current == null)
                {
                    lock (padLock)
                    {
                        if (current == null)
                        {
                            current = new VehicleRepository();
                        }
                    }
                }
                return current;
            }
        }

        private IEnumerable<Vehicle> vehicleList { get; set; }

        private VehicleRepository()
        {
            vehicleList = new VehicleDataParser().VehicleList;
        }

        public Vehicle RetrieveById(string licencePlate)
        {
            return vehicleList.First(vehicle => vehicle.LicencePlate == licencePlate);
        }

        public ReadOnlyCollection<Vehicle> RetrieveAll()
        {
            return vehicleList.ToList().AsReadOnly();
        }

        public static void AddVehicleToParking(Vehicle vehicle)
        {
            List<Vehicle> listVehicles = new List<Vehicle>();
            foreach (var vehicleItem in new VehicleDataParser().VehicleList)
            {
                listVehicles.Add(vehicleItem);
            }

            listVehicles.Add(vehicle);
            var updatedJsonString = JsonConvert.SerializeObject(listVehicles);
            File.WriteAllText(ConfigurationManager.AppSettings.Get("VehicleListFilePath"), updatedJsonString);
        }
        public static void RemoveVehicleToParking(Vehicle vehicle)
        {
            List<Vehicle> listVehicles = new List<Vehicle>();
            foreach (var vehicleItem in new VehicleDataParser().VehicleList)
            {
                listVehicles.Add(vehicleItem);
            }

            listVehicles.Remove(listVehicles.First(v => v.LicencePlate == vehicle.LicencePlate));
            var updatedJsonString = JsonConvert.SerializeObject(listVehicles);
            File.WriteAllText(ConfigurationManager.AppSettings.Get("VehicleListFilePath"), updatedJsonString);
        }
    }
}
