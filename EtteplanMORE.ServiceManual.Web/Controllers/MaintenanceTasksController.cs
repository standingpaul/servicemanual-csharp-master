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

        /// <summary>
        /// HTTP GET: /ServiceManual/AllMaintenanceTasks
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
        /// <returns>A single task from given task id or null if not found</returns>
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
        /// HTTP PUT: ServiceManual/CreateMaintenanceTask
        /// </summary>
        /// <param name="maintenanceTask"></param>         
        /// <returns>The new task, or null if not inserted</returns>
        [HttpPut("CreateMaintenanceTask")]
        public async Task<MaintenanceTask> CreateNewMaintenanceTask([FromBody] MaintenanceTask maintenanceTask)
        {
            return await maintenanceTasksService.CreateNewMaintenanceTask(maintenanceTask.DeviceId, maintenanceTask.Description, maintenanceTask.Severity);
        }

        /// <summary>
        /// HTTP PUT: ServiceManual/ModifyMaintenanceTask/{TaskId}
        /// </summary>
        /// <param name="maintenanceTask"></param>       
        /// <returns>boolean to inform if update was successful</returns>
        [HttpPut("ModifyMaintenanceTask/{maintenanceTaskId}")]
        public async Task<bool> ModifyMaintenanceTask([FromBody] MaintenanceTask maintenanceTask, string maintenanceTaskId)
        {
            return await maintenanceTasksService.ModifyMaintenanceTask(
                maintenanceTaskId,
                maintenanceTask.DeviceId,
                maintenanceTask.Description,
                maintenanceTask.Severity,
                maintenanceTask.TaskStatus
                );
        }

        /// <summary>
        /// HTTP DELETE: ServiceManual/DeleteMaintenanceTask/{maintenanceTaskId}
        /// </summary>
        /// <param name="maintenanceTaskId"></param>
        /// <returns>boolean to inform if the task was deleted</returns>
        [HttpDelete("DeleteMaintenanceTask/{maintenanceTaskId}")]
        public async Task<bool> DeleteMaintenanceTask(string maintenanceTaskId)
        {
            return await maintenanceTasksService.DeleteMaintenanceTask(maintenanceTaskId);
        }

        /// <summary>
        /// HTTP GET: ServiceManual/AllDevices
        /// </summary>
        /// <returns>List of all devices in the database</returns>
        [HttpGet("AllDevices")]
        public async Task<IEnumerable<Device>> GetAllDevices()
        {
            return await maintenanceTasksService.GetAllDevices();
        }

        /// <summary>
        /// HTTP GET: ServiceManual/SingleDeviceById/{deviceId}
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
