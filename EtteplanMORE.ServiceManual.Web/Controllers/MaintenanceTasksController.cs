using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EtteplanMORE.ServiceManual.ApplicationCore.Entities;
using EtteplanMORE.ServiceManual.ApplicationCore.Interfaces;
using EtteplanMORE.ServiceManual.ApplicationCore.Services;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;

namespace EtteplanMORE.ServiceManual.Web.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MaintenanceTasksController : Controller
    {
        private readonly IMaintenanceTasksService maintenanceTaskService;

        public MaintenanceTasksController(IMaintenanceTasksService factoryMaintenanceTasksService)
        {
            maintenanceTaskService = factoryMaintenanceTasksService;
        }

        /// <summary>
        /// HTTP GET: api/maintenancetasks/
        /// </summary>
        /// <returns>List of all maintenance tasks ordered by severity then registration time</returns>
        [HttpGet]
        public async Task<IEnumerable<MaintenanceTask>> Get()
        {
            return await maintenanceTaskService.GetAll();

        }

        /// <summary>
        /// HTTP GET: api/maintenancetasks/{idOfDevice}
        /// </summary>
        /// <param name="id"></param>
        /// <returns>List of maintenance tasks filtered by the given device id, ordered by severity then registration time</returns>
        [HttpGet("{id}")]
        public async Task<IEnumerable<MaintenanceTask>> Get(int id)
        {
            return await maintenanceTaskService.GetMaintenanceTasksByDeviceId(id);
        }

        /// <summary>
        /// HTTP PUT: api/maintenancetasks/{idOfDevice}
        /// </summary>
        /// <param name="deviceId"></param>
        /// <param name="description"></param>
        /// <param name="severity"></param>       
        [HttpPut("{deviceId}/{description}/{severity}")]
        public async Task<MaintenanceTask> CreateNewTask(string deviceId, string description, Severity severity)
        {
            await maintenanceTaskService.CreateNewTask(deviceId, description, severity);

            return
        }
    }
}
