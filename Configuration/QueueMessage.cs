using System;
using System.Collections.Generic;
using System.Text;

namespace Configuration
{
    public class QueueMessage<T>
    {
        public Guid Key {get; set;}

        public T Value { get; set; }
    }
}
