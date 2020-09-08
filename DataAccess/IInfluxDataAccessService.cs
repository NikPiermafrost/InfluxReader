using InfluxReaderBlazor.Shared;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataAccess
{
    public interface IInfluxDataAccessService
    {
        Task<ValueModel> SelectDataReturn(string EntityType, DateTime DateStart, DateTime DateEnd);
        List<ValueModel> TrimInconsistentData(List<ValueModel> toTrim);
    }
}