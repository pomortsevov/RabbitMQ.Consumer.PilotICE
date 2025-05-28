using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RabbitMQ.Consumer.PilotICE
{
    public class RabbitMQConsumerConfig
    {
        public bool IsEnabled { get; set; }
        public string RabbitMQUrl { get; set; }
        public string RabbitMQUser { get; set; }
        public string RabbitMQPassword { get; set; }
        public string RabbitMQExchange { get; set; }
        public string RabbitMQQueue { get; set; }
        public string Timeout { get; set; }
        public string PilotICE_URL { get; set; }
        public string PilotICE_user { get; set; }
        public string PilotICE_passwd { get; set; }
        public string PilotICE_search_user { get; set; }
        public string PilotICE_search_passwd { get; set; }
       

    }
}
