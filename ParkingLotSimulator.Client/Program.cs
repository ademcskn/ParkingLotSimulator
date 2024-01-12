using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using ParkingLotSimulator.Business;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Linq;
using System.Security.Cryptography;

namespace ParkingLotSimulator.Client
{
    class Program
    {
        static void Main(string[] args)
        {
            string[] validValues = new string[3] { "1", "2", "3" };
            string parkingCapacity = ConfigurationManager.AppSettings.Get("ParkingLotCapacity");
            ReadOnlyCollection<Vehicle> parkingCarList = VehicleRepository.Current.RetrieveAll();
            int occupancy = parkingCarList.Count();
            Console.WriteLine("Otoparka Hoşgeldiniz!!!\nLütfen yapmak istediğiniz işlem numarasını tuşlayın : ");
            Console.WriteLine("\t1 - Otopark kapasitesi - Otoparktaki araçlar");
            Console.WriteLine("\t2 - Araç otopark ücreti ve Çıkış");
            Console.WriteLine("\t3 - Araç girişi");
            var clientInputValue = Console.ReadLine();


            if (validValues.Contains(clientInputValue))
            {
                switch (clientInputValue)
                {
                    case "1":
                        Console.WriteLine("Otopark doluluk oranı : " + occupancy + "/" + parkingCapacity);
                        Console.WriteLine("Otoparktaki Araçların Listesi : ");
                        foreach (var carInfo in parkingCarList)
                        {
                            Console.WriteLine("Plaka : " + carInfo.LicencePlate + " , Giriş Saati : " + carInfo.TicketIssueDate);
                        }
                        break;
                    case "2":
                        Console.WriteLine("Lütfen aracın plakasını girin : ");
                        clientInputValue = Console.ReadLine();
                        int vehicleParkingMoney = VehicleParkingMoney(clientInputValue);

                        Console.WriteLine(clientInputValue + " Plakalı araç otopark fiyatı : " + vehicleParkingMoney + "TL");
                        Console.WriteLine("Aracın çıkışı yapılsın mı? (E/H)");
                        if (Console.ReadLine() == "E")
                        {
                            VehicleRepository.RemoveVehicleToParking(VehicleRepository.Current.RetrieveById(clientInputValue));
                            Console.WriteLine("Aracın çıkışı yapıldı");
                        }
                        else
                        {
                            Console.WriteLine("Çıkış yapılmadı.");
                        }
                        break;
                    case "3":
                        if (Convert.ToInt32(parkingCapacity) <= occupancy)
                        {
                            Console.WriteLine("Otopark şu an dolu!!!");
                        }
                        else
                        {
                            Console.WriteLine("Lütfen giriş yapacak aracın plaka bilgisini 7 karakter olacak şekilde giriniz : ");
                            string inputCarLicencePlate = Console.ReadLine();
                            int sayi = inputCarLicencePlate.ToCharArray().Length;
                            if (inputCarLicencePlate.ToCharArray().Length != 7)
                            {
                                Console.WriteLine("lütfen 7 karakter olarak giriş yapın");
                            }
                            else
                            {
                                var newVehicle = new Vehicle();
                                newVehicle.LicencePlate = inputCarLicencePlate;
                                newVehicle.TicketIssueDate = DateTime.Now;
                                VehicleRepository.AddVehicleToParking(newVehicle);

                                Console.WriteLine("Araç Eklendi");
                            }
                        }
                        break;
                    default:
                        Console.WriteLine("Yanlış değer");
                        break;
                }
            }

            Console.ReadLine();
        }

        public static int VehicleParkingMoney(string LicencePlate)
        {
            Vehicle vehicle = VehicleRepository.Current.RetrieveById(LicencePlate);
            TimeSpan span = DateTime.Now.Subtract(vehicle.TicketIssueDate);
            int vehicleMinutes = Convert.ToInt32(String.Format("{0:0}", span.TotalMinutes));
            int vehicleHours = Convert.ToInt32(String.Format("{0:0}", span.TotalHours));
            if (vehicleMinutes <= 180)
            {
                return (vehicleHours + 1) * 3;
            }
            else if (vehicleMinutes > 180 && vehicleMinutes < 360)
            {
                return 9 + ((vehicleHours - 2) * 2);
            }
            else
            {
                int extraHour = (vehicleHours - 6) / 3;
                return 15 + ((extraHour + 1) * 5);
            }
        }
    }
}