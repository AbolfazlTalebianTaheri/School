using School.Domain.Entitis;
using System;
using MongoDB.Bson.Serialization;
using System.Collections.Generic;
using System.Text;
using MongoDB.Bson.Serialization.IdGenerators;
using MongoDB.Bson.Serialization.Serializers;

namespace School.Infrastructure.Persistencies.Mongo
{
    public class MongoMappingConfig
    {
        private static bool _registered;
        private static readonly Lock Lock = new();
        public static void Register()
        {
            lock (Lock)
            {
                if (_registered)
                    return;
                RegisterExcpetionLog();
                _registered = true;
            }
        }
        private static void RegisterExcpetionLog()
        {
            if (BsonClassMap.IsClassMapRegistered(typeof(ExceptionLog)))
                return;
            BsonClassMap.RegisterClassMap<ExceptionLog>(map =>
            {
                map.AutoMap();
                map.MapIdMember(x => x.Id)
                .SetIdGenerator(StringObjectIdGenerator.Instance)
                .SetSerializer(new StringSerializer(MongoDB.Bson.BsonType.ObjectId));
                map.SetIgnoreExtraElements(true);
            });
        }
    }
}
