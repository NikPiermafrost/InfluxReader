using AdysTech.InfluxDB.Client.Net;
using InfluxReaderBlazor.Shared;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace DataAccess
{
    public class InfluxDataAccessService : IInfluxDataAccessService
    {
        private readonly string _ipAddress;
        private readonly string _userName;
        private readonly string _password;
        private readonly string _dbName;
        private readonly InfluxDBClient _client;
        public InfluxDataAccessService(string ipAddress, string userName, string password, string dbName)
        {
            _ipAddress = ipAddress;
            _password = password;
            _userName = userName;
            _dbName = dbName;
            _client = new InfluxDBClient(ipAddress, userName, password);
        }

        public async Task<ValueModel> SelectDataReturn(string EntityType, DateTime DateStart, DateTime DateEnd)
        {
            switch (EntityType)
            {
                case "bool":
                    return await GetBoolEntries(DateStart, DateEnd);
                case "float":
                    return await GetFloatEntries(DateStart, DateEnd);
                case "int":
                    return await GetIntEntries(DateStart, DateEnd);
                case "string":
                    return await GetStringEntries(DateStart, DateEnd);
                default:
                    throw new Exception($"Cannot find value type {EntityType}");
            }
        }

        public async Task<List<string>> GetDatabaseTables()
        {
            var series = await _client.GetInfluxDBStructureAsync(_dbName);
            return series.Measurements.Select(x => x.Name).ToList();
        }

        private async Task<ValueModel> GetBoolEntries(DateTime DateStart, DateTime DateEnd)
        {
            var queryResult = await _client.QueryMultiSeriesAsync<BoolModel>(_dbName, $"SELECT * FROM IsLorem WHERE Time <= {GetEpoch(DateEnd)} AND Time >= {GetEpoch(DateStart)}");
            var result = new ValueModel()
            {
                EntityName = "bool",
                Values = queryResult.First().Entries.Select(x => new FloatModel { Time = x.Time, Value = Convert.ToInt32(x.Value)}).ToList()
            };
            return result;
        }

        private async Task<ValueModel> GetFloatEntries(DateTime DateStart, DateTime DateEnd)
        {
            var queryResult = await _client.QueryMultiSeriesAsync<FloatModel>(_dbName, $"SELECT * FROM RandomInt WHERE Time <= {GetEpoch(DateEnd)} AND Time >= {GetEpoch(DateStart)}");
            var result = new ValueModel()
            {
                EntityName = "float",
                Values = queryResult.First().Entries.ToList()
            };
            return result;
        }

        private async Task<ValueModel> GetIntEntries(DateTime DateStart, DateTime DateEnd)
        {
            var queryResult = await _client.QueryMultiSeriesAsync<IntModel>(_dbName, $"SELECT * FROM RandomFloat WHERE Time <= {GetEpoch(DateEnd)} AND Time >= {GetEpoch(DateStart)}");
            var result = new ValueModel()
            {
                EntityName = "int",
                Values = queryResult.First().Entries.Select(x => new FloatModel { Time = x.Time, Value = x.Value }).ToList()
            };
            return result;
        }

        private async Task<ValueModel> GetStringEntries(DateTime DateStart, DateTime DateEnd)
        {
            var queryResult = await _client.QueryMultiSeriesAsync<StringModel>(_dbName, $"SELECT * FROM LoremIpsum WHERE Time <= {GetEpoch(DateEnd)} AND Time >= {GetEpoch(DateStart)}");
            var result = new ValueModel()
            {
                EntityName = "string",
                Values = queryResult.First().Entries.Select(x => new FloatModel { Time = x.Time, Value = x.Value.Length }).ToList()
            };
            return result;
        }

        private long GetEpoch(DateTime Date)
        {
            var epochStart = new DateTime(1970, 1, 1);
            return (Date - epochStart).Ticks * 100;
        }

        public List<ValueModel> TrimInconsistentData(List<ValueModel> toTrim)
        {
            var minArrayCount = toTrim.Min(x => x.Values.Count);
            toTrim.ForEach(x => x.Values = x.Values.Take(minArrayCount).ToList());
            return toTrim;
        }
    }
}
