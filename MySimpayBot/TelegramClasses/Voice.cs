using System.Runtime.Serialization;

namespace AltonTechBotManager
{
    [DataContract]
    public class Voice
    {
        [DataMember(Name = "file_id", Order = 1)]
        public string ID { set; get; }

        [DataMember(Name = "duration", Order = 2)]
        public long Duration { set; get; }

        [DataMember(Name = "mime_type", Order = 3)]
        public string MimeType { set; get; }

        [DataMember(Name = "file_size", Order = 4)]
        public long Size { set; get; }
    }
}