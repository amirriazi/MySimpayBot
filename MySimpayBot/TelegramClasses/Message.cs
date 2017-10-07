using System;
using System.Runtime.Serialization;
using Telegram.Bot.Types.Enums;

namespace AltonTechBotManager
{
    [DataContract]
    public class Message
    {
        [DataMember(Name = "message_id", Order = 1)]
        public int ID { set; get; }

        [DataMember(Name = "from", Order = 2)]
        public User From { set; get; }

        [DataMember(Name = "date", Order = 3)]
        public long Date { set; get; }

        [DataMember(Name = "chat", Order = 4)]
        public Chat Chat { set; get; }

        [DataMember(Name = "forward_from", Order = 5)]
        public User ForwardFrom { set; get; }

        [DataMember(Name = "forward_date", Order = 6)]
        public long ForwardDate { set; get; }

        [DataMember(Name = "reply_to_message", Order = 7)]
        public Message ReplyToMessage { set; get; }

        [DataMember(Name = "text", Order = 8)]
        public string Text { set; get; }

        [DataMember(Name = "audio", Order = 9)]
        public Audio Audio { set; get; }

        [DataMember(Name = "document", Order = 10)]
        public Document Document { set; get; }

        [DataMember(Name = "photo", Order = 11)]
        public PhotoSize[] Photo { set; get; }

        [DataMember(Name = "sticker", Order = 12)]
        public Sticker Sticker { set; get; }

        [DataMember(Name = "video", Order = 13)]
        public Video Video { set; get; }

        [DataMember(Name = "voice", Order = 14)]
        public Voice Voice { set; get; }

        [DataMember(Name = "caption", Order = 15)]
        public string Caption { set; get; }

        [DataMember(Name = "contact", Order = 16)]
        public Contact Contact { set; get; }

        [DataMember(Name = "location", Order = 17)]
        public Location Location { set; get; }

        [DataMember(Name = "new_chat_participant", Order = 18)]
        public User NewChatParticipant { set; get; }

        [DataMember(Name = "left_chat_participant", Order = 19)]
        public User LeftChatParticipant { set; get; }

        [DataMember(Name = "new_chat_title", Order = 20)]
        public string NewChatTitle { set; get; }

        [DataMember(Name = "new_chat_photo", Order = 21)]
        public PhotoSize[] NewChatPhoto { set; get; }

        [DataMember(Name = "delete_chat_photo", Order = 22)]
        public bool DeleteChatPhoto { set; get; }

        [DataMember(Name = "group_chat_created", Order = 23)]
        public bool GroupChatCreated { set; get; }

        [DataMember(Name = "supergroup_chat_created", Order = 24)]
        public bool SupergroupChatCreated { set; get; }

        [DataMember(Name = "channel_chat_created", Order = 25)]
        public bool ChannelChatCreated { set; get; }

        [DataMember(Name = "migrate_to_chat_id", Order = 26)]
        public long MigrateToChatID { set; get; }

        [DataMember(Name = "migrate_from_chat_id", Order = 27)]
        public long MigrateFromChatID { set; get; }

        [IgnoreDataMember]
        public MessageType Type
        {
            get
            {
                if (Audio != null)
                    return MessageType.AudioMessage;

                if (Document != null)
                    return MessageType.DocumentMessage;

                //if (Game != null)
                //    return MessageType.GameMessage;

                if (Photo != null)
                    return MessageType.PhotoMessage;

                if (Sticker != null)
                    return MessageType.StickerMessage;

                if (Video != null)
                    return MessageType.VideoMessage;

                if (Voice != null)
                    return MessageType.VoiceMessage;

                if (Contact != null)
                    return MessageType.ContactMessage;

                if (Location != null)
                    return MessageType.LocationMessage;

                if (Text != null)
                    return MessageType.TextMessage;

                //if (Venue != null)
                //    return MessageType.VenueMessage;

                //if (NewChatMember != null ||
                //    LeftChatMember != null ||
                //    NewChatTitle != null ||
                //    NewChatPhoto != null ||
                //    PinnedMessage != null ||
                //    DeleteChatPhoto ||
                //    GroupChatCreated ||
                //    SupergroupChatCreated ||
                //    ChannelChatCreated ||
                //    MigrateFromChatId == default(long) ||
                //    MigrateToChatId == default(long))
                //    return MessageType.ServiceMessage;

                return MessageType.UnknownMessage;
            }
        }
    }
}