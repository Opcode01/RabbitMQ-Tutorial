﻿using RabbitMQ.Client;
using Send;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PublishLog
{
    public abstract class LoggerBase
    {
        public string _hostname { get; set; } = "localhost";
        protected Sender _sender;

        protected IConnection _connection;
        protected IModel _channel;

        public LoggerBase()
        {
            //Initialize Connection
            var factory = new ConnectionFactory();
            factory.HostName = _hostname;
            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();
        }

        public void PublishMessage(string message)
        {
            _sender.SendMessage(message);
        }

        public void CloseConnection()
        {
            _sender.CloseConnection();
        }
    }
}
