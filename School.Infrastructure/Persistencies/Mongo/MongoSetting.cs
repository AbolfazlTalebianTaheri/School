using School.Domain.Entitis;
using System;
using System.Collections.Generic;
using System.Text;

namespace School.Infrastructure.Persistencies.Mongo
{
    public class MongoSetting
    {
        public const string SectionName = "Mongo";
        public string ConeenectionString { get; set; } = "mongodb://localhost:27017";
        public string DataBase { get; set; } = "SchoolDb";
        public string ExcpetionLogColection { get; set; } = "exception_logs";
    }
}
