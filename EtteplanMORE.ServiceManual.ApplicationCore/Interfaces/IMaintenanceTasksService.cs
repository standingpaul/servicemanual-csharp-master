﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using EtteplanMORE.ServiceManual.ApplicationCore.Entities;

namespace EtteplanMORE.ServiceManual.ApplicationCore.Interfaces
{
    public interface IMaintenanceTasksService
    {
        // MAINTENANCE TASKS

        /// <summary>
        /// Implement method to return all maintenance tasks (order by severity, registration time)
        /// </summary>
        /// <returns>Returns all maintenance tasks in the database</returns>
        Task<IEnumerable<MaintenanceTask>> GetAll();

        /// <summary>
        /// Implement method to return a single maintenance task by the given maintenance task id
        /// </summary>
        /// <param name="taskId"></param>
        /// <returns>Returns the maintenance task if found or null</returns>
        Task<MaintenanceTask> GetMaintenanceTaskByTaskId(string taskId);

        /// <summary>
        /// Implement method to return all maintenance tasks filtered by given device id (order by severity, registration time)
        /// </summary>
        /// <returns>List of the maintenance tasks which are attached the the given device id</returns>
        Task<IEnumerable<MaintenanceTask>> GetMaintenanceTasksByDeviceId(int deviceId);

        /// <summary>
        /// Implement method to create a new maintenance task
        /// </summary>
        /// <param name="deviceId">deviceId</param>
        /// <param name="description"></param>
        /// <param name="severity"></param>
        /// <returns>The new task, or null if not inserted</returns>
        MaintenanceTask CreateNewTask(string deviceId, string description, Severity severity);

        /// <summary>
        /// Implement method to modify an already existing task
        /// </summary>
        /// <param name="maintenanceTaskId"></param>
        /// <param name="deviceId"></param>
        /// <param name="description"></param>
        /// <param name="severity"></param>
        /// <param name="maintenanceTaskStatus"></param>
        /// <returns>The current maintenance task in the database (hopefully modified)</returns>
        MaintenanceTask ModifyMaintenanceTask(string maintenanceTaskId, string deviceId, string description, Severity severity, MaintenanceTaskStatus maintenanceTaskStatus);

        /// <summary>
        /// Implement method to delete a single maintenance task by its id
        /// </summary>
        /// <param name="maintenanceTaskId"></param>
        /// <returns></returns>
        Task DeleteMaintenanceTask(string maintenanceTaskId);

        // DEVICES

        /// <summary>
        /// Implement method to return all devices(order by id)
        /// </summary>
        /// <returns>List of all devices in the database</returns>
        Task<IEnumerable<MaintenanceTask>> GetAllDevices();
    }
}