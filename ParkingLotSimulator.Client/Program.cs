using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using ParkingLotSimulator.Business;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Threading;
using static System.Net.Mime.MediaTypeNames;

namespace ParkingLotSimulator.Client
{
    class Program
    {
        private static int occupancy;
        private static string clientInputValue;
        private static string parkingCapacity = ConfigurationManager.AppSettings.Get("ParkingLotCapacity");
        private static ReadOnlyCollection<Vehicle> parkingCarList = VehicleRepository.Current.RetrieveAll();
        private static string[] validValues = new string[4] { "1", "2", "3", "4" };
        static void Main(string[] args)
        {
            Console.WriteLine("~~~~~~~~~~~~OTOPARKA HOŞGELDİNİZ!!!~~~~~~~~~~~~");
            UserOperations();
            Console.ReadLine();
        }
        public static void UserOperations()
        {
            do
            {
                Console.WriteLine("Lütfen yapmak istediğiniz işlem numarasını tuşlayın: ");
                Console.WriteLine("\t1 - Otopark kapasitesi - Otoparktaki araçlar");
                Console.WriteLine("\t2 - Araç otopark ücreti ve Çıkış");
                Console.WriteLine("\t3 - Araç girişi");
                Console.WriteLine("\t4 - Çıkış");
                clientInputValue = Console.ReadLine();
                if (validValues.Contains(clientInputValue))
                {
                    switch (clientInputValue)
                    {
                        case "1":
                            occupancy = parkingCarList.Count();
                            Console.WriteLine("Otopark doluluk oranı : " + occupancy + "/" + parkingCapacity);
                            Console.WriteLine("Otoparktaki Araçların Listesi : ");
                            foreach (var carInfo in parkingCarList)
                            {
                                Console.WriteLine("Plaka : " + carInfo.LicencePlate + " , Giriş Saati : " + carInfo.TicketIssueDate);
                            }
                            ProcessCompleted();
                            UserOperations();
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
                            ProcessCompleted();
                            UserOperations();
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
                            ProcessCompleted();
                            UserOperations();
                            break;
                        case "4":
                            Environment.Exit(0);
                            break;
                        default:
                            Console.WriteLine("Yanlış değer");
                            break;
                    }
                }
                else
                {
                    Console.WriteLine("Lütfen sadece programdaki işlemleri seçin!");
                    Console.Write("Yapmak istediğiniz işlemi seçin : ");
                }
            } while (!validValues.Contains(clientInputValue));
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

        static void ProcessCompleted()
        {
            Console.WriteLine();
            Console.WriteLine("-----------*-----------*------ İşlem Tamamlandı -----------*-----------*------");
            Console.WriteLine();
        }
    }
}