using System.Runtime.Serialization;

namespace AltonTechBotManager
{
    [DataContract]
    public class InlineQuery
    {

        [DataMember(Name = "from")]
        public User From { set; get; }
        [DataMember(Name = "id")]
        public string ID { set; get; }
        [DataMember(Name = "location")]
        public Location Location { set; get; }
        [DataMember(Name = "offset")]
        public string Offset { set; get; }
        [DataMember(Name = "query")]
        public string Query { set; get; }
    }
}