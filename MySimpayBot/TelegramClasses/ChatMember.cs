using System.Runtime.Serialization;

namespace AltonTechBotManager
{
    [DataContract]
    //
    // Summary:
    //     This object contains information about one member of the chat.
    public class ChatMember
    {
        [DataMember(Name = "status")]
        public string Status { set; get; }
        [DataMember(Name = "user")]
        public User User { set; get; }
    }
}