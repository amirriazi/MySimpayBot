using System.Runtime.Serialization;

namespace AltonTechBotManager
{
    [DataContract]
    public class Video
    {
        [DataMember(Name = "file_id", Order = 1)]
        public string ID { set; get; }

        [DataMember(Name = "width", Order = 2)]
        public long Width { set; get; }

        [DataMember(Name = "height", Order = 3)]
        public long Height { set; get; }

        [DataMember(Name = "duration", Order = 4)]
        public long Duration { set; get; }

        [DataMember(Name = "thumb", Order = 5)]
        public PhotoSize Thumb { set; get; }

        [DataMember(Name = "mime_type", Order = 6)]
        public string MimeType { set; get; }

        [DataMember(Name = "file_size", Order = 7)]
        public long Size { set; get; }
    }
}