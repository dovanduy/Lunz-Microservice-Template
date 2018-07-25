using System;
using System.Net;
using Lunz.Microservice.Core.Models;
using Lunz.Microservice.Core.Models.ActivityStreams;

namespace Lunz.Microservice.ActivityStreams.Services
{
    public class OrderHtmlContext : HtmlContext
    {
        public virtual string OrderLink(Guid orderId)
        {
            return $"<a href=\"_parent\">#{orderId}</a>";
        }
    }

    public class HtmlContext
    {
        public string Encode(string htmlString)
        {
            return WebUtility.HtmlEncode(htmlString);
        }

        public virtual string UserLink(UserDetails user)
        {
            if (user == null)
                return string.Empty;

            return $"<a class=\"activity-item-user activity-item-author\" target=\"_parent\">{user.Name}</a>";
        }

        public virtual string AttchmentLink(ActivityAttachment attachment)
        {
            if (attachment == null)
                return string.Empty;

            if (!string.IsNullOrWhiteSpace(attachment.Type)
                && attachment.Type.ToLowerInvariant().Equals("image"))
            {
                return
                    $"<br/><a href=\"{attachment.Url}\" target=\"_blank\"><img src=\"{attachment.Url}\" alt=\"{attachment.Content}\"></a>";
            }

            return $"<br/><a href=\"{attachment.Url}\" target=\"_blank\">{attachment.Content}</a>";
        }
    }
}