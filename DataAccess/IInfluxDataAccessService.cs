using DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataAccess
{
    public interface IInfluxDataAccessService
    {
        Task<ValueModel> SelectDataReturn(string EntityType, DateTime DateStart, DateTime DateEnd);
    }
}