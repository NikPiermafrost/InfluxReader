using DataAccess;
using InfluxReaderBlazor.Shared;
using Microsoft.AspNetCore.SignalR;
using Microsoft.JSInterop;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using Timer = System.Timers.Timer;

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
        //handles the infinite data sento to the rabbit
        public bool IsDisposed = false;

        public ReplayChartSignalRHub(IRabbitSender rabbitSender)
        {
            _rabbitSender = rabbitSender;
            _timer = new Timer();
            _timer.AutoReset = true;
            _timer.Elapsed += async (sender, e) => await RabbitHandler(ValueModelArray);
        }

        public async Task InitializeSimulation(List<ValueModel> List, int Frequency, bool isReplay)
        {
            //in order to apply some data on the timer handler, i have to initialize some data on the constructor
            _clients = Clients;
            _timer.Interval = Frequency;
            ValueModelArray = List;
            entities = List.Select(x => x.EntityName).ToList();
            SimulationDate = DateTime.Now;
            SimulationId = Guid.NewGuid().ToString();
            IsReplay = isReplay;
            IsDisposed = false;
            _timer.Start();
        }

        private async Task RabbitHandler(List<ValueModel> List)
        {
            //sends the displayed data on the rabbitmq service
            if (!IsDisposed)
            {
                _rabbitSender.SendReplayDataToRabbit(@$"{JsonConvert.SerializeObject(new SimulationtoRabbitValues
                {
                    TimeStamp = DateTime.Now.Ticks,
                    Entities = entities,
                    Dateref = SimulationDate.Ticks,
                    SimId = SimulationId,
                    IsLoop = IsReplay
                })}");
            }
            //sends back to the frontend the counter of the displayed data for the data flow shifting
            await _clients.Caller.SendAsync("CounterForjs", Counter);
            Counter++;
            //resets the counter to 0 if it's a loop when it has reached the end of the array
            if (Counter == List[0].Values.Count)
            {
                if (IsReplay)
                {
                    Counter = 0;
                    IsDisposed = true;
                }
                else
                {
                    _timer.Stop();
                }
            }
        }
    }
}
