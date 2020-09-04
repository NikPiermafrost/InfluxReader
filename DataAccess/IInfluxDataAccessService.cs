using DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataAccess
{
    public interface IInfluxDataAccessService
    {
        Task<List<BoolModel>> GetBoolEntries(DateTime DateStart, DateTime DateEnd);
        Task<List<FloatModel>> GetFloatEntries(DateTime DateStart, DateTime DateEnd);
        Task<List<IntModel>> GetIntEntries(DateTime DateStart, DateTime DateEnd);
        Task<List<StringModel>> GetStringEntries(DateTime DateStart, DateTime DateEnd);
    }
}