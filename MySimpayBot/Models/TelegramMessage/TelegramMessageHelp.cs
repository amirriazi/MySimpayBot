using Shared.WebService;
using System;
using System.Collections.Generic;
using Telegram.Bot.Types;


namespace Models
{
    public partial class TelegramMessage
    {
        private void HelpShow()
        {
            Def.MyDbLogger.method = Utils.GetCurrentMethod();
            var helpResources = HelpGetResources();
            var inlineK = new List<InlineKeyboardButton[]>();
            var colKey = new List<InlineKeyboardButton>();
            var emoji = "";
            foreach (var helpResource in helpResources)
            {
                switch (helpResource.helpType)
                {
                    case Telegram.Bot.Types.Enums.MessageType.TextMessage:
                        emoji = "📖";
                        break;
                    case Telegram.Bot.Types.Enums.MessageType.PhotoMessage:
                        emoji = "📷";
                        break;
                    case Telegram.Bot.Types.Enums.MessageType.VoiceMessage:
                    case Telegram.Bot.Types.Enums.MessageType.AudioMessage:
                        emoji = "🎧";
                        break;
                    case Telegram.Bot.Types.Enums.MessageType.VideoMessage:
                        emoji = "🎬";
                        break;
                    case Telegram.Bot.Types.Enums.MessageType.DocumentMessage:
                        emoji = "📖";
                        break;
                    case Telegram.Bot.Types.Enums.MessageType.ServiceMessage:
                        emoji = "📖";
                        break;

                    default:
                        emoji = "🔑";
                        break;
                }
                if (helpResource.helpType == Telegram.Bot.Types.Enums.MessageType.ServiceMessage)
                {
                    colKey.Add(new InlineKeyboardButton
                    {
                        Text = $"{emoji}{helpResource.showName}",
                        Url = $"{helpResource.fileId}",
                    });

                }
                else
                {
                    colKey.Add(new InlineKeyboardButton
                    {
                        Text = $"{emoji}{helpResource.showName}",
                        CallbackData = $"{SimpaySectionEnum.Help}:showhelp:{helpResource.helpId}"
                    });

                }
                inlineK.Add(colKey.ToArray());
                colKey.Clear();
            }

            var markup = new Telegram.Bot.Types.ReplyMarkups.InlineKeyboardMarkup();
            markup.InlineKeyboard = inlineK.ToArray();
            telegramAPI.send("لطفا بخش راهنمای مورد نظر را از میان فهرست زیر انتخاب نمایید:", markup);

        }
        private void HelpCallback(string data)
        {
            Def.MyDbLogger.method = Utils.GetCurrentMethod();
            var action = data.Split(':')[1];
            var helpId = "";
            do
            {
                switch (action.ToLower())
                {
                    case "showhelp":
                        helpId = data.Split(':')[2];
                        HelpSelect(helpId);
                        break;
                    default:
                        break;
                }
            } while (false);
        }

        private void HelpSelect(string helpId)
        {
            Def.MyDbLogger.method = Utils.GetCurrentMethod();
            do
            {
                var helpResources = HelpGetResources();
                var help = helpResources.Find(delegate (HelpingResource HR)
                {
                    return (HR.helpId == helpId);
                });
                if (help == null)
                {
                    sendMenu(message: "راهنمای مورد نظر یافت نشد");
                    break;
                }

                telegramAPI.fileToSend = new FileToSend { FileId = help.fileId };
                telegramAPI.caption = help.showName;
                telegramAPI.send(help.helpType);

            } while (false);


        }
        private List<HelpingResource> HelpGetResources()
        {
            var help = new List<HelpingResource>();
            help.Add(new HelpingResource
            {
                helpId = "autocharge",
                fileId = "BAADAwADAQADPmTBTUeNWlF_QZ3QAg",
                helpType = Telegram.Bot.Types.Enums.MessageType.VideoMessage,
                showName = "خرید شارژ"
            });

            help.Add(new HelpingResource
            {
                helpId = "trainticket",
                fileId = "BAADAQADAgADPmTBTdcd9W6NOO6DAg",
                helpType = Telegram.Bot.Types.Enums.MessageType.VideoMessage,
                showName = "خرید بلیط قطار"
            });

            help.Add(new HelpingResource
            {
                helpId = "bills",
                fileId = "BAADAQADAQADOdnITd3quk7eD0UGAg",
                helpType = Telegram.Bot.Types.Enums.MessageType.VideoMessage,
                showName = "پرداخت قبوض"
            });
            help.Add(new HelpingResource
            {
                helpId = "trafficfine",
                fileId = "BAADAQADBAADPmTBTenKOmG4arf-Ag",
                helpType = Telegram.Bot.Types.Enums.MessageType.VideoMessage,
                showName = "جرائم رانندگی"
            });
            help.Add(new HelpingResource
            {
                helpId = "giftCard",
                fileId = "http://simpay.ir/help/microsoft",
                helpType = Telegram.Bot.Types.Enums.MessageType.ServiceMessage,
                showName = "Microsoft Gift Card"
            });
            help.Add(new HelpingResource
            {
                helpId = "giftCard",
                fileId = "http://simpay.ir/help/googleplay",
                helpType = Telegram.Bot.Types.Enums.MessageType.ServiceMessage,
                showName = "GooglePlay Gift Card"
            });
            help.Add(new HelpingResource
            {
                helpId = "giftCard",
                fileId = "http://simpay.ir/help/xbox",
                helpType = Telegram.Bot.Types.Enums.MessageType.ServiceMessage,
                showName = "XBox Gift Card"
            });
            help.Add(new HelpingResource
            {
                helpId = "giftCard",
                fileId = "http://simpay.ir/help/itunes",
                helpType = Telegram.Bot.Types.Enums.MessageType.ServiceMessage,
                showName = "iTunes Gift Card"
            });
            help.Add(new HelpingResource
            {
                helpId = "giftCard",
                fileId = "http://simpay.ir/help/amazon",
                helpType = Telegram.Bot.Types.Enums.MessageType.ServiceMessage,
                showName = "Amazon Gift Card"
            });
            help.Add(new HelpingResource
            {
                helpId = "giftCard",
                fileId = "http://simpay.ir/help/playstation",
                helpType = Telegram.Bot.Types.Enums.MessageType.ServiceMessage,
                showName = "PlayStation Gift Card"
            });
            return help;
        }

        private class HelpingResource
        {

            public string helpId { get; set; }
            public string fileId { get; set; }
            public Telegram.Bot.Types.Enums.MessageType helpType { get; set; }
            public string showName { get; set; }
            public string url { get; set; }


        }
    }
}
