using System.Runtime.Serialization;

namespace AltonTechBotManager
{
    [DataContract]

    public class CallbackQuery
    {
        [DataMember(Name = "chat_instance")]
        public string ChatInstance { get; set; }
        [DataMember(Name = "data")]
        public string Data { get; set; }

        [DataMember(Name = "from")]
        public User From { get; set; }
        [DataMember(Name = "game_short_name")]
        public string GameShortName { get; set; }
        [DataMember(Name = "id")]
        public string ID { get; set; }
        [DataMember(Name = "inline_message_id")]
        public string InlineMessageId { get; set; }
        public bool IsGameQuery { set; get; }
        [DataMember(Name = "message")]
        public Message Message { get; set; }
    }
}