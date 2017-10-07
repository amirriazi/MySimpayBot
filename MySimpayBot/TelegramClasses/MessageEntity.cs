using System.Runtime.Serialization;

namespace AltonTechBotManager
{
    [DataContract]
    //
    // Summary:
    //     This object represents one special entity in a text message. For example, hashtags,
    //     usernames, URLs, etc.
    public class MessageEntity
    {
        [DataMember(Name = "length")]
        public int Length { set; get; }
        [DataMember(Name = "offset")]
        public int Offset { set; get; }
        [DataMember(Name = "type")]
        public string Type { set; get; }
        [DataMember(Name = "url")]
        public string Url { set; get; }
        [DataMember(Name = "user")]
        public User User { set; get; }
    }
}