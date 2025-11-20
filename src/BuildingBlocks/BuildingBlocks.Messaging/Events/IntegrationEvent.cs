using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuildingBlocks.Messaging.Events
{
    public record IntegrationEvent
    {
        public Guid Id => Guid.NewGuid();
        public DateTime CreatedAt => DateTime.UtcNow;
        public string EventType => GetType().AssemblyQualifiedName;  
    }
}
