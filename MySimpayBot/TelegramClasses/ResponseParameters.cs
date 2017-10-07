using System.Runtime.Serialization;

namespace AltonTechBotManager
{
    [DataContract]
    //
    // Summary:
    //     Contains information about why a request was unsuccessfull.
    public class ResponseParameters
    {
        [DataMember(Name = "migrate_to_chat_id")]
        public long MigrateToChatId { get; set; }
        [DataMember(Name = "retry_after")]
        public int RetryAfter { get; set; }
    }
}