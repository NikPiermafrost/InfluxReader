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

        public async Task<List<string>> GetDatabaseTables()
        {
            var series = await _client.GetInfluxDBStructureAsync(_dbName);
            return series.Measurements.Select(x => x.Name).ToList();
        }

        public async Task<ValueModel> GetEntries(DateTime DateStart, DateTime DateEnd, string EntityName)
        {
            try
            {
                var queryResult = await _client.QueryMultiSeriesAsync<FloatModel>(_dbName, $"SELECT * FROM {EntityName} WHERE time <= {GetEpoch(DateEnd)} AND time >= {GetEpoch(DateStart)}");
                var result = new ValueModel()
                {
                    EntityName = EntityName,
                    Values = queryResult.First().Entries.ToList()
                };
                return result;
            }
            catch (Exception ex)
            {
                return new ValueModel()
                {
                    EntityName = EntityName,
                    Values = new List<FloatModel>()
                };
            }
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
