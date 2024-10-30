using MyDoctorAppointment.Domain.Enums;
using Newtonsoft.Json;

namespace MyDoctorAppointment.Data.Configuration;

public class AppSettings
{
    public RepositorySettings Doctors { get; set; } 
    public RepositorySettings Patients { get; set; } 
    public RepositorySettings Appointments { get; set; } 
    public SaveFileTypes SaveFileType { get; set; }

    public static AppSettings ReadFromFile() => JsonConvert.DeserializeObject<AppSettings>(File.ReadAllText(Constants.AppSettingsPath))!;

    public static void SaveAppSettings(AppSettings appSettings)
    {
        File.WriteAllText(Constants.AppSettingsPath, JsonConvert.SerializeObject(appSettings, Formatting.Indented));
    }
}

public class RepositorySettings
{
    public string JsonSaveFilePath { get; set; }
    public string XmlSaveFilePath { get; set; }
    public int LastId { get; set; }
}