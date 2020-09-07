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
        readonly ConfigurationModel _conf;
        readonly IConnectionFactory _connectionFactory;
        readonly IConnection _connection;
        readonly IModel _channel;

        public RabbitSender(ConfigurationModel configuration)
        {
            _conf = configuration;
            _connectionFactory = new ConnectionFactory() { HostName = _conf.Hostname, UserName = "nicola", Password = "Password1!", VirtualHost = "/" };
            _connection = _connectionFactory.CreateConnection();
            _channel = _connection.CreateModel();
            Console.WriteLine(_conf.Hostname);
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
                Console.WriteLine(ex.InnerException);
            }
        }
    }
}
