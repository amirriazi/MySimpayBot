using System.Runtime.Serialization;

namespace AltonTechBotManager
{
    [DataContract]
    //
    // Summary:
    //     This object represents an animation file to be displayed in the message containing
    //     a Telegram.Bot.Types.Game.
    //
    // Summary:
    //     This object represents a game. Use BotFather to create and edit games, their
    //     short names will act as unique identifiers.
    public class Game
    {

        [DataMember(Name = "animation")]
        public Animation Animation { get; set; }
        [DataMember(Name = "description")]
        public string Description { get; set; }
        [DataMember(Name = "photo")]
        public PhotoSize[] Photo { get; set; }
        [DataMember(Name = "text")]
        public string Text { get; set; }
        [DataMember(Name = "text_entities")]
        public MessageEntity[] TextEntities { get; set; }
        [DataMember(Name = "title")]
        public string Title { get; set; }
    }
}