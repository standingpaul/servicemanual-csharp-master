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
        // The collections in the database
        private readonly IMongoCollection<Device> factoryDeviceCollection;
        private readonly IMongoCollection<MaintenanceTask> maintenanceTasksCollection;

        // Constructor
        public MaintenanceTasksService(IOptions<ServiceManualDatabaseSettings> serviceManualDatabaseSettings)
        {
            Console.WriteLine(serviceManualDatabaseSettings.Value.ConnectionString);
            var mongoClient = new MongoClient(
                serviceManualDatabaseSettings.Value.ConnectionString);

            var mongoDatabase = mongoClient.GetDatabase(
                serviceManualDatabaseSettings.Value.DatabaseName);



            factoryDeviceCollection = mongoDatabase.GetCollection<Device>(serviceManualDatabaseSettings.Value.FactoryDevicesCollectionName);
            maintenanceTasksCollection = mongoDatabase.GetCollection<MaintenanceTask>(serviceManualDatabaseSettings.Value.MaintenanceTasksCollectionName);

            CreateDeviceIdIndexInMaintenanceTasksTable();
        }

        /// <summary>
        /// Creates an index by device id in the database
        /// </summary>
        public async void CreateDeviceIdIndexInMaintenanceTasksTable()
        {
            // Create index by device id on the tasks table  
            await maintenanceTasksCollection.Indexes.CreateOneAsync
            (
                new CreateIndexModel<MaintenanceTask>(Builders<MaintenanceTask>.IndexKeys.Ascending(task => task.DeviceId))
            );

            Console.WriteLine($"<color=green>Index should have been created</color>");
        }

        // MAINTENANCE TASKS

        /// <summary>
        /// Method returns all maintenance tasks (order by severity, registration time)
        /// </summary>
        /// <returns>Returns all maintenance tasks in the database</returns>
        public async Task<IEnumerable<MaintenanceTask>> GetAll()
        {
            return await maintenanceTasksCollection.Find(MaintenanceTask => true).SortByDescending(MaintenanceTask => MaintenanceTask.Severity).ThenBy(MaintenanceTask => MaintenanceTask.RegistrationTime).ToListAsync();
        }

        /// <summary>
        /// Method returns a single maintenance task by the given maintenance task id
        /// </summary>
        /// <param name="taskId"></param>
        /// <returns>Returns the maintenance task if found or null</returns>       
        public Task<MaintenanceTask> GetMaintenanceTaskByTaskId(string taskId)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Implement method to return all maintenance tasks filtered by given device id (order by severity, registration time)
        /// </summary>
        /// <param name="deviceId"></param>
        /// <returns>List of the maintenance tasks which are attached to the given device id</returns>
        public Task<IEnumerable<MaintenanceTask>> GetMaintenanceTasksByDeviceId(int deviceId)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Implement method to create a new maintenance task
        /// </summary>
        /// <param name="deviceId">deviceId</param>
        /// <param name="description"></param>
        /// <param name="severity"></param>
        /// <returns>The new task, or null if not inserted</returns>
        public async Task<MaintenanceTask> CreateNewTask(string deviceId, string description, Severity severity)
        {
            MaintenanceTask newTask = new(deviceId, description, severity, MaintenanceTaskStatus.open);

            await maintenanceTasksCollection.InsertOneAsync(newTask);

            return await GetMaintenanceTaskByTaskId(newTask.TaskId);
        }

        /// <summary>
        /// Implement method to modify an already existing task
        /// </summary>
        /// <param name="maintenanceTaskId"></param>
        /// <param name="deviceId"></param>
        /// <param name="description"></param>
        /// <param name="severity"></param>
        /// <param name="maintenanceTaskStatus"></param>
        /// <returns>The current maintenance task in the database (hopefully modified)</returns>
        public MaintenanceTask ModifyMaintenanceTask(string maintenanceTaskId, string deviceId, string description, Severity severity, MaintenanceTaskStatus maintenanceTaskStatus)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Implement method to delete a single maintenance task by its id
        /// </summary>
        /// <param name="maintenanceTaskId"></param>
        /// <returns></returns>
        public Task DeleteMaintenanceTask(string maintenanceTaskId)
        {
            throw new NotImplementedException();
        }

        // DEVICES

        /// <summary>
        /// Implement method to return all devices(order by id)
        /// </summary>
        /// <returns>List of all devices in the database</returns>
        public Task<IEnumerable<MaintenanceTask>> GetAllDevices()
        {
            throw new NotImplementedException();
        }
    }
}