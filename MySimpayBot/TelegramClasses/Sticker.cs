using System.Runtime.Serialization;

namespace AltonTechBotManager
{
    [DataContract]
    public class Sticker
    {
        [DataMember(Name = "file_id", Order = 1)]
        public string ID { set; get; }

        [DataMember(Name = "width", Order = 2)]
        public string Width { set; get; }

        [DataMember(Name = "height", Order = 3)]
        public string Height { set; get; }

        [DataMember(Name = "thumb", Order = 4)]
        public PhotoSize Thumb { set; get; }

        [DataMember(Name = "file_size", Order = 5)]
        public long Size { set; get; }
    }
}