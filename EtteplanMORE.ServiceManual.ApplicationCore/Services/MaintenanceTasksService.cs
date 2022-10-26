using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using EtteplanMORE.ServiceManual.ApplicationCore.Entities;
using EtteplanMORE.ServiceManual.ApplicationCore.Interfaces;
using EtteplanMORE.ServiceManual.ApplicationCore.Models;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;

namespace EtteplanMORE.ServiceManual.ApplicationCore.Services
{
    public class MaintenanceTasksService : IMaintenanceTasksService
    {
        private readonly IMongoCollection<MaintenanceTask> factoryDeviceCollection;
        private readonly IMongoCollection<MaintenanceTask> maintenanceTasksCollection;

        public MaintenanceTasksService(IOptions<ServiceManualDatabaseSettings> serviceManualDatabaseSettings)
        {
            Console.WriteLine(serviceManualDatabaseSettings.Value.ConnectionString);
            var mongoClient = new MongoClient(
                serviceManualDatabaseSettings.Value.ConnectionString);

            var mongoDatabase = mongoClient.GetDatabase(
                serviceManualDatabaseSettings.Value.DatabaseName);

            factoryDeviceCollection = mongoDatabase.GetCollection<MaintenanceTask>(serviceManualDatabaseSettings.Value.FactoryDevicesCollectionName);
            maintenanceTasksCollection = mongoDatabase.GetCollection<MaintenanceTask>(serviceManualDatabaseSettings.Value.MaintenanceTasksCollectionName);
        }

        public async Task<IEnumerable<MaintenanceTask>> GetAll()
        {
            return await maintenanceTasksCollection.Find(MaintenanceTask => true).SortByDescending(MaintenanceTask => MaintenanceTask.Severity).ThenBy(MaintenanceTask => MaintenanceTask.RegistrationTime).ToListAsync();
        }

        public async Task<IEnumerable<MaintenanceTask>> Get(int id)
        {
            return await maintenanceTasksCollection.Find(MaintenanceTask => MaintenanceTask.DeviceId == id).SortByDescending(MaintenanceTask => MaintenanceTask.Severity).ThenBy(MaintenanceTask => MaintenanceTask.RegistrationTime).ToListAsync();
        }

        public async Task CreateNewTask(int deviceId, string description, Severity severity)
        {
            MaintenanceTask newTask = new(deviceId, description, severity, MaintenanceTaskStatus.open);

            await maintenanceTasksCollection.InsertOneAsync(newTask);
        }
    }
}