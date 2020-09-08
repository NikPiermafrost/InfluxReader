using DataAccess;
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
        private readonly IRabbitSender _rabbitSender;
        public ReplayChartSignalRHub(IRabbitSender rabbitSender)
        {
            _rabbitSender = rabbitSender;
        }
        public async Task InitializeSimulation(List<ValueModel> List, int Frequency)
        {
            _rabbitSender.SendReplayDataToRabbit("Started");
            for (int i = 0; i < List[0].Values.Count; i++)
            {
                await Clients.Caller.SendAsync("CounterForjs", i);
                System.Threading.Thread.Sleep(Frequency);
                _rabbitSender.SendReplayDataToRabbit($"{List[0].Values[i].Time.Ticks}");
            }
            _rabbitSender.SendReplayDataToRabbit("Ended");
        }
    }
}
