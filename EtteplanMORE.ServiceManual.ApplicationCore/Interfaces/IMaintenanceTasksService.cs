using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using EtteplanMORE.ServiceManual.ApplicationCore.Entities;

namespace EtteplanMORE.ServiceManual.ApplicationCore.Interfaces
{
    public interface IMaintenanceTasksService
    {
        /// <summary>
        ///Implement to return all maintenance tasks (order by severity, registration time)
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<MaintenanceTask>> GetAll();

        /// <summary>
        ///Implement to return all maintenance tasks filtered by given id (order by severity, registration time)
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<MaintenanceTask>> Get(int id);

        Task CreateNewTask(int deviceId, string description, Severity severity);
    }
}