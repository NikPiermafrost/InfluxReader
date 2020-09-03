using DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataAccess
{
    public interface IInfluxDataAccessService
    {
        Task<List<BoolModel>> GetBoolEntries();
        Task<List<FloatModel>> GetFloatEntries();
        Task<List<IntModel>> GetIntEntries(DateTime DateStart, DateTime DateEnd);
        Task<List<StringModel>> GetStringEntries();
    }
}