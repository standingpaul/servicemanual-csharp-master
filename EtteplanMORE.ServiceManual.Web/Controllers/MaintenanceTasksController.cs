using System;
using System.Collections.Generic;
using EtteplanMORE.ServiceManual.ApplicationCore.Entities;
using EtteplanMORE.ServiceManual.ApplicationCore.Interfaces;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;

namespace EtteplanMORE.ServiceManual.Web.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ServiceManualController : Controller
    {
        private readonly IMaintenanceTasksService maintenanceTasksService;

        public ServiceManualController(IMaintenanceTasksService factoryMaintenanceTasksService)
        {
            maintenanceTasksService = factoryMaintenanceTasksService;
        }

        // MAINTENANCE TASKS

        /// <summary>
        /// HTTP GET: ServiceManual/AllTasks
        /// </summary>
        /// <returns>List of all maintenance tasks ordered by severity then registration time</returns>
        [HttpGet("AllMaintenanceTasks")]
        public async Task<IEnumerable<MaintenanceTask>> GetAllMaintenanceTasks()
        {
            return await maintenanceTasksService.GetAllMaintenanceTasks();
        }

        /// <summary>
        /// HTTP GET: ServiceManual/SingleMaintenanceTaskByTaskId/{maintenanceTaskId}
        /// </summary>
        /// <param name="maintenanceTaskId"></param>
        /// <returns>A single task frmo given task id or null if not found</returns>
        [HttpGet("SingleMaintenanceTaskByTaskId/{maintenanceTaskId}")]
        public async Task<MaintenanceTask> GetSingleMaintenanceTaskByTaskId(string maintenanceTaskId)
        {
            return await maintenanceTasksService.GetSingleMaintenanceTaskByTaskId(maintenanceTaskId);
        }

        /// <summary>
        /// HTTP GET: ServiceManual/MaintenanceTasksByDeviceId/{deviceId}
        /// </summary>
        /// <param name="deviceId"></param>
        /// <returns>List of maintenance tasks filtered by the given device id, ordered by severity then registration time</returns>
        [HttpGet("MaintenanceTasksByDeviceId/{deviceId}")]
        public async Task<IEnumerable<MaintenanceTask>> GetMaintenanceTasksByDeviceId(string deviceId)
        {
            return await maintenanceTasksService.GetMaintenanceTasksByDeviceId(deviceId);
        }

        /// <summary>
        /// HTTP PUT: ServiceManual/CreateMaintenanceTask/{deviceId}/{description}/{severity}
        /// </summary>
        /// <param name="deviceId"></param>
        /// <param name="description"></param>
        /// <param name="severity"></param>    
        /// <returns>The new task, or null if not inserted</returns>
        [HttpPut("CreateMaintenanceTask/{deviceId}/{description}/{severity}")]
        public async Task<MaintenanceTask> CreateNewMaintenanceTask(string deviceId, string description, Severity severity)
        {
            return await maintenanceTasksService.CreateNewMaintenanceTask(deviceId, description, severity);
        }

        /// <summary>
        /// ServiceManual/ModifyMaintenanceTask/{existingMaintenanceTaskId}/{deviceId}/{description}/{severity}/{maintenanceTaskStatus}
        /// </summary>
        /// <param name="maintenanceTaskId"></param>
        /// <param name="deviceId"></param>
        /// <param name="description"></param>
        /// <param name="severity"></param>
        /// <param name="maintenanceTaskStatus"></param>
        /// <returns>boolean to inform if update was successful</returns>
        [HttpPut("ModifyMaintenanceTask/{existingMaintenanceTaskId}/{deviceId}/{description}/{severity}/{maintenanceTaskStatus}")]
        public async Task<bool> ModifyMaintenanceTask(string existingMaintenanceTaskId, string deviceId, string description, Severity severity, MaintenanceTaskStatus maintenanceTaskStatus)
        {
            return await maintenanceTasksService.ModifyMaintenanceTask(
                existingMaintenanceTaskId,
                deviceId,
                description,
                severity,
                maintenanceTaskStatus
                );
        }

        /// <summary>
        /// Method to delete a single maintenance task by its id
        /// </summary>
        /// <param name="maintenanceTaskId"></param>
        /// <returns>boolean to inform if the task was deleted</returns>
        [HttpPut("DeleteMaintenanceTask/{maintenanceTaskId}")]
        public async Task<bool> DeleteMaintenanceTask(string maintenanceTaskId)
        {
            return await maintenanceTasksService.DeleteMaintenanceTask(maintenanceTaskId);
        }

        // DEVICES

        /// <summary>
        /// ServiceManual/AllDevices
        /// </summary>
        /// <returns>List of all devices in the database</returns>
        [HttpGet("AllDevices")]
        public async Task<IEnumerable<Device>> GetAllDevices()
        {
            return await maintenanceTasksService.GetAllDevices();
        }

        /// <summary>
        /// ServiceManual/SingleDeviceById/{deviceId}
        /// </summary>
        /// <param name="deviceId"></param>
        /// <returns>The device or null if device was not found</returns>
        [HttpGet("SingleDeviceById/{deviceId}")]
        public async Task<Device> GetSingleDeviceById(string deviceId)
        {
            return await maintenanceTasksService.GetSingleDeviceById(deviceId);
        }
    }
}
