using EasyNetQ;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Artel.Telemetry.Common.Extension
{
    
    public  abstract class DebugInfo
    {
        public abstract void Print(string message);
    }

    public class ConsoleMessage : DebugInfo
    {
        public override void Print(string message) => Console.WriteLine(message);
    }

    public class RabbitMqMessage : DebugInfo
    {
        IBus bus;
        public RabbitMqMessage(string connectionString) => bus = RabbitHutch.CreateBus(connectionString);

        public override void Print(string message) => bus.Publish(message);
    }

    public class BrodcastMessage : DebugInfo
    {
        IBus bus;
        public BrodcastMessage(string connectionString) => bus = RabbitHutch.CreateBus(connectionString);

        public override void Print(string message)
        {
            Console.WriteLine(message);
            bus.Publish(message);
        }
    }
}
