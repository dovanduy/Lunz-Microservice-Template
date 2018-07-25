using System.Runtime.Serialization;

namespace Lunz.Microservice.Core.Models.ActivityStreams
{
    /// <summary>
    /// Based on https://www.w3.org/TR/activitystreams-vocabulary/#dfn-activity
    /// </summary>
    [DataContract(Namespace = "")]
    public class Activity : ActivityStreamObject
    {
        [DataMember(EmitDefaultValue = true)]
        public ActivityType Type { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public Actor Actor { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public ActivityStreamObject Object { get; set; }
    }
}