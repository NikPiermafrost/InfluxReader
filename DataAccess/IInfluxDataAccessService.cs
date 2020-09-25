using InfluxReaderBlazor.Shared;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataAccess
{
    public interface IInfluxDataAccessService
    {
        Task<ValueModel> SelectDataReturn(string EntityType, DateTime DateStart, DateTime DateEnd);
        Task<List<string>> GetDatabaseTables();
        List<ValueModel> TrimInconsistentData(List<ValueModel> toTrim);
    }
}