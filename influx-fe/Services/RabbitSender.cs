using influx_fe.Models;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace influx_fe.Services
{
    public class RabbitSender: IRabbitSender
    {
        private readonly ConfigurationModel _conf;
        private readonly IConnectionFactory _connectionFactory;
        private readonly IConnection _connection;
        private readonly IModel _channel;

        public RabbitSender(ConfigurationModel configuration)
        {
            _conf = configuration;
            _connectionFactory = new ConnectionFactory() { HostName = _conf.Hostname};
            _connection = _connectionFactory.CreateConnection();
            _channel = _connection.CreateModel();
        }
        public void SendReplayDataToRabbit(string Data)
        {
            try
            {
                _channel.QueueDeclare(queue: _conf.Queue, durable: true, exclusive: false, autoDelete: false, arguments: null);

                var body = Encoding.UTF8.GetBytes(Data);

                _channel.BasicPublish(exchange: _conf.Exchange, routingKey: _conf.RoutingKey, basicProperties: null, body: body);
                Console.WriteLine($"Sent: {Data}");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
