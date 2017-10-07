using System.Runtime.Serialization;

namespace AltonTechBotManager
{
    [DataContract]
    public class Chat
    {
        [DataMember(Name = "id", Order = 1)]
        public long ID { set; get; }

        [DataMember(Name = "type", Order = 2)]
        public string Type { set; get; }

        [DataMember(Name = "title", Order = 3)]
        public string Title { set; get; }

        [DataMember(Name = "username", Order = 4)]
        public string UserName { set; get; }

        [DataMember(Name = "first_name", Order = 5)]
        public string FirstName { set; get; }

        [DataMember(Name = "last_name", Order = 6)]
        public string LastName { set; get; }
    }
}