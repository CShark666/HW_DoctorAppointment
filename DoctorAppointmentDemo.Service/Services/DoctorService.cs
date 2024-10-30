using MyDoctorAppointment.Data.Configuration;
using MyDoctorAppointment.Data.Interfaces;
using MyDoctorAppointment.Data.Repositories;
using MyDoctorAppointment.Domain.Entities;
using MyDoctorAppointment.Domain.Enums;
using MyDoctorAppointment.Service.Interfaces;

namespace MyDoctorAppointment.Service.Services
{
    public class DoctorService : IDoctorService
    {
        private readonly IDoctorRepository _doctorRepository;
        private readonly AppSettings _appSettings = AppSettings.ReadFromFile();

        public DoctorService()
        {
            _doctorRepository = new DoctorRepository();
        }

        public Doctor Create(Doctor doctor)
        {
            return _doctorRepository.Create(doctor);
        }

        public bool Delete(int id)
        {
            return _doctorRepository.Delete(id);
        }

        public Doctor? Get(int id)
        {
            return _doctorRepository.GetById(id);
        }

        public IEnumerable<Doctor> GetAll()
        {
            return _doctorRepository.GetAll();
        }

        public Doctor Update(int id, Doctor doctor)
        {
            return _doctorRepository.Update(id, doctor);
        }

        public void SwitchDbType(SaveFileTypes newSaveFileType)
        {
            _appSettings.SaveFileType = newSaveFileType;
            AppSettings.SaveAppSettings(_appSettings);
            
            var result = AppSettings.ReadFromFile();

            _doctorRepository.Path = result.SaveFileType == SaveFileTypes.Json ? result.Doctors.JsonSaveFilePath : result.Doctors.XmlSaveFilePath;
            _doctorRepository.LastId = result.Doctors.LastId;
        }
    }
}
