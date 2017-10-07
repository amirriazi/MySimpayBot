using Models;
using Shared.WebService;
using System;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.InlineQueryResults;
using Telegram.Bot.Types.ReplyMarkups;

namespace myTelegramApplication
{
    public class TelegramAPI
    {
        //public string apiToken { get; set; } = "355253393:AAFNsuNu_5n56r2ZoOajeuZ-tS0lIzhuIFs";
        //public string apiToken { get; set; } = "133954921:AAH6iKm_QiR2wihseUoMXviewVTaFuL2F-M";
        public Models.GeneralResultAction resultAction { get; set; }

        public dynamic chatId { get; set; }
        public MessageType messageType { get; set; }
        public int replyToMessageId { get; set; } = 0;
        public string text { get; set; }
        public int messageId { get; set; } = 0;
        public bool disableWebPagePreview { get; set; } = false;
        public bool disableNotification { get; set; } = false;
        public ParseMode parseMode { get; set; } = ParseMode.Default;
        public IReplyMarkup replyMarkup { get; set; }
        public FileToSend fileToSend { get; set; }
        public string fileId { get; set; }
        public string caption { get; set; }
        public int duration { get; set; } = 0;
        public int width { get; set; } = 0;
        public int height { get; set; } = 0;
        public string performer { get; set; }
        public string title { get; set; }
        public float latitude { get; set; }
        public float longitude { get; set; }
        public string address { get; set; }
        public string foursquareId { get; set; }
        public string phoneNumber { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string callBackId { get; set; }
        public string inlineQueryId { get; set; }
        public InlineQueryResult[] inlineQueryResult { get; set; }
        public Telegram.Bot.TelegramBotClient botAPI { get; set; }
        public TelegramAPI()
        {
        }

        public TelegramAPI(long theChatId)
        {
            chatId = theChatId;
            botAPI = new TelegramBotClient(ProjectValues.telegramApiToken);
        }

        public TelegramAPI(string theChatId)
        {
            chatId = theChatId;
            botAPI = new TelegramBotClient(ProjectValues.telegramApiToken);
        }

        public void sendTypingStatus()
        {

            botAPI.SendChatActionAsync(chatId, ChatAction.Typing).GetAwaiter();
        }
        public void sendUploadingVideoStatus()
        {

            botAPI.SendChatActionAsync(chatId, ChatAction.UploadVideo).GetAwaiter();
        }
        public void sendUploadingPhotoStatus()
        {

            botAPI.SendChatActionAsync(chatId, ChatAction.UploadPhoto).GetAwaiter();
        }
        public void sendUploadingAudioStatus()
        {

            botAPI.SendChatActionAsync(chatId, ChatAction.UploadAudio).GetAwaiter();
        }
        public void sendUploadingDocumentStatus()
        {

            botAPI.SendChatActionAsync(chatId, ChatAction.UploadDocument).GetAwaiter();
        }




        public bool send(MessageType theMessagetype)
        {
            messageType = theMessagetype;
            return sendMessageToChat();
        }

        public bool send(string message)
        {
            text = message;
            messageType = MessageType.TextMessage;
            return sendMessageToChat();
        }
        public bool send(string message, IReplyMarkup theReplyMarkup)
        {
            text = message;
            messageType = MessageType.TextMessage;
            replyMarkup = theReplyMarkup;
            return sendMessageToChat();
        }
        public bool send(MessageType theMessagetype, IReplyMarkup theReplyMarkup)
        {
            messageType = theMessagetype;
            replyMarkup = theReplyMarkup;
            return sendMessageToChat();
        }
        public bool sendTextAsync(string text, string theChatId)
        {
            resultAction = new Models.GeneralResultAction();
            try
            {
                var result = botAPI.SendTextMessageAsync(chatId: theChatId, text: text, disableNotification: disableNotification, parseMode: parseMode).GetAwaiter().GetResult();
                return true;
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("Forbidden"))
                {
                    var user = new Models.UserModel();
                    user.reportBlocked(Convert.ToInt64(theChatId));
                }

                return false;
            }
        }
        private bool sendMessageToChat()
        {
            var ans = true;
            resultAction = new Models.GeneralResultAction();

            var result = new Message();
            do
            {
                try
                {
                    switch (messageType)
                    {
                        case MessageType.TextMessage:
                            //var u = new UserModel(chatId);

                            //text = $"به {u.firstName}:{u.isNewUser} {text}";
                            result = botAPI.SendTextMessageAsync(chatId, text, disableNotification, disableNotification, replyToMessageId, replyMarkup, parseMode).GetAwaiter().GetResult();

                            Def.MyDbLogger.action = "SendText";
                            Def.MyDbLogger.playLoad = text;
                            Def.MyDbLogger.reportLog();
                            break;
                        case MessageType.PhotoMessage:
                            result = botAPI.SendPhotoAsync(chatId, fileToSend, caption, disableNotification, replyToMessageId, replyMarkup).GetAwaiter().GetResult();
                            break;
                        case MessageType.AudioMessage:
                            result = botAPI.SendAudioAsync(chatId, fileToSend, duration, performer, title, disableNotification, replyToMessageId, replyMarkup).GetAwaiter().GetResult();
                            break;
                        case MessageType.VideoMessage:
                            result = botAPI.SendVideoAsync(chatId, fileToSend, duration, caption, disableNotification, replyToMessageId, replyMarkup).GetAwaiter().GetResult();
                            break;
                        case MessageType.VoiceMessage:
                            result = botAPI.SendVoiceAsync(chatId, fileToSend, duration, disableNotification, replyToMessageId, replyMarkup).GetAwaiter().GetResult();
                            break;
                        case MessageType.DocumentMessage:
                            result = botAPI.SendDocumentAsync(chatId, fileToSend, caption, disableNotification, replyToMessageId, replyMarkup).GetAwaiter().GetResult();
                            break;
                        case MessageType.StickerMessage:
                            result = botAPI.SendStickerAsync(chatId, fileToSend, disableNotification, replyToMessageId, replyMarkup).GetAwaiter().GetResult();
                            break;
                        case MessageType.LocationMessage:
                            result = botAPI.SendLocationAsync(chatId, latitude, longitude, disableNotification, replyToMessageId, replyMarkup).GetAwaiter().GetResult();
                            break;
                        default:
                            Log.Error("Unrecognizable message type", 0);
                            resultAction = new Models.GeneralResultAction("sendMessage", true, "Unrecognizable message type ");
                            ans = false;
                            break;
                    }

                }
                catch (Exception ex)
                {

                    var errText = $"send to:{chatId} -{ex.Source}  {ex.Message} text={text ?? "empty"}";
                    if (errText.Contains("bot was blocked by the user"))
                    {
                        var user = new Models.UserModel(chatId);
                        user.reportBlocked();
                    }
                    resultAction = new Models.GeneralResultAction("sendMessage", true, errText);
                    Log.Error(errText, 0);
                    //botAPI.SendTextMessageAsync(ProjectValues.adminChatId, errText).GetAwaiter().GetResult();
                    ans = false;
                }
            } while (false);
            return ans;
        }

        public bool editText(int oldMessageId, string newText, IReplyMarkup newReplyMarkup = null)
        {
            messageId = oldMessageId;
            text = newText;
            replyMarkup = newReplyMarkup;
            return editTextMessage(false);
        }

        public bool editText(int oldMessageId, IReplyMarkup newReplyMarkup)
        {
            messageId = oldMessageId;
            replyMarkup = newReplyMarkup;
            return editTextMessage(true);
        }

        private bool editTextMessage(bool onlyMarkup)
        {
            var ans = true;


            var result = new Message();
            do
            {
                try
                {
                    if (onlyMarkup)
                    {
                        result = botAPI.EditMessageReplyMarkupAsync(chatId, messageId, replyMarkup).GetAwaiter().GetResult();
                    }
                    else
                    {
                        result = botAPI.EditMessageTextAsync(chatId, messageId, text, parseMode, disableWebPagePreview, replyMarkup).GetAwaiter().GetResult();
                    }

                }
                catch (Exception ex)
                {
                    if (ex.HResult != -2146233088) //ignor this error: Bad Request: message is not modified
                        Log.Error(ex.HResult.ToString() + ": " + ex.Message, 0);


                    ans = false;
                }
            } while (false);
            return ans;


        }
        public void answerCallBack(string callbackQueryId, string text = null)
        {


            callBackId = callbackQueryId;
            botAPI.AnswerCallbackQueryAsync(callbackQueryId: callBackId, text: text, showAlert: false, url: null, cacheTime: 3);

        }
        public void answerCallBack()
        {
            //botAPI.AnswerCallbackQueryAsync(callBackId,texts).GetAwaiter().GetResult();

            try
            {
                botAPI.AnswerCallbackQueryAsync(callbackQueryId: callBackId, text: "", showAlert: false, url: null, cacheTime: 0).GetAwaiter().GetResult();
            }
            catch (Exception ex)
            {

                Log.Error($"{ex.Message} callbackQueryId={callBackId}", 0);
            }
        }

        public void answerCallBack(string text)
        {
            //botAPI.AnswerCallbackQueryAsync(callBackId,texts).GetAwaiter().GetResult();

            var result = botAPI.AnswerCallbackQueryAsync(callbackQueryId: callBackId, text: text, showAlert: true, url: null, cacheTime: 3).GetAwaiter().GetResult();


        }
        public void answerInlineQuery(string theInlineQueryId, InlineQueryResult[] result)
        {
            inlineQueryId = theInlineQueryId;
            inlineQueryResult = result;
            answerInlineQuery();

        }
        public void answerInlineQuery(string nextOffset = null)
        {
            try
            {

                var botResult = botAPI.AnswerInlineQueryAsync(inlineQueryId: inlineQueryId, results: inlineQueryResult, nextOffset: nextOffset).GetAwaiter().GetResult();
            }
            catch (Exception ex)
            {

                Log.Error(ex.Message, 0);
            }

        }

        public Telegram.Bot.Types.File getFile()
        {

            var File = botAPI.GetFileAsync(fileId).GetAwaiter().GetResult();
            return File;
        }
        public string getFileUrl()
        {
            var Url = "";
            do
            {
                var File = this.getFile();
                if (File == null)
                    break;
                Url = $@"https://api.telegram.org/file/bot{ProjectValues.telegramApiToken}/{File.FilePath}";
            } while (false);
            return Url;
        }
        public BotInfo getMe()
        {

            string ans = Utils.ConvertClassToJson(botAPI.GetMeAsync().GetAwaiter().GetResult());
            var userInfo = Utils.ConvertJsonToClass<BotInfo>(ans);

            return userInfo;
        }

        public class BotInfo
        {
            public long id { get; set; } = 0;
            public string first_name { get; set; } = "";
            public string last_name { get; set; } = "";
            public string username { get; set; } = "";
        }

    }
}