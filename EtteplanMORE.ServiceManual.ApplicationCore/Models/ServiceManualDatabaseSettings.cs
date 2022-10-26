namespace EtteplanMORE.ServiceManual.ApplicationCore.Models
{
    public class ServiceManualDatabaseSettings
    {
        public string ConnectionString { get; set; } = null;

        public string DatabaseName { get; set; } = null;

        public string FactoryDevicesCollectionName { get; set; } = null;

        public string MaintenanceTasksCollectionName { get; set; } = null;
    }
}
