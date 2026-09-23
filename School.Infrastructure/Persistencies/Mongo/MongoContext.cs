using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using School.Domain.Entitis;
using System;
using System.Collections.Generic;
using System.Text;

namespace School.Infrastructure.Persistencies.Mongo
{
    public class MongoContext
    {
        private readonly MongoSetting _setting;
        public MongoContext(IOptions<MongoSetting> setting)
        {
            _setting = setting.Value;
            var client = new MongoClient(_setting.ConeenectionString);
            Database = client.GetDatabase(_setting.DataBase);
        }
        public IMongoDatabase Database { get; }
        public IMongoCollection<ExceptionLog> ExcpetionLogs => Database.GetCollection<ExceptionLog>(_setting.ExcpetionLogColection);
    }
}
