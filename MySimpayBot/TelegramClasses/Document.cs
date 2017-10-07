using System.Runtime.Serialization;

namespace AltonTechBotManager
{
    [DataContract]
    public class Document
    {
        [DataMember(Name = "file_id", Order = 1)]
        public string ID { set; get; }

        [DataMember(Name = "thumb", Order = 2)]
        public PhotoSize Thumb { set; get; }

        [DataMember(Name = "file_name", Order = 3)]
        public string Name { set; get; }

        [DataMember(Name = "mime_type", Order = 4)]
        public string MimeType { set; get; }

        [DataMember(Name = "file_size", Order = 5)]
        public long Size { set; get; }
    }
}