using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EtteplanMORE.ServiceManual.ApplicationCore.Entities;
using EtteplanMORE.ServiceManual.ApplicationCore.Interfaces;
using EtteplanMORE.ServiceManual.ApplicationCore.Models;
using EtteplanMORE.ServiceManual.ApplicationCore.Utility;
using Microsoft.Extensions.Options;
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
            //Console.WriteLine(serviceManualDatabaseSettings.Value.ConnectionString);
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
        }

        // MAINTENANCE TASKS

        /// <summary>
        /// Method returns all maintenance tasks (order by severity, registration time)
        /// </summary>
        /// <returns>Returns all maintenance tasks in the database</returns>
        public async Task<IEnumerable<MaintenanceTask>> GetAllMaintenanceTasks()
        {
            return await maintenanceTasksCollection.Find(MaintenanceTask => true).SortBy(MaintenanceTask => MaintenanceTask.Severity).ThenBy(MaintenanceTask => MaintenanceTask.RegistrationTime).ToListAsync();
        }

        /// <summary>
        /// Method returns a single maintenance task by the given maintenance task id
        /// </summary>
        /// <param name="maintenanceTaskId"></param>
        /// <returns>Returns the maintenance task if found or null</returns>       
        public async Task<MaintenanceTask> GetSingleMaintenanceTaskByTaskId(string maintenanceTaskId)
        {
            try
            {
                return await maintenanceTasksCollection.Find(maintenanceTask => maintenanceTask.TaskId == maintenanceTaskId).FirstOrDefaultAsync();
            }
            catch (Exception e)
            {
                return null;
            }
        }

        /// <summary>
        /// Method to return all maintenance tasks filtered by given device id (order by severity, registration time)
        /// </summary>
        /// <param name="deviceId"></param>
        /// <returns>List of the maintenance tasks which are attached to the given device id</returns>
        public async Task<IEnumerable<MaintenanceTask>> GetMaintenanceTasksByDeviceId(string deviceId)
        {
            return await maintenanceTasksCollection.Find(MaintenanceTask => MaintenanceTask.DeviceId == deviceId).SortByDescending(MaintenanceTask => MaintenanceTask.Severity).ThenBy(MaintenanceTask => MaintenanceTask.RegistrationTime).ToListAsync();
        }

        /// <summary>
        /// Method to create a new maintenance task
        /// </summary>
        /// <param name="deviceId"></param>
        /// <param name="description"></param>
        /// <param name="severity"></param>
        /// <returns>The new task, or null if not inserted</returns>
        public async Task<MaintenanceTask> CreateNewMaintenanceTask(string deviceId, string description, Severity severity)
        {
            // If the device does not exist, do not update and return false
            if (await GetSingleDeviceById(deviceId) == null)
            {
                return null;
            }

            MaintenanceTask newTask = new(deviceId, description, severity, MaintenanceTaskStatus.open);

            await maintenanceTasksCollection.InsertOneAsync(newTask);

            return await GetSingleMaintenanceTaskByTaskId(newTask.TaskId);
        }

        /// <summary>
        /// Method to modify an already existing task
        /// </summary>
        /// <param name="maintenanceTaskId"></param>
        /// <param name="deviceId"></param>
        /// <param name="description"></param>
        /// <param name="severity"></param>
        /// <param name="maintenanceTaskStatus"></param>
        /// <returns>boolean to inform if update was successful</returns>
        public async Task<bool> ModifyMaintenanceTask(string maintenanceTaskId, string deviceId, string description, Severity severity, MaintenanceTaskStatus maintenanceTaskStatus)
        {
            // If the device does not exist, do not update and return false
            if (await GetSingleDeviceById(deviceId) == null)
            {
                return false;
            }

            MaintenanceTask modifiedMaintenanceTask = await GetSingleMaintenanceTaskByTaskId(maintenanceTaskId);

            modifiedMaintenanceTask.DeviceId = deviceId;
            modifiedMaintenanceTask.Description = description;
            modifiedMaintenanceTask.Severity = severity;
            modifiedMaintenanceTask.TaskStatus = maintenanceTaskStatus;

            ReplaceOneResult result = await maintenanceTasksCollection.ReplaceOneAsync
            (
                maintenanceTask => maintenanceTask.TaskId == maintenanceTaskId,
                modifiedMaintenanceTask,
                new ReplaceOptions
                {
                    IsUpsert = true
                }
            );

            return ExtensionMethods.DatabaseResultWasPositive(result);
        }

        /// <summary>
        /// Method to delete a single maintenance task by its id
        /// </summary>
        /// <param name="maintenanceTaskId"></param>
        /// <returns>boolean to inform if the task was deleted</returns>
        public async Task<bool> DeleteMaintenanceTask(string maintenanceTaskId)
        {
            try
            {
                DeleteResult deleteResult = await maintenanceTasksCollection.DeleteOneAsync(maintenanceTask => maintenanceTask.TaskId == maintenanceTaskId);
                Console.WriteLine(deleteResult.DeletedCount.ToString());
                return deleteResult.DeletedCount >= 1;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        // DEVICES

        /// <summary>
        /// Method to return all devices(order by id)
        /// </summary>
        /// <returns>List of all devices in the database</returns>
        public async Task<IEnumerable<Device>> GetAllDevices()
        {
            return await factoryDeviceCollection.Find(device => true).SortBy(device => device.DeviceId).ToListAsync();
        }

        /// <summary>
        /// Get a single device by its id
        /// </summary>
        /// <param name="deviceId"></param>
        /// <returns>The device or null if device was not found</returns>
        public async Task<Device> GetSingleDeviceById(string deviceId)
        {
            try
            {
                return await factoryDeviceCollection.Find(device => device.DeviceId == deviceId).FirstOrDefaultAsync();
            }
            catch (Exception e)
            {
                return null;
            }
        }
    }
}