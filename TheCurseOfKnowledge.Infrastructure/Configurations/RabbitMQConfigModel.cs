using System;
using System.Collections.Generic;

namespace TheCurseOfKnowledge.Infrastructure.Configurations
{
    public class RabbitMQConfigModel
    {
        public string name { get; set; }
        public string hostname { get; set; }
        public string username { get; set; }
        public string password { get; set; }
        public int port { get; set; } = 5672;
        public string vhost { get; set; } = "/";
        public bool automaticrecoveryenabled { get; set; } = true;
        public int networkrecoveryintervalinseconds { get; set; } = 5;
        public string queue { get; set; }
        public ushort prefetchcount { get; set; } = 5;
        public string type { get; set; } = "topic";
        public string exchangeroutingkey { get; set; } = null;
    }
}
