using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Domain.Exceptions
{
    public class DomainExcception:Exception
    {
        public DomainExcception(string message):base($"Domain Error: {message}")    
        {
        }
    }
}
