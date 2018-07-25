using System.Runtime.Serialization;

namespace Lunz.Microservice.Core.Models.ActivityStreams
{
    [DataContract(Namespace = "")]
    public class Actor
    {
        [DataMember(EmitDefaultValue = true)]
        public ActorType Type { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public string Id { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public string Name { get; set; }
    }
}