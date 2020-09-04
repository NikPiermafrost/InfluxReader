using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace influx_fe.Services
{
    public interface IRabbitSender
    {
        void SendReplayDataToRabbit(string Data);
    }
}
