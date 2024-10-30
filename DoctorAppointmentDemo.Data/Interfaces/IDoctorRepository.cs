using MyDoctorAppointment.Domain.Entities;


namespace MyDoctorAppointment.Data.Interfaces
{
    public interface IDoctorRepository : IGenericRepository<Doctor>
    {
        // you can add more specific doctor's methods
        public string Path { get; set; }

        public int LastId { get; set; }
    }
}
