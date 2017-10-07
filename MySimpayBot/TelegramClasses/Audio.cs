using System.Runtime.Serialization;

namespace AltonTechBotManager
{
    [DataContract]
    public class Audio
    {
        [DataMember(Name = "file_id", Order = 1)]
        public string ID { set; get; }

        [DataMember(Name = "duration", Order = 2)]
        public long Duration { set; get; }

        [DataMember(Name = "performer", Order = 3)]
        public string Performer { set; get; }

        [DataMember(Name = "title", Order = 4)]
        public string Title { set; get; }

        [DataMember(Name = "mime_type", Order = 5)]
        public string MimeType { set; get; }

        [DataMember(Name = "file_size", Order = 6)]
        public long FileSize { set; get; }
    }
}