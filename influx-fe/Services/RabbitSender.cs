using influx_fe.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace influx_fe.Services
{
    public class RabbitSender: IRabbitSender
    {
        private readonly ConfigurationModel _conf;
        public RabbitSender(ConfigurationModel configuration)
        {
            _conf = configuration;
        }
        public void test()
        {
            Console.WriteLine(_conf.Hostname);
        }
    }
}
