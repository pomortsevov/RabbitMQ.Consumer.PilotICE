using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RabbitMQ.Consumer.PilotICE
{
    public class SearchResultContainer
    {
        public List<string> Ids { get; } = new List<string>();
        public string FirstID;

    }
}
