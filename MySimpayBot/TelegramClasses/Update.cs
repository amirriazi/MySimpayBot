using Newtonsoft.Json;
using System;
using System.Runtime.Serialization;
using Telegram.Bot.Types.Enums;

namespace AltonTechBotManager
{
    [DataContract]
    public class Update
    {

        [DataMember(Name = "callback_query")]
        public CallbackQuery CallbackQuery { set; get; }

        [DataMember(Name = "chosen_inline_result")]
        public ChosenInlineResult ChosenInlineResult { set; get; }

        [DataMember(Name = "edited_message")]
        public Message EditedMessage { set; get; }

        [DataMember(Name = "update_id")]
        public long Id { set; get; }

        [DataMember(Name = "inline_query")]
        public InlineQuery InlineQuery { set; get; }

        [DataMember(Name = "message")]
        public Message Message { set; get; }


        [DataMember(Name = "channel_post")]
        public Message ChannelPost { set; get; }


        [IgnoreDataMember]
        public UpdateType Type
        {
            get
            {
                if (Message != null) return UpdateType.MessageUpdate;
                if (InlineQuery != null) return UpdateType.InlineQueryUpdate;
                if (ChosenInlineResult != null) return UpdateType.ChosenInlineResultUpdate;
                if (CallbackQuery != null) return UpdateType.CallbackQueryUpdate;
                if (EditedMessage != null) return UpdateType.EditedMessage;
                if (ChannelPost != null) return UpdateType.ChannelPost;
                return UpdateType.UnknownUpdate;
                throw new ArgumentOutOfRangeException();
            }
        }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}