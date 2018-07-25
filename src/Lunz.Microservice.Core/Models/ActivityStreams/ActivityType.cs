using System.Runtime.Serialization;

namespace Lunz.Microservice.Core.Models.ActivityStreams
{
    /// <summary>
    /// https://www.w3.org/TR/activitystreams-vocabulary/#activity-types
    /// </summary>
    public enum ActivityType
    {
        [EnumMember(Value = "Activity")] Activity = 0,
        [EnumMember(Value = "Accept")] Accept,
        [EnumMember(Value = "Add")] Add,
        [EnumMember(Value = "Announce")] Announce,
        [EnumMember(Value = "Arrive")] Arrive,
        [EnumMember(Value = "Block")] Block,
        [EnumMember(Value = "Create")] Create,
        [EnumMember(Value = "Delete")] Delete,
        [EnumMember(Value = "Dislike")] Dislike,
        [EnumMember(Value = "Flag")] Flag,
        [EnumMember(Value = "Follow")] Follow,
        [EnumMember(Value = "Ignore")] Ignore,
        [EnumMember(Value = "Invite")] Invite,
        [EnumMember(Value = "Join")] Join,
        [EnumMember(Value = "Leave")] Leave,
        [EnumMember(Value = "Like")] Like,
        [EnumMember(Value = "Listen")] Listen,
        [EnumMember(Value = "Move")] Move,
        [EnumMember(Value = "Offer")] Offer,
        [EnumMember(Value = "Question")] Question,
        [EnumMember(Value = "Reject")] Reject,
        [EnumMember(Value = "Read")] Read,
        [EnumMember(Value = "Remove")] Remove,
        [EnumMember(Value = "TentativeReject")] TentativeReject,
        [EnumMember(Value = "TentativeAccept")] TentativeAccept,
        [EnumMember(Value = "Travel")] Travel,
        [EnumMember(Value = "Undo")] Undo,
        [EnumMember(Value = "Update")] Update,
        [EnumMember(Value = "View")] View
    }
}