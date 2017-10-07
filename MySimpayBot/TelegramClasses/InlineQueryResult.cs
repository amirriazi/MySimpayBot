using Telegram.Bot.Types.InlineQueryResults;

namespace AltonTechBotManager
{
    public class InlineQueryResultMaster : InlineQueryResultNew
    {
        public string type { get; set; }
        public string title { get; set; }
        public string caption { get; set; }
        public string description { get; set; }
    }
    public class InlineQueryResultDocumentXXX
    {
        public string type { get; set; }
        public string id { get; set; }
        public string title { get; set; }
        public string caption { get; set; }
        public string document_url { get; set; }
        public string mime_type { get; set; }
        public string description { get; set; }
        public object reply_markup { get; set; }
        public object input_message_content { get; set; }
        public string thumb_url { get; set; }
        public string thumb_width { get; set; }
        public string thumb_height { get; set; }

    }
    public class InlineQueryResultPhotoXXX
    {
        public string type { get; set; }
        public string id { get; set; }
        public string photo_url { get; set; }
        public int photo_width { get; set; }
        public int photo_height { get; set; }
        public string thumb_url { get; set; }
        public string thumb_width { get; set; }
        public string thumb_height { get; set; }

        public string title { get; set; }
        public string description { get; set; }
        public string caption { get; set; }
        public object reply_markup { get; set; }
        public object input_message_content { get; set; }

    }
}