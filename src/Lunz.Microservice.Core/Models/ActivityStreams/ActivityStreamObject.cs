using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Lunz.Microservice.Core.Models.ActivityStreams
{
    /// <summary>
    /// Based on https://www.w3.org/TR/activitystreams-vocabulary/#dfn-object
    /// </summary>
    [DataContract(Namespace = "")]
    public class ActivityStreamObject
    {
        /// <summary>
        /// Identifies the context within which the object exists or an activity was performed.
        /// </summary>
        /// <remarks>
        /// The notion of "context" used is intentionally vague. The intended function 
        /// is to serve as a means of grouping objects and activities that share a
        /// common originating context or purpose. An example could be all activities 
        /// relating to a common project or event.
        /// </remarks>
        [DataMember(EmitDefaultValue = false)]
        public string Context { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public DateTime Published { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public string Summary { get; set; }


        [DataMember(EmitDefaultValue = false)]
        public string Content { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public string MediaType { get; set; }

        /// <summary>
        /// Identifies one or more links to representations of the object
        /// </summary>
        [DataMember(EmitDefaultValue = false)]
        public string Url { get; set; }

        /// <summary>
        /// The date and time at which the object was updated
        /// </summary>
        [DataMember(EmitDefaultValue = false)]
        public DateTime Updated { get; set; }

        /// <summary>
        /// Identifies a resource attached or related to an object that 
        /// potentially requires special handling. The intent is to provide 
        /// a model that is at least semantically similar to attachments in email.
        /// </summary>
        [DataMember(EmitDefaultValue = false)]
        public List<ActivityAttachment> Attachment { get; set; }
    }
}