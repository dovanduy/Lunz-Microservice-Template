using System;
using Lunz.Microservice.Core.Models;
using Lunz.Microservice.Core.Models.ActivityStreams;
using Lunz.Microservice.ActivityStreams.Services;

namespace Lunz.Microservice.ActivityStreams.Handlers
{
    public abstract class ActivityStreamsHandler<THtmlContext> where THtmlContext : HtmlContext
    {
        protected virtual string ActivityContextUri { get; set; }

        protected THtmlContext Html { get; private set; }

        protected ActivityStreamsHandler(THtmlContext htmlContext)
        {
            Html = htmlContext;
        }

        protected virtual T NewActivity<T>(UserDetails user, DateTime published, ActivityType type = ActivityType.Activity)
            where T : Activity, new()
        {
            return new T()
            {
                Context = ActivityContextUri,
                Actor = Actor(user),
                Published = published,
                Type = type,
            };
        }

        protected Actor Actor(UserDetails user)
        {
            return new Actor
            {
                Id = user?.Id.ToString(),
                Name = user?.Name,
                Type = ActorType.Person
            };
        }
    }
}