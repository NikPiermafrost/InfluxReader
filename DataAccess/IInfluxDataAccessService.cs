using InfluxReaderBlazor.Shared;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataAccess
{
    public interface IInfluxDataAccessService
    {
        Task<ValueModel> GetEntries(DateTime DateStart, DateTime DateEnd, string EntityName);
        Task<List<string>> GetDatabaseTables();
        List<ValueModel> TrimInconsistentData(List<ValueModel> toTrim);
    }
}