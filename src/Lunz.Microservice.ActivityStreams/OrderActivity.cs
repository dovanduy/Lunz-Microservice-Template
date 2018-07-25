using System;

namespace Lunz.Microservice.ActivityStreams
{
    public class OrderActivity : ActivityBase
    {
        public Guid OrderId { get; set; }
    }
}