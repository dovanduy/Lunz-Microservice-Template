using System;
using System.Runtime.Serialization;

namespace Lunz.Microservice.Core.Models
{
    [DataContract]
    public class UserDetails
    {
        protected UserDetails() { }

        [DataMember(Name = "sub")]
        public Guid? Id { get; protected set; }

        [DataMember(Name = "username")]
        public string Username { get; protected set; }

        [DataMember(Name = "name")]
        public string Name { get; protected internal set; }

        //[DataMember(Name = "sub")]
        public string Email { get; protected set; }

        // 只为兼容 用户中心
        [DataMember(Name = "authToken")]
        public Guid? AuthToken { get; protected set; }

        public static UserDetails Create(Guid? id, string name, string email)
        {
            if (name == null) throw new ArgumentNullException(nameof(name));
            if (email == null) throw new ArgumentNullException(nameof(email));

            return new UserDetails
            {
                Id = id,
                Name = name,
                Email = email
            };
        }

        public static UserDetails Create(string name, string email)
        {
            if (name == null) throw new ArgumentNullException(nameof(name));
            if (email == null) throw new ArgumentNullException(nameof(email));

            return new UserDetails
            {
                Name = name,
                Email = email
            };
        }
    }
}