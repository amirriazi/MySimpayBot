using System.Runtime.Serialization;

namespace AltonTechBotManager
{
    [DataContract]
    //
    // Summary:
    //     This object represents an animation file to be displayed in the message containing
    //     a Telegram.Bot.Types.Game.
    public class Animation
    {

        [DataMember(Name = "file_id")]
        public string FileId { get; set; }
        [DataMember(Name = "file_name")]
        public string FileName { get; set; }
        [DataMember(Name = "file_size")]
        public int FileSize { get; set; }
        [DataMember(Name = "mime_type")]
        public string MimeType { get; set; }
        [DataMember(Name = "thumb")]
        public PhotoSize Thumb { get; set; }
    }
}