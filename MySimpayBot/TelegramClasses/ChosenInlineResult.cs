using System.Runtime.Serialization;

namespace AltonTechBotManager
{
    [DataContract]
    public class ChosenInlineResult
    {
        [DataMember(Name = "from")]
        public User From { set; get; }
        [DataMember(Name = "inline_message_id")]
        public string InlineMessageId { set; get; }
        [DataMember(Name = "location")]
        public Location Location { set; get; }
        [DataMember(Name = "query")]
        public string Query { set; get; }
        [DataMember(Name = "result_id")]
        public string ResultId { set; get; }
    }
}