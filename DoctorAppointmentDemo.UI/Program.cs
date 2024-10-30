using MyDoctorAppointment.Data.Configuration;
using MyDoctorAppointment.Domain.Entities;
using MyDoctorAppointment.Domain.Enums;
using MyDoctorAppointment.Service.Interfaces;
using MyDoctorAppointment.Service.Services;

namespace MyDoctorAppointment
{
    public class DoctorAppointment
    {
        private readonly IDoctorService _doctorService;
        private readonly AppSettings _appSettings = AppSettings.ReadFromFile();

        public DoctorAppointment()
        {
            _doctorService = new DoctorService();
        }

        public void Menu()
        {
            while (true)
            {
                Console.WriteLine($"DoctorAppointment app.\nCurrent DB type: {AppSettings.ReadFromFile().SaveFileType}\nSelect an action:");
                var input = Console.ReadKey();
                Console.WriteLine();
                switch (input.Key)
                {
                    default:
                    case ConsoleKey.L:
                        Console.WriteLine("Current doctors list: ");
                        var docs = _doctorService.GetAll();

                        foreach (var doc in docs)
                        {
                            Console.WriteLine(doc.Name);
                        }

                        break;
                    case ConsoleKey.Q:
                        Console.WriteLine("Exiting. Bye.");
                        return;
                    case ConsoleKey.A:
                        Console.WriteLine("Adding doctor: ");

                        Console.Write("Name: ");
                        var name = Console.ReadLine();
                        Console.Write("Surname: ");
                        var surname = Console.ReadLine();
                        Console.Write("Experience: ");
                        var experienceParsed = Byte.TryParse(Console.ReadLine(), out byte experience);
                        Console.WriteLine("Select Doctor Type: \n0 for FamilyDoctor\n1 for Dentist\n2 for Dermatologist\n3 for Paramedic");
                        Byte.TryParse(Console.ReadLine(), out byte doctorType);
                        
                        var newDoctor = new Doctor
                        {
                            Name = name,
                            Surname = surname,
                            Experience = experience,
                            DoctorType = (Domain.Enums.DoctorTypes)doctorType
                        };
                        _doctorService.Create(newDoctor);
                        break;
                    case ConsoleKey.D:
                        Console.WriteLine("Deleting doctor");
                        Console.Write("Id to delete: ");
                        var id = int.Parse(Console.ReadLine());
                        _doctorService.Delete(id);
                        break;
                    case ConsoleKey.X:
                        _doctorService.SwitchDbType(SaveFileTypes.Xml);
                        break;
                    case ConsoleKey.J:
                        _doctorService.SwitchDbType(SaveFileTypes.Json);
                        break;
                }
            }
        }
    }

    public static class Program
    {
        public static void Main()
        {
            var doctorAppointment = new DoctorAppointment();
            doctorAppointment.Menu();
        }
    }
}