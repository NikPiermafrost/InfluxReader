﻿using DataAccess;
using InfluxReaderBlazor.Shared;
using Microsoft.AspNetCore.SignalR;
using Microsoft.JSInterop;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Timers;

namespace InfluxReaderBlazor.Server.Hubs
{
    public class ReplayChartSignalRHub: Hub
    {
        private readonly IRabbitSender _rabbitSender;
        private IHubCallerClients _clients { get; set; }
        public Timer _timer { get; set; }
        private List<ValueModel> ValueModelArray { get; set; }
        private List<string> entities { get; set; }
        private int Counter { get; set; }
        private DateTime SimulationDate { get; set; }
        public string SimulationId { get; set; }
        public bool IsReplay { get; set; }

        public ReplayChartSignalRHub(IRabbitSender rabbitSender)
        {
            _rabbitSender = rabbitSender;
            _timer = new Timer();
            _timer.AutoReset = true;
            _timer.Elapsed += async (sender, e) => await RabbitHandler(ValueModelArray);
        }

        public async Task InitializeSimulation(List<ValueModel> List, int Frequency, bool isReplay)
        {
            Counter = 0;
            _clients = Clients;
            _timer.Interval = Frequency;
            ValueModelArray = List;
            entities = List.Select(x => x.EntityName).ToList();
            SimulationDate = DateTime.Now;
            SimulationId = Guid.NewGuid().ToString();
            IsReplay = isReplay;
            _timer.Start();
        }

        private async Task RabbitHandler(List<ValueModel> List)
        {
            _rabbitSender.SendReplayDataToRabbit(@$"{JsonConvert.SerializeObject(new SimulationtoRabbitValues
            {
                TimeStamp = DateTime.Now.Ticks,
                Entities = entities,
                Dateref = SimulationDate.Ticks,
                SimId = SimulationId
            })}");
            await _clients.Caller.SendAsync("CounterForjs", Counter);
            Counter++;
            if (Counter == List[0].Values.Count)
            {
                if (IsReplay)
                {
                    Counter = 0;
                }
                else
                {
                    _timer.Stop();
                }
            }
        }
    }
}
