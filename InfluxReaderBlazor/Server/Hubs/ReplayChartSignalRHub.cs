using DataAccess;
using InfluxReaderBlazor.Shared;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
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
            var SimulationDate = DateTime.Now.Ticks;
            var entities = List.Select(x => x.EntityName).ToList();
            _rabbitSender.SendReplayDataToRabbit(@$"{JsonConvert.SerializeObject(new SimulationtoRabbitValues 
                { 
                    TimeStamp = DateTime.Now.Ticks, 
                    Status = 0, 
                    Entities = entities,
                    Dateref = SimulationDate
                })}");
            for (int i = 0; i < List[0].Values.Count; i++)
            {
                await Clients.Caller.SendAsync("CounterForjs", i);
                System.Threading.Thread.Sleep(Frequency);
                _rabbitSender.SendReplayDataToRabbit(@$"{JsonConvert.SerializeObject(new SimulationtoRabbitValues 
                    { 
                        TimeStamp = List[0].Values[i].Time.Ticks,
                        Status = 1,
                        Entities = entities,
                        Dateref = SimulationDate
                    })}");
            }
            _rabbitSender.SendReplayDataToRabbit(@$"{JsonConvert.SerializeObject(new SimulationtoRabbitValues 
                { 
                    TimeStamp = DateTime.Now.Ticks,
                    Status = 2, 
                    Entities = entities,
                    Dateref = SimulationDate
                })}");
        }
    }
}
