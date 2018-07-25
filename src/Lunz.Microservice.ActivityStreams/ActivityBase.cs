using System;
using Lunz.Microservice.Core.Models.ActivityStreams;

namespace Lunz.Microservice.ActivityStreams
{
    public abstract class ActivityBase : Activity
    {
        public Guid Id { get; set; }
    }
}