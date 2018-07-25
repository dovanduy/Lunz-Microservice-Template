using System.Runtime.Serialization;

namespace Lunz.Microservice.Core.Models.ActivityStreams
{
    [DataContract(Namespace = "")]
    public class ActivityStreamLink
    {
        [DataMember(EmitDefaultValue = true)]
        public ActivityStreamLinkType Type { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public string Href { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public string Name { get; set; }
    }
}