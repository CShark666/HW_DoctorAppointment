using System.Xml.Serialization;
using MyDoctorAppointment.Data.Configuration;
using MyDoctorAppointment.Data.Interfaces;
using MyDoctorAppointment.Domain.Entities;
using MyDoctorAppointment.Domain.Enums;
using Newtonsoft.Json;

namespace MyDoctorAppointment.Data.Repositories
{
    public abstract class GenericRepository<TSource> : IGenericRepository<TSource> where TSource : Auditable
    {
        public abstract string Path { get; set; }

        public abstract int LastId { get; set; }

        public TSource Create(TSource source)
        {
            var appSettings = AppSettings.ReadFromFile();
            source.Id = ++LastId;
            source.CreatedAt = DateTime.Now;

            if (appSettings.SaveFileType == SaveFileTypes.Json)
            {
                File.WriteAllText(Path, JsonConvert.SerializeObject(GetAll().Append(source), Formatting.Indented));
            }
            else
            {
                var arrSerializer = new XmlSerializer(typeof(TSource[]));
                using (var writer = new FileStream(Path, FileMode.OpenOrCreate))
                {
                    arrSerializer.Serialize(writer, GetAll().Append(source).ToArray());
                }
            }
            
            SaveLastId();

            return source;
        }

        public bool Delete(int id)
        {
            if (GetById(id) is null)
                return false;

            var appSettings = AppSettings.ReadFromFile();
            if (appSettings.SaveFileType == SaveFileTypes.Json)
            {
                File.WriteAllText(Path, JsonConvert.SerializeObject(GetAll().Where(x => x.Id != id), Formatting.Indented));
            }
            else
            {
                var arrSerializer = new XmlSerializer(typeof(TSource[]));
                using (var writer = new FileStream(Path, FileMode.OpenOrCreate))
                {
                    arrSerializer.Serialize(writer, GetAll().Where(x => x.Id != id).ToArray());
                }
            }

            return true;
        }

        public IEnumerable<TSource> GetAll()
        {
            var appSettings = AppSettings.ReadFromFile();
            if (appSettings.SaveFileType == SaveFileTypes.Json)
            {
                if (!File.Exists(Path))
                {
                    File.WriteAllText(Path, "[]");
                }

                var json = File.ReadAllText(Path);

                if (string.IsNullOrWhiteSpace(json))
                {
                    File.WriteAllText(Path, "[]");
                    json = "[]";
                }

                return JsonConvert.DeserializeObject<List<TSource>>(json)!;
            }

            var arrSerializer = new XmlSerializer(typeof(TSource[]));
            using (var fs = new FileStream(Path, FileMode.OpenOrCreate))
            {
                var res = (TSource[])arrSerializer.Deserialize(fs);
                if (res == null) return Enumerable.Empty<TSource>();
                return res;
            }
        }

        public TSource? GetById(int id)
        {
            return GetAll().FirstOrDefault(x => x.Id == id);
        }

        public TSource Update(int id, TSource source)
        {
            source.UpdatedAt = DateTime.Now;
            source.Id = id;
            var appSettings = AppSettings.ReadFromFile();
            if (appSettings.SaveFileType == SaveFileTypes.Json)
            {
                File.WriteAllText(Path, JsonConvert.SerializeObject(GetAll().Select(x => x.Id == id ? source : x), Formatting.Indented));
            }
            else
            {
                var arrSerializer = new XmlSerializer(typeof(TSource[]));
                using (var writer = new FileStream(Path, FileMode.OpenOrCreate))
                {
                    arrSerializer.Serialize(writer, GetAll().Select(x => x.Id == id ? source : x).ToArray());
                }
            }

            return source;
        }

        public abstract void ShowInfo(TSource source);

        protected abstract void SaveLastId();
    }
}
