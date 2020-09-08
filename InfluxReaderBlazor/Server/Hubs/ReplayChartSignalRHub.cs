using InfluxReaderBlazor.Shared;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InfluxReaderBlazor.Server.Hubs
{
    public class ReplayChartSignalRHub: Hub
    {
        public async Task InitializeSimulation(List<ValueModel> List, int Frequency)
        {
            for (int i = 0; i < List[0].Values.Count; i++)
            {
                await Clients.Caller.SendAsync("CounterForjs", i);
                System.Threading.Thread.Sleep(Frequency);
            }
        }
    }
}
