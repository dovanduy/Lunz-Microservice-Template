using System.Runtime.Serialization;

namespace Lunz.Microservice.Core.Models.ActivityStreams
{
    public enum ActorType
    {
        [EnumMember(Value = "Person")] Person
    }
}