using AdysTech.InfluxDB.Client.Net;
using DataAccess.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

        public async Task<List<BoolModel>> GetBoolEntries()
        {
            var result = await _client.QueryMultiSeriesAsync<BoolModel>(_dbName, "SELECT * FROM IsLorem");
            return result.First().Entries.ToList();
        }

        public async Task<List<FloatModel>> GetFloatEntries()
        {
            var result = await _client.QueryMultiSeriesAsync<FloatModel>(_dbName, "SELECT * FROM RandomInt");
            return result.First().Entries.ToList();
        }

        public async Task<List<IntModel>> GetIntEntries()
        {
            var result = await _client.QueryMultiSeriesAsync<IntModel>(_dbName, "SELECT * FROM RandomFloat");
            return result.First().Entries.ToList();
        }

        public async Task<List<StringModel>> GetStringEntries()
        {
            var result = await _client.QueryMultiSeriesAsync<StringModel>(_dbName, "SELECT * FROM LoremIpsum");
            return result.First().Entries.ToList();
        }
    }
}
