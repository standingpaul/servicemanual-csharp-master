﻿using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace EtteplanMORE.ServiceManual.ApplicationCore.Entities
{
    public class MaintenanceTask
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string TaskId { get; private set; }
        public string DeviceId { get; private set; }
        public string Description { get; private set; }
        public Severity Severity { get; private set; }
        public MaintenanceTaskStatus TaskStatus { get; private set; }
        public DateTime RegistrationTime { get; private set; } = DateTime.UtcNow;

        public MaintenanceTask(string deviceTaskRefersTo, string description, Severity severity, MaintenanceTaskStatus taskStatus)
        {
            DeviceId = deviceTaskRefersTo;
            Description = description;
            Severity = severity;
            TaskStatus = taskStatus;
        }
    }

    public enum Severity
    {
        critial = 0, important, unimportant
    }

    public enum MaintenanceTaskStatus
    {
        open = 0, closed
    }
}