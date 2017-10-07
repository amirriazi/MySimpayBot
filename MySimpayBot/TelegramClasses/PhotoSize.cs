using System.Runtime.Serialization;

namespace AltonTechBotManager
{
    [DataContract]
    public class PhotoSize
    {
        [DataMember(Name = "file_id", Order = 1)]
        public string ID { set; get; }

        [DataMember(Name = "width", Order = 2)]
        public long Width { set; get; }

        [DataMember(Name = "height", Order = 3)]
        public long Height { set; get; }

        [DataMember(Name = "file_size", Order = 4)]
        public long Size { set; get; }
    }
}