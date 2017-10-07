using Shared.WebService;
using System;
using System.Collections.Generic;
using System.Linq;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using myTelegramApplication;
using Newtonsoft.Json;

namespace Models
{
    public partial class TelegramMessage
    {

        #region CommonsPart
        public static UserModel thisUser { get; set; }

        public static long chatId { get; set; }
        public AltonTechBotManager.Message message { get; set; }
        public AltonTechBotManager.Message channelPost { get; set; }
        public AltonTechBotManager.InlineQuery inlineQuery { get; set; }
        public AltonTechBotManager.CallbackQuery callbackQuery { get; set; }
        public long updateId { get; set; }
        public string callBackId { get; set; }
        public string inlineQueryId { get; set; }

        public MessageType messageType { get; set; }


        public long introduceBy { get; set; }

        public TelegramAPI telegramAPI { get; set; }

        private CurrentAction currentAction { get; set; }

        public TelegramMessage()
        {

        }

        //public TelegramMessage(AltonTechBotManager.Update U)
        public TelegramMessage(AltonTechBotManager.Update U)
        {
            do
            {
                var updateTmpClass = U.IgnoreEmptyPropertyEx();
                Def.MyDbLogger.playLoad = Utils.ConvertClassToJson(updateTmpClass);
                Def.MyDbLogger.action = "TelegramMessageUpdate";
                Def.MyDbLogger.reportLog();


                if (U?.Message != null)
                {
                    chatId = U.Message.From.ID;
                    thisUser = new UserModel(chatId);
                    telegramAPI = new TelegramAPI(chatId);
                    SimpayCore.chatId = chatId;

                    U.Message.Text = Converter.ToEnglishString(U.Message.Text);

                    telegramAPI = new TelegramAPI(chatId);
                    currentAction = new CurrentAction(chatId);

                    message = U.Message;
                    updateId = U.Id;
                    messageType = message.Type;
                    introduceBy = 0;


                    checkUserExistence(message.From);

                }
                else if (U?.EditedMessage != null)
                {

                    chatId = U.EditedMessage.From.ID;
                    thisUser = new UserModel(chatId);
                    telegramAPI = new TelegramAPI(chatId);
                    SimpayCore.chatId = chatId;

                    U.EditedMessage.Text = Converter.ToEnglishString(U.EditedMessage.Text);

                    telegramAPI = new TelegramAPI(chatId);
                    currentAction = new CurrentAction(chatId);

                    message = U.EditedMessage;
                    updateId = U.Id;
                    messageType = message.Type;
                    introduceBy = 0;


                    checkUserExistence(message.From);

                }
                else if (U?.CallbackQuery != null)
                {
                    var callBack = U.CallbackQuery;
                    chatId = callBack.From.ID;
                    thisUser = new UserModel(chatId);
                    SimpayCore.chatId = chatId;

                    telegramAPI = new TelegramAPI(chatId);
                    currentAction = new CurrentAction(chatId);

                    callbackQuery = callBack;
                    updateId = U.Id;
                    callBackId = callBack.ID;

                    if (callBack.Message != null)
                    {
                        messageType = callBack.Message.Type;
                    }


                    telegramAPI.callBackId = callbackQuery.ID;
                    telegramAPI.answerCallBack();


                }
                else if (U?.InlineQuery != null)
                {
                    chatId = U.InlineQuery.From.ID;
                    thisUser = new UserModel(chatId);
                    SimpayCore.chatId = chatId;

                    telegramAPI = new TelegramAPI(chatId);
                    currentAction = new CurrentAction(chatId);

                    U.InlineQuery.Query = Converter.ToEnglishString(U.InlineQuery.Query);

                    updateId = U.Id;
                    inlineQuery = U.InlineQuery;
                    checkUserExistence(U?.InlineQuery.From);
                }
                else if (U?.ChannelPost != null)
                {
                    chatId = U.ChannelPost.Chat.ID;
                    thisUser = new UserModel(chatId);
                    currentAction = new CurrentAction(chatId);
                    channelPost = U.ChannelPost;
                    telegramAPI = new TelegramAPI(chatId);
                }
                else
                {

                    Log.Fatal("Can not recognize the Update object!(" + Utils.ConvertClassToJson(U) + ")", 0);
                }
                reportUpdate(Utils.ConvertClassToJson(U));
            } while (false);


        }
        public void actionOnUpdate()
        {
            do
            {
                Def.MyDbLogger.method = "actionOnUpdate";
                try
                {

                    if (message != null)
                    {
                        actionOnMessage();
                    }
                    else if (callbackQuery != null)
                    {
                        actionOnCallbackQuery();
                    }
                    else if (inlineQuery != null)
                    {
                        actionOnInlineQuery();
                    }
                    else if (channelPost != null)
                    {
                        //telegramAPI.send("این ربات در گروه نمیتواند فعالیت نماید. لطفا آن را از گروه حذف نمایید!");
                        break;
                    }

                }
                catch (Exception ex)
                {
                    Log.Fatal(ex.Message, 0);
                    if (currentAction != null)
                    {
                        currentAction.remove();
                    }
                    //telegramAPI.send(ex.Message);
                    sendMenu(message: "متاسفانه بعلت بروز مشکل، ادامه فعالیت جاری متوقف شد. خواهشمند است از ابتدا فرایند را شروع نمایید. ");
                }

            } while (false);

        }


        private void actionOnMessage()
        {
            //TelegramActions_X.sendTypingStatus(message.From.ID);



            do
            {
                //private static bool isRightMessageType(AltonTechBotManager.Message message)
                if (!isRightMessageType(messageType))
                {
                    //TelegramActions_X.SendMessage(chatId, "Cannot process this format of message");
                    telegramAPI.send("Cannot process this format of message");

                    break;
                }
                if (actionOnFile())
                {
                    break;
                }
                if (!String.IsNullOrEmpty(message.Text))
                {
                    if (AdminCommands())
                    {
                        break;
                    }

                    if (actionOnReservedCommands())
                    {
                        break;
                    }

                    if (actionOnCurrentAction())
                    {
                        break;
                    }

                    if (actionOnMenu())
                    {
                        break;
                    }
                }

                //If reaches here means there is no recognizable command
                var Msg = "";
                Msg += "پیام ورودی شناخته نشد!" + (char)10 + (char)13;
                Msg += "لطفا از فهرست زیر دستور مورد نظر را انتخاب نمایید.";
                currentAction.remove();
                sendMenu(message: Msg);

            } while (false);


        }
        private bool actionOnFile()
        {
            var ans = false;
            do
            {
                if (message.Type == MessageType.AudioMessage
                    || message.Type == MessageType.DocumentMessage
                    || message.Type == MessageType.PhotoMessage
                    || message.Type == MessageType.StickerMessage
                    || message.Type == MessageType.VideoMessage
                    || message.Type == MessageType.VoiceMessage)

                {
                    if (currentAction.section == SimpaySectionEnum.BillPaymentProduct && currentAction.action == "billid" && (message.Document != null || message.Photo != null))
                    {
                        ans = true;
                        var fileId = "";
                        if (message.Document != null)
                        {
                            fileId = message.Document.ID;
                        }
                        else if (message.Photo != null)
                        {
                            fileId = message.Photo[1].ID; ;
                        }

                        BillPaymentBarcode(currentAction.parameter, fileId);
                    }
                    else if (thisUser.isAdmin)
                    {
                        ans = true;
                        ShowUploadedFileSpecification();
                    }


                }


            } while (false);
            return ans;
        }

        private void actionOnCallbackQuery()
        {
            // put answerCallBack here so if the error occurs the telegram wont retry the call back
            //TelegramActions_x.answerCallBack(callbackQuery.ID, "");
            telegramAPI.answerCallBack(callbackQuery.ID, "");
            do
            {
                var data = callbackQuery.Data;
                var actionName = data.Split(':')[0];
                if (actionName == "general")
                {

                    var actionValue = data.Split(':')[1];
                    if (actionValue == "cancel")
                    {
                        currentAction.remove();
                        sendMenu(message: "Current action has been canceled!");
                    }
                    else if (actionValue == "showmenu")
                    {
                        currentAction.remove();
                        sendMenu();
                    }
                }
                else if (actionName == Convert.ToString(SimpaySectionEnum.Help))
                {
                    HelpCallback(data);
                }
                else if (actionName == Convert.ToString(SimpaySectionEnum.Activation))
                {
                    activateUserMessage();
                }
                else if (actionName == Convert.ToString(SimpaySectionEnum.Unactivation))
                {
                    UnactivateUserCallBack(data);
                }

                else if (actionName == Convert.ToString(SimpaySectionEnum.AutoCharge))
                {
                    AutoChargeCallBackQuery(data);
                }
                else if (actionName == Convert.ToString(SimpaySectionEnum.CinemaTicket))
                {
                    CinemaTicketCallBackQuery(data);
                }

                else if (actionName == Convert.ToString(SimpaySectionEnum.Drama))
                {
                    DramaCallBackQuery(data);
                }

                else if (actionName == Convert.ToString(SimpaySectionEnum.TrafficFinesProduct))
                {
                    TrafficFineCallBackQuery(data);
                }
                else if (actionName == Convert.ToString(SimpaySectionEnum.MciMobileBill))
                {
                    MciMobileBillCallBackQuery(data);
                }
                else if (actionName == Convert.ToString(SimpaySectionEnum.FixedLineBill))
                {
                    FixedLineBillCallBackQuery(data);
                }

                else if (actionName == Convert.ToString(SimpaySectionEnum.GasBill))
                {
                    GasBillCallBackQuery(data);
                }
                else if (actionName == Convert.ToString(SimpaySectionEnum.ElectricityBill))
                {
                    ElectricityBillCallBackQuery(data);
                }


                else if (actionName == Convert.ToString(SimpaySectionEnum.BillPaymentProduct))
                {
                    BillPaymentCallBackQuery(data);
                }
                else if (actionName == Convert.ToString(SimpaySectionEnum.XpinProduct))
                {
                    XpinGiftCardCallBackQuery(data);
                }
                else if (actionName == Convert.ToString(SimpaySectionEnum.History))
                {
                    CoreHistoryCallBack(data);
                }
                else if (actionName == Convert.ToString(SimpaySectionEnum.PinCharge))
                {
                    PinChargeCallBack(data);
                }
                else if (actionName == Convert.ToString(SimpaySectionEnum.Calendar))
                {
                    CalendarCallBack(data);
                }
                else if (actionName == Convert.ToString(SimpaySectionEnum.BusTicket))
                {
                    BusTicketCallBack(data, callbackQuery.Message.ID);
                }
                else if (actionName == Convert.ToString(SimpaySectionEnum.TrainTicket))
                {
                    TrainTicketCallBack(data);
                }
                else if (actionName == Convert.ToString(SimpaySectionEnum.AirplaneTicket))
                {
                    AirplaneTicketCallBack(data);
                }
                else if (actionName == Convert.ToString(SimpaySectionEnum.TMTNServices))
                {
                    TMTNCallBackQuery(data);
                }

                else if (actionName == Convert.ToString(SimpaySectionEnum.Payment))
                {
                    PaymentCallBackQuery(data);
                }







                else if (actionName == "showinfo")
                {
                    var actionValue = data.Split(':')[1];
                    if (String.IsNullOrEmpty(actionValue))
                    {
                        currentAction.remove();
                        sendMenu(message: "Poll Id should be passed to this state!");
                        break;
                    }


                }

            } while (false);

        }

        private void actionOnInlineQuery()
        {


        }

        private void checkUserExistence(AltonTechBotManager.User userInfo)
        {


            if (thisUser.userId == 0)
            {
                thisUser.firstName = userInfo.FirstName;
                thisUser.lastName = userInfo.LastName;
                thisUser.userName = userInfo.UserName;
                thisUser.introduceBy = 0;
                thisUser.invitationCode = "";
                thisUser.reportUser();
            };


        }
        private void reportUpdate(string update)
        {
            do
            {
                int messageId = 0;
                MessageType messageType = MessageType.UnknownMessage;
                if (message != null)
                {
                    messageId = message.ID;
                    messageType = message.Type;
                }
                if (updateId == 0)
                {
                    break;
                }
                var result = DataBase.ReportUpdate(updateId, messageId, (int)messageType, chatId, update);
                if (result.ReturnCode != 1 || result.SPCode != 1)
                {
                    Log.Error($"Update Save error: {result.Text}", 0);
                }

            } while (false);
        }



        private Menu initialMenu(int selectedMenuId = 0)
        {
            var menu = new Menu(chatId, selectedMenuId);
            if (menu.menuItem == null)
            {
                return null;
            }
            return menu;

        }

        private void sendMenu(int selectedMenuId = 0, string message = "")
        {
            do
            {

                var menu = initialMenu(selectedMenuId);
                if (menu == null)
                    break;

                var lKeyB = new List<KeyboardButton[]>();
                var colKey = new List<KeyboardButton>();
                for (var i = 0; i < menu.menuItem.Count; i += 1)
                {

                    if (menu.menuItem[i].newLine)
                    {
                        if (colKey.Count > 0)
                        {
                            lKeyB.Add(colKey.ToArray());
                            colKey.Clear();
                        }
                    }
                    colKey.Add(new KeyboardButton(menu.menuItem[i].menuCaption));
                    if (i == menu.menuItem.Count - 1)
                    {
                        lKeyB.Add(colKey.ToArray());
                    }
                }

                if (menu.hasParent)
                {
                    lKeyB.Add(new[] { new KeyboardButton("بازگشت به فهرست قبلی") });
                }

                var keyB = lKeyB.ToArray();

                var r = new Telegram.Bot.Types.ReplyMarkups.ReplyKeyboardMarkup();
                r.Keyboard = keyB;
                r.OneTimeKeyboard = true;
                r.ResizeKeyboard = true;

                if (message == "")
                    message = "لطفا از فهرست زیر عملیات خود را انتخاب نمایید. ";


                //var t = TelegramActions_x.SendMessage(chatId: chatId, Message: message, replyMarkup: r);

                //var TMA = new TelegramAPI(chatId, MessageType.TextMessage)
                //{
                //    text = message,
                //    replyMarkup = r
                //};
                telegramAPI.send(message, r);

            } while (false);

        }


        private bool actionOnCurrentAction()
        {
            var showMenu = false;
            var ans = true;
            do
            {

                if (String.IsNullOrEmpty(currentAction.action))
                {
                    ans = false;
                    break;
                }
                else if (currentAction.section == SimpaySectionEnum.AutoCharge)
                {
                    AutoChargeInfoUpdate(currentAction.action.ToLower(), Converter.ToEnglishString(message.Text), currentAction.parameter);
                    break;
                }
                else if (currentAction.section == SimpaySectionEnum.TrafficFinesProduct)
                {
                    TrafficFineInfoUpdate(currentAction.action.ToLower(), Converter.ToEnglishString(message.Text), currentAction.parameter);
                    break;
                }
                else if (currentAction.section == SimpaySectionEnum.MciMobileBill)
                {
                    MciMobileBillInfoUpdate(currentAction.action.ToLower(), Converter.ToEnglishString(message.Text), currentAction.parameter);
                    break;
                }
                else if (currentAction.section == SimpaySectionEnum.FixedLineBill)
                {
                    FixedLineBillInfoUpdate(currentAction.action.ToLower(), Converter.ToEnglishString(message.Text), currentAction.parameter);
                    break;
                }

                else if (currentAction.section == SimpaySectionEnum.GasBill)
                {
                    GasBillInfoUpdate(currentAction.action.ToLower(), Converter.ToEnglishString(message.Text), currentAction.parameter);
                    break;
                }
                else if (currentAction.section == SimpaySectionEnum.ElectricityBill)
                {
                    ElectricityBillInfoUpdate(currentAction.action.ToLower(), Converter.ToEnglishString(message.Text), currentAction.parameter);
                    break;
                }

                else if (currentAction.section == SimpaySectionEnum.BillPaymentProduct)
                {
                    BillPaymentInfoUpdate(currentAction.action.ToLower(), Converter.ToEnglishString(message.Text), currentAction.parameter);
                    break;
                }
                else if (currentAction.section == SimpaySectionEnum.Activation)
                {
                    activationUserProcess(currentAction.action.ToLower(), message.Text);
                    break;
                }
                else if (currentAction.section == SimpaySectionEnum.BusTicket)
                {
                    BusTicketCurrenAction(currentAction.action.ToLower(), currentAction.parameter, message.Text);
                    break;

                }
                else if (currentAction.section == SimpaySectionEnum.TrainTicket)
                {
                    TrainTicketVerifyUserEntryText(currentAction.action.ToLower(), Converter.ToEnglishString(message.Text), currentAction.parameter);
                    break;
                }
                else if (currentAction.section == SimpaySectionEnum.AirplaneTicket)
                {
                    AirplaneTicketVerifyUserEntryText(currentAction.action.ToLower(), Converter.ToEnglishString(message.Text), currentAction.parameter);
                    break;
                }
                else if (currentAction.section == SimpaySectionEnum.TMTNServices)
                {
                    TMTNVerifyUserEntry(currentAction.action.ToLower(), Converter.ToEnglishString(message.Text), currentAction.parameter);
                    break;
                }
                else if (currentAction.section == SimpaySectionEnum.MTNServices)
                {
                    MTNVerifyUserEntry(currentAction.action.ToLower(), Converter.ToEnglishString(message.Text), currentAction.parameter);
                    break;
                }
                else if (currentAction.section == SimpaySectionEnum.Payment)
                {
                    //PaymentInputDiscount(message.Text, Convert.ToInt32(currentAction.parameter));
                    PaymentUpdateInfo(currentAction.action.ToLower(), message.Text, Convert.ToInt32(currentAction.parameter));
                    break;
                }
                else if (currentAction.section == SimpaySectionEnum.CinemaTicket)
                {
                    CinemaTicketEntry(currentAction.action.ToLower(), message.Text);
                    break;
                }
                else if (currentAction.section == SimpaySectionEnum.Drama)
                {
                    DramaEntry(currentAction.action.ToLower(), Converter.ToEnglishString(message.Text), Convert.ToInt64(currentAction.parameter));
                    break;
                }





                else
                {
                    telegramAPI.send("وضعیت جاری شما در سامانه مشخص نیست!");
                    showMenu = true;
                }

            } while (false);
            if (showMenu)
            {
                currentAction.remove();
                sendMenu();
            }
            return ans;
        }

        private bool actionOnMenu(string forcedMenuText = "")
        {
            var ans = true;
            do
            {
                var allMenu = new Menu(chatId);
                if (allMenu.menuItem == null)
                    break;

                var menuCaption = message.Text.Trim();
                if (!String.IsNullOrEmpty(forcedMenuText))
                    menuCaption = forcedMenuText;

                var selectedResult = allMenu.menuItem.Find(delegate (Menu.MenuItem MI)
                {
                    return (MI.menuCaption.Trim() == Converter.ToEnglishString(menuCaption));
                });

                if (selectedResult == null)
                {
                    ans = false;
                    break;
                }

                Def.MyDbLogger.action = "Menu";
                Def.MyDbLogger.playLoad = $"Menu={selectedResult?.menuKey}";
                Def.MyDbLogger.reportLog();

                setUserMenuSelection(selectedResult.menuId);
                //eyJmZHQiOiIyMDE3LTAzLTAxVDA5OjM4OjM3LjA1NyIsInRkdCI6IjIwNjctMDItMTdUMDk6Mzg6MzcuMDU3Iiwic2VhIjoxLCJwam4iOiJpbnRlcmZhY2UiLCJwcmEiOmZhbHNlLCJzaWQiOjExMjQzMDN9.NUNPS2VxK3JQZ3c4RDZSYlY0U29BRWlJMWRmQ2p3Mko4QmZVRXc1SzBlUjVpRi9BOUd0QjlEMTBDK0U4QXUzRA==.eyJ2bHUiOiJBNkRDNUNFQzhBQzAwQzVFQ0RGQzIxRTczNjk5NTVCRDEyMTIxNzdBNDNGQjQ1NUZCMkZGNUVGM0IyRTMwNUZBIn0=
                if (selectedResult?.menuKey == "products")
                {
                    if (!checkUserActivation())
                    {
                        userActivationAsking();
                    }
                }

                if (selectedResult?.menuKey == "cinemafilm")
                {
                    CinemaTicketRequestInfo();
                    break;
                }
                if (selectedResult?.menuKey == "cinemasearch")
                {
                    CinemaTicketRequestInfo("filmquery");
                    break;
                }
                if (selectedResult?.menuKey == "drama")
                {
                    DramaRequestInfo();
                    break;
                }

                if (selectedResult?.menuKey == "autocharge")
                {
                    AutoChargeRequestInfo();
                    break;
                }
                if (selectedResult?.menuKey == "trafficfine")
                {
                    TrafficFineRequestInfo();

                    break;
                }
                if (selectedResult?.menuKey == "mcimobilebill")
                {
                    MciMobileBillRequestInfo();
                    break;
                }
                if (selectedResult?.menuKey == "fixedlinebill")
                {
                    FixedLineBillRequestInfo();
                    break;
                }

                if (selectedResult?.menuKey == "gasbill")
                {
                    GasBillRequestInfo();
                    break;
                }
                if (selectedResult?.menuKey == "electricity")
                {
                    ElectricityBillRequestInfo();
                    break;
                }


                if (selectedResult?.menuKey == "billpayment")
                {
                    BillPaymentRequestInfo();
                    break;
                }
                if (selectedResult?.menuKey == "giftcard")
                {
                    XpinRequestInfo(XPIN.XpinCategoryEnum.GiftCard);
                    break;
                }
                if (selectedResult?.menuKey == "telecom")
                {
                    XpinRequestInfo(XPIN.XpinCategoryEnum.TeleCom);
                    break;
                }
                if (selectedResult?.menuKey == "untivirus")
                {
                    XpinRequestInfo(XPIN.XpinCategoryEnum.Univirus);
                    break;
                }

                if (selectedResult?.menuKey == "activation")
                {
                    if (!checkUserActivation())
                        activateUserMessage();
                    else
                        sendMenu(message: "شما قبلا ثبت نام کرده اید.");

                    break;
                }
                if (selectedResult?.menuKey == "unactivation")
                {
                    if (checkUserActivation())
                        UnactivateUserMessage();
                    else
                        sendMenu(message: "شما قبلا ثبت نام نکرده اید.");

                    break;
                }
                if (selectedResult?.menuKey == "help")
                {
                    HelpShow();
                }


                if (selectedResult?.menuKey == "busticket")
                {
                    //BusTicketProcess("done", 31);
                    BusTicketProcess();

                    break;
                }
                if (selectedResult?.menuKey == "trainticket")
                {
                    if (chatId == ProjectValues.adminChatId)
                    {
                        //TrainTicketRequestInfo("nationalcode", null, 79);
                        TrainTicketRequestInfo();
                    }
                    else
                    {
                        TrainTicketRequestInfo();
                    }

                    break;
                }
                if (selectedResult?.menuKey == "airplaneticket")
                {
                    if (chatId == ProjectValues.adminChatId)
                    {
                        //AirplaneTicketRequestInfo("endpassenger", null, 7);
                        AirplaneTicketRequestInfo();

                    }
                    else
                    {
                        AirplaneTicketRequestInfo();
                    }

                    break;
                }
                if (selectedResult?.menuKey == "tmtn")
                {
                    TMTNRequestInfo();
                    break;
                }

                if (selectedResult?.menuKey == "mtn")
                {
                    MTNRequestInfo();
                    break;
                }

                if (selectedResult?.menuKey == "apps")
                {
                    AppsShow();
                }
                if (selectedResult?.menuKey == "history")
                {
                    if (checkUserActivation())
                        CoreShowHistoryProducts();
                    else
                    {
                        telegramAPI.send("برای دیدن سوابق شما باید ابتدا در سامانه ثبت نام نمایید.");
                        //activateUserMessage();
                    }

                    break;

                }

                if (selectedResult?.menuKey == "unfinishes")
                {
                    if (checkUserActivation())
                        CoreShowHistoryUnfinishedPaymentInfo();
                    else
                    {
                        telegramAPI.send("برای دیدن سوابق شما باید ابتدا در سامانه ثبت نام نمایید.");
                        //activateUserMessage();
                    }
                    break;

                }
                if (selectedResult?.menuKey == "pincharge")
                {
                    PinChargeSelectChargeMessage();
                    break;
                }


                if (selectedResult?.menuKey == "invlink")
                {
                    InvitationSendLinkToFriends();
                    break;
                }
                if (selectedResult?.menuKey == "invruls")
                {
                    InvitationRuls();
                    break;
                }

                if (selectedResult?.menuKey == "friendstatus")
                {
                    InvitationGetStatus();
                    break;
                }

                if (selectedResult?.menuKey == "aboutus")
                {
                    sendAmoutUs();
                    break;
                }

                if (selectedResult?.menuKey == "contactus")
                {
                    ContactUsShowMessage();
                    break;
                }

                // if reachs here then set user menu selection 


                // check if there is sub menu
                var subMenu = new Menu(chatId, selectedResult.menuId);
                if (subMenu != null)
                {
                    sendMenu(selectedResult.menuId);
                }


            } while (false);


            return ans;
        }
        //private void sendLinktoFriends()
        //{

        //    var info = SimpayCore.GetInvitationInfo();

        //    var url = info.Link;
        //    string caption = "";
        //    caption += " \n ";
        //    caption += $"{info.Summery}";
        //    caption += " \n ";
        //    caption += $"{info.TextToShare}";
        //    caption += " \n \n ";
        //    caption += $"{info.InvitationCode}";
        //    caption += url;


        //    telegramAPI.send(message: "لطفا پست زیر را برای دوستان ارسال فرمایید.");
        //    telegramAPI.send(caption);
        //    //telegramAPI.fileToSend = new FileToSend
        //    //{
        //    //    Url = new Uri("http://simpay.ir/images/simpay-logo-1200.png"),
        //    //};
        //    //telegramAPI.caption = caption;
        //    //telegramAPI.send(MessageType.PhotoMessage);

        //}

        private bool actionOnReservedCommands()
        {
            var ans = false;
            do
            {
                if (message.Text.StartsWith("⛔ انصراف از عملیات جاری") || message.Text.StartsWith("/cancel", StringComparison.CurrentCultureIgnoreCase))
                {
                    currentAction.remove();

                    sendMenu(message: "عملیات جاری لغو گردید");
                    ans = true;
                    break;
                }
                else if (message.Text.StartsWith("بازگشت به فهرست قبلی"))
                {
                    currentAction.remove();
                    sendPreviousMenu();
                    ans = true;
                    break;

                }
                else if (message.Text.StartsWith("/start", StringComparison.CurrentCultureIgnoreCase))
                {
                    var arrStartCommand = Converter.ToEnglishString(message.Text).Split(' ');
                    if (arrStartCommand.Length > 1)
                    {
                        var cmdParam = Converter.ToEnglishString(arrStartCommand[1]);
                        if (cmdParam.IndexOf('-') >= 0)
                        {
                            CallBackfromBank(cmdParam);
                            ans = true;
                            break;
                        }
                        else if (Utils.isLong(cmdParam)) // means it send by invitation code
                        {
                            var user = new UserModel(chatId)
                            {
                                introduceBy = Convert.ToInt32(cmdParam),
                                linkAction = "BotInvitaion"
                            };
                            user.reportUser();
                        }
                        else
                        {
                            var user = new UserModel(chatId) // send by other invitation
                            {
                                introduceBy = 0,
                                linkAction = cmdParam.Length <= 20 ? cmdParam : cmdParam.Substring(0, 20)
                            };
                            user.reportUser();
                        }
                    }
                    ans = true;
                    sendMenu();

                }
                else if (message.Text.StartsWith("/resetall", StringComparison.CurrentCultureIgnoreCase))
                {
                    resetAllCurrentState();
                    ans = true;
                    sendMenu();

                }

                else if (message.Text.ToLower() == "#cal")
                {
                    Calendar(Navigation: CalendarNavigationSwitchEnum.ChangeMonth);
                    ans = true;
                }
                else if (message.Text.ToLower() == "#calfull")
                {
                    Calendar(Navigation: CalendarNavigationSwitchEnum.SelectYearsMonth);
                    ans = true;
                }

            } while (false);
            return ans;
        }
        private void sendPreviousMenu()
        {
            do
            {
                var result = DataBase.SetPreviousMenu(chatId);
                if (result.ReturnCode != 1 || result.SPCode != 1)
                {
                    Log.Error(result.Text, 0);
                    break;
                }
                sendMenu();

            } while (false);

        }
        private bool sendAmoutUs()
        {

            var text = "";
            text += "از اینکه ربات سیم پی را برای خرید و پرداخت های خود استفاده می کنید سپاس گزاریم.";
            text += "\n";
            text += "سیم پی نشان تجاری ثبت شده شرکت تجارت الکترونیک التون در حوزه تجارت الکترونیک و پرداخت است.";
            text += "\n";
            text += "در سیم پی همواره در تلاشیم که بهترین خدمات را ارائه دهیم چون معتقدیم که شما شایسته بهترین ها هستید.";
            text += "\n";
            text += "برای خرید کافی است رمز دوم کارت خود را داشته باشد. اطلاعات کارت بانکی شما تنها در صفحه بانک وارد شده و در هیچ کجا ذخیره نمی گردد. ";
            text += "\n";
            text += "لطفا هرگونه ایراد، انتقاد یا پیشنهاد خود را از راه های زیر با ما در میان بگذارید.";
            text += "\n";
            text += "\n";
            text += "ارتباط با بخش پشتیبانی: @simpayC";
            text += "\n";
            text += "وب سایت: http://simpay.ir ";
            text += "\n";
            text += "پست الکترونیک : info@simpay.ir ";
            text += "\n";
            text += "تلفن تماس:41576-021";
            text += "\n";
            text += "دورنگار : 88649084-021";
            text += "\n";
            text += "آدرس نماد اعتماد الکترونیکی : http://simpay.ir/enamad.html ";
            text += "\n";
            text += "-";

            //var inlineK = new InlineKeyboardButton[][]
            //{
            //    new InlineKeyboardButton[] {new InlineKeyboardButton() { Url="http://tabnak.ir",CallbackData="general:cancel", Text= "⛔ Cancel", CallbackGame=1   } }
            //};
            //var r = new Telegram.Bot.Types.ReplyMarkups.InlineKeyboardMarkup();
            //r.InlineKeyboard = inlineK.ToArray();
            //telegramAPI.send(text, r);
            sendMenu(message: text);
            return true;

        }


        private void setUserMenuSelection(int menuId)
        {
            try
            {
                var result = DataBase.reportUserSelection(chatId, menuId);
                if (result.ReturnCode != 1 || result.SPCode != 1)
                {
                    Log.Error($@"report user selection error: {result.Text}", 0);

                }


            }
            catch (Exception)
            {

            }
        }



        /// <summary>
        /// Check if this is a text message not voice, picture and ... other type of update
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        private bool isRightMessageType(MessageType messageType)
        {

            bool ans = (messageType == MessageType.TextMessage
                        || messageType == MessageType.AudioMessage
                        || messageType == MessageType.DocumentMessage
                        || messageType == MessageType.PhotoMessage
                        || messageType == MessageType.StickerMessage
                        || messageType == MessageType.VideoMessage
                        || messageType == MessageType.VoiceMessage);

            //bool ans = (messageType == Telegram.Bot.Types.Enums.MessageType.TextMessage
            //           || messageType == Telegram.Bot.Types.Enums.MessageType.AudioMessage);

            Log.Info("isRightMessageType returned " + ans, 0);

            return ans;
        }

        private Telegram.Bot.Types.ReplyMarkups.InlineKeyboardMarkup cancelInlineKeyboard()
        {
            var inlineK = new InlineKeyboardButton[][]
            {
                new InlineKeyboardButton[] {new InlineKeyboardButton() { CallbackData="general:cancel", Text= "⛔ Cancel" } }
            };
            var r = new Telegram.Bot.Types.ReplyMarkups.InlineKeyboardMarkup();
            r.InlineKeyboard = inlineK.ToArray();
            return r;

        }
        private Telegram.Bot.Types.ReplyMarkups.IReplyMarkup cancelButton()
        {
            var keyB = new KeyboardButton[][]
            {
                new KeyboardButton[] {new KeyboardButton("⛔ انصراف از عملیات جاری") }
            };
            var r = new Telegram.Bot.Types.ReplyMarkups.ReplyKeyboardMarkup();
            r.Keyboard = keyB;
            r.OneTimeKeyboard = true;
            r.ResizeKeyboard = true;

            return r;

        }

        private List<InlineKeyboardButton> GetPagingButtons(SimpaySectionEnum section, int currentPage, int maxPage)
        {
            var nextPage = currentPage + 1;
            var prePage = currentPage - 1;

            var colK = new List<InlineKeyboardButton>();
            if (currentPage > 1)
            {
                colK.Add(new InlineKeyboardButton
                {
                    Text = "<<",
                    CallbackData = $"{section}:paging:{1}"
                });
                colK.Add(new InlineKeyboardButton
                {
                    Text = "<",
                    CallbackData = $"{section}:paging:{prePage}"
                });
            }
            else
            {
                colK.Add(new InlineKeyboardButton
                {
                    Text = ".",
                    CallbackData = $"{section}:null:{0}"
                });
                colK.Add(new InlineKeyboardButton
                {
                    Text = ".",
                    CallbackData = $"{section}:null:{0}"
                });

            }
            colK.Add(new InlineKeyboardButton
            {
                Text = $@"page={currentPage}/{maxPage}",
                CallbackData = $"{section}:null:{0}"
            });

            if (currentPage < maxPage)
            {
                colK.Add(new InlineKeyboardButton
                {
                    Text = ">",
                    CallbackData = $"{section}:paging:{nextPage}"
                });
                colK.Add(new InlineKeyboardButton
                {
                    Text = ">>",
                    CallbackData = $"{section}:paging:{maxPage}"
                });
            }
            else
            {
                colK.Add(new InlineKeyboardButton
                {
                    Text = ".",
                    CallbackData = $"{section}:null:{0}"
                });
                colK.Add(new InlineKeyboardButton
                {
                    Text = ".",
                    CallbackData = $"{section}:null:{0}"
                });

            }

            return colK;
        }


        private InlineKeyboardButton[] paginButtons(int buttonCount, int pageNumber, int maxPage, string buttonExtraInfo)
        {

            if (maxPage <= 1)
                return null;



            //int Left = 0, Right = 0, Start = 0, Finish = 0;
            //do
            //{

            //    if (pageNumber > maxPage)
            //        pageNumber = maxPage;

            //    if (maxPage <= buttonCount)
            //    {
            //        Left = 1;
            //        Right = maxPage;
            //        Start = Left + 1;
            //        Finish = Right - 1;
            //        break;
            //    }

            //    if (maxPage > buttonCount)
            //    {
            //        Right = maxPage;
            //        Left = 1;
            //        if (pageNumber == maxPage)
            //        {
            //            Finish = pageNumber - 1;
            //            Start = Finish - buttonCount + 3;
            //            break;
            //        }

            //        if (pageNumber == maxPage - 1)
            //        {
            //            Finish = maxPage - 1;
            //            Start = Finish - buttonCount + 3;
            //            break;
            //        }
            //        if (pageNumber <)
            //            if (pageNumber - buttonCount + 3 > Left)
            //            {
            //                Finish = pageNumber;
            //                Start = Finish - buttonCount + 3;
            //                break;
            //            }

            //        Start = Left + 1;
            //        Finish = Start + buttonCount - 3;
            //        break;
            //    }

            //} while (false);

            //var kb = new List<InlineKeyboardButton>();
            //var keybordIdx = 0;


            //keybordIdx++;
            //kb.Add(new InlineKeyboardButton()
            //{
            //    Text = Left == pageNumber ? $"[{pageNumber}]" : $"{Left}",
            //    CallbackData = $"{buttonExtraInfo}:{Left}"
            //});

            //for (var page = Start; page <= Finish; page++)
            //{
            //    keybordIdx++;
            //    kb.Add(new InlineKeyboardButton()
            //    {
            //        Text = page == pageNumber ? $"[{page}]" : $"{page}",
            //        CallbackData = $"{buttonExtraInfo}:{page}"
            //    });

            //}

            //keybordIdx++;
            //kb.Add(new InlineKeyboardButton()
            //{
            //    Text = Right == pageNumber ? $"[{pageNumber}]" : $"{Right}",
            //    CallbackData = $"{buttonExtraInfo}:{Right}"
            //});

            //// for other empty buttons
            //for (var i = 1; i <= 5 - keybordIdx; i++)
            //{
            //    keybordIdx++;
            //    kb.Add(new InlineKeyboardButton()
            //    {
            //        Text = " . ",
            //        CallbackData = $"{buttonExtraInfo}:0"
            //    });

            //}

            //return kb.ToArray();




            var nextPage = pageNumber + 1;
            var prePage = pageNumber - 1;

            var colK = new List<InlineKeyboardButton>();
            if (pageNumber > 1)
            {
                colK.Add(new InlineKeyboardButton
                {
                    Text = "<<",
                    CallbackData = $"{buttonExtraInfo}:{1}"
                });
                colK.Add(new InlineKeyboardButton
                {
                    Text = "<",
                    CallbackData = $"{buttonExtraInfo}:{prePage}"
                });
            }
            else
            {
                colK.Add(new InlineKeyboardButton
                {
                    Text = ".",
                    CallbackData = $"{buttonExtraInfo}:{0}"
                });
                colK.Add(new InlineKeyboardButton
                {
                    Text = ".",
                    CallbackData = $"{buttonExtraInfo}:{0}"
                });

            }
            colK.Add(new InlineKeyboardButton
            {
                Text = $@"{pageNumber}/{maxPage}",
                CallbackData = $"{buttonExtraInfo}:{0}"
            });

            if (pageNumber < maxPage)
            {
                colK.Add(new InlineKeyboardButton
                {
                    Text = ">",
                    CallbackData = $"{buttonExtraInfo}:{nextPage}"
                });
                colK.Add(new InlineKeyboardButton
                {
                    Text = ">>",
                    CallbackData = $"{buttonExtraInfo}:{maxPage}"
                });
            }
            else
            {
                colK.Add(new InlineKeyboardButton
                {
                    Text = ".",
                    CallbackData = $"{buttonExtraInfo}:{0}"
                });
                colK.Add(new InlineKeyboardButton
                {
                    Text = ".",
                    CallbackData = $"{buttonExtraInfo}:{0}"
                });

            }

            return colK.ToArray();
        }
        private InlineKeyboardButton[] paginButtons_old(int buttonCount, int pageNumber, int maxPage, string buttonExtraInfo)
        {

            if (maxPage <= 1)
                return null;



            int Left = 0, Right = 0, Start = 0, Finish = 0;
            do
            {

                if (pageNumber > maxPage)
                    pageNumber = maxPage;

                if (maxPage <= buttonCount)
                {
                    Left = 1;
                    Right = maxPage;
                    Start = Left + 1;
                    Finish = Right - 1;
                    break;
                }

                if (maxPage > buttonCount)
                {
                    Right = maxPage;
                    Left = 1;
                    if (pageNumber == maxPage)
                    {
                        Finish = pageNumber - 1;
                        Start = Finish - buttonCount + 3;
                        break;
                    }

                    if (pageNumber == maxPage - 1)
                    {
                        Finish = maxPage - 1;
                        Start = Finish - buttonCount + 3;
                        break;
                    }

                    if (pageNumber - buttonCount + 3 > Left)
                    {
                        Finish = pageNumber;
                        Start = Finish - buttonCount + 3;
                        break;
                    }

                    Start = Left + 1;
                    Finish = Start + buttonCount - 3;
                    break;
                }

            } while (false);

            var kb = new List<InlineKeyboardButton>();
            var keybordIdx = 0;


            keybordIdx++;
            kb.Add(new InlineKeyboardButton()
            {
                Text = Left == pageNumber ? $"[{pageNumber}]" : $"{Left}",
                CallbackData = $"{buttonExtraInfo}:{Left}"
            });

            for (var page = Start; page <= Finish; page++)
            {
                keybordIdx++;
                kb.Add(new InlineKeyboardButton()
                {
                    Text = page == pageNumber ? $"[{page}]" : $"{page}",
                    CallbackData = $"{buttonExtraInfo}:{page}"
                });

            }

            keybordIdx++;
            kb.Add(new InlineKeyboardButton()
            {
                Text = Right == pageNumber ? $"[{pageNumber}]" : $"{Right}",
                CallbackData = $"{buttonExtraInfo}:{Right}"
            });

            // for other empty buttons
            for (var i = 1; i <= 5 - keybordIdx; i++)
            {
                keybordIdx++;
                kb.Add(new InlineKeyboardButton()
                {
                    Text = " . ",
                    CallbackData = $"{buttonExtraInfo}:0"
                });

            }

            return kb.ToArray();
        }

        private void resetAllCurrentState()
        {
            SimpayCore.resetAllCurrentState();
        }

        #endregion //CommonsPart

        #region Core
        public void sendPaymentMessage(string paymentLink, string specifiedMessage)
        {
            var inlineK = new List<InlineKeyboardButton[]>();
            var colKey = new List<InlineKeyboardButton>();

            colKey.Add(new InlineKeyboardButton
            {
                Text = "پرداخت ",
                Url = paymentLink
            });
            inlineK.Add(colKey.ToArray());

            var r = new Telegram.Bot.Types.ReplyMarkups.InlineKeyboardMarkup();
            r.InlineKeyboard = inlineK.ToArray();
            telegramAPI.send(specifiedMessage, r);
        }
        public CallBackTransactionOutput CallBackWebhook(CallBackTransactionInput callback)
        {
            Def.MyDbLogger.method = Utils.GetCurrentMethod();
            var ans = "";
            Log.Info("CallBackWebhook:" + Utils.ConvertClassToJson(callback), 0);
            var RET = new CallBackTransactionOutput() { HasError = false, Message = "Successfull" };
            do
            {
                if (callback.Status == 0)
                {
                    RET = new CallBackTransactionOutput { HasError = true, Message = callback.Description };
                    break;
                }

                chatId = SimpayCore.GetChatIdBySimpay(callback.SaleKey);


                if (chatId == 0)
                {
                    RET = new CallBackTransactionOutput { HasError = true, Message = "این شناسه فروش در دیتابیس وجود ندارد!" };
                    break;
                }
                TelegramMessage.chatId = chatId;
                SimpayCore.chatId = chatId;
                currentAction = new CurrentAction(chatId);
                SimpayCore.SetPaymentFinished(callback.SaleKey);

                var telegramAdmin = new TelegramAPI(ProjectValues.adminChatId);
                try
                {
                    var pmnt = new Payment.Manager(callback.SaleKey);
                    if (pmnt.data != null && pmnt.data.productId != 0)
                    {
                        telegramAdmin.send($"فروش برای {chatId} : {Utils.ConvertClassToJson(pmnt.data)}");
                    }
                    else
                    {
                        telegramAdmin.send($"فروش برای {chatId} : {Utils.ConvertClassToJson(callback)}");
                    }

                }
                catch (Exception ex)
                {
                    Log.Error(ex.Message, 0);
                    telegramAdmin.send($"callBack Error {chatId} : {ex.Message}");
                }


                //ProjectValues.adminChatId;

                telegramAPI = new TelegramAPI(chatId);

                if (callback.ProductId == 1)
                {
                    TrainTicketRedeemTicket(callback.SaleKey);
                    break;
                }
                else if (callback.ProductId == 2)
                {
                    var bus = new BusTicket.Manager(chatId);
                    ans = bus.Redeem(callback.SaleKey);
                    break;
                }
                else if (callback.ProductId == 3)
                {
                    //CinemaTicket
                    CinemaTicketBankCallBack(chatId, callback.SaleKey);
                    break;
                }
                else if (callback.ProductId == 4)
                {
                    var traffic = new TrafficFine.Manager(chatId);
                    ans = traffic.Redeem(callback.SaleKey);
                    break;
                }
                else if (callback.ProductId == 5)
                {
                    var billPayment = new BillPayment.Manager(chatId);
                    ans = billPayment.Redeem(callback.SaleKey);
                    break;
                }
                else if (callback.ProductId == 6 || callback.ProductId == 14)
                {
                    var autoCharge = new AutoCharge.Manager(chatId);
                    ans = autoCharge.Redeem(callback.SaleKey);
                    break;
                }
                else if (callback.ProductId == 7)
                {
                    var pinCharge = new PinCharge.Manager(chatId);
                    ans = pinCharge.Redeem(callback.SaleKey);
                    if (pinCharge.resultAction.hasError)
                    {
                        ans = pinCharge.resultAction.message;
                    }
                    break;
                }
                else if (callback.ProductId == 8)
                {
                    TMTNServiceRedeem(callback.SaleKey);
                    break;

                }
                else if (callback.ProductId == 10)
                {
                    ans = SimpayCore.GeneralBillpaymentRedeem(callback.SaleKey);
                    break;
                }
                else if (callback.ProductId == 11)
                {
                    AirplaneRedeemTicket(callback.SaleKey);
                    break;
                }


                else if (callback.ProductId == 15)
                {
                    ans = SimpayCore.GeneralBillpaymentRedeem(callback.SaleKey);
                    break;
                }

                else if (callback.ProductId == 16)
                {
                    ans = SimpayCore.GeneralBillpaymentRedeem(callback.SaleKey);
                    break;
                }

                else if (callback.ProductId == 17)
                {
                    ans = SimpayCore.GeneralBillpaymentRedeem(callback.SaleKey);
                    break;
                }
                else if (callback.ProductId >= 50 && callback.ProductId <= 100)
                {
                    var xpin = new XPIN.Manager();
                    ans = xpin.Redeem(callback.SaleKey);
                    break;
                }
                else if (callback.ProductId == 41 || callback.ProductId == 42)
                {
                    var Drama = new EventsTicket.Manager(callback.SaleKey);
                    if (Drama.data.id == 0)
                    {
                        ans = "متاسفانه بلیط شما توسط سروریس دهنده قطعی نشده است!";
                        break;
                    }
                    Drama.BuyTicket();
                    if (Drama.resultAction.hasError)
                    {
                        ans = Drama.resultAction.message;
                        break;
                    }
                    ans = Drama.PrintTicket();
                    break;


                }
                else if (callback.ProductId == 13)
                {
                    //AlborzInsurance
                }

                else if (callback.ProductId == 9)
                {
                    //USSDCharge
                }
                else if (callback.ProductId == 12)
                {
                    //USSDBill
                }

                else
                {
                    RET = new CallBackTransactionOutput { HasError = true, Message = "نوع محصول غیر قابل شناسایی" };
                    break;
                }



            } while (false);
            if (!String.IsNullOrEmpty(ans))
            {
                sendMenu(message: ans);
            }

            return RET;
        }
        public void CallBackfromBank(string callBackData)
        {
            //var ans = SimpayCore.redeemProductInfo(chatId, data);
            //sendMenu(message: ans);

            var ans = "";
            do
            {
                var callback = SimpayCore.ResolveCallBackData(callBackData);
                if (callback == null)
                {
                    ans = "مقدار برگشتی غیر قابل استناد است " + callBackData;
                    break;
                }
                if (callback.status == 0)
                {
                    ans = callback.description;
                    break;
                }
                switch (callback.productId)
                {
                    case SimpaySectionEnum.Unknown:
                        ans = "نوع محصول غیر قابل شناسایی";
                        break;
                    case SimpaySectionEnum.TrainTicket:
                        TrainTicketRedeemTicket(callback.saleKey);
                        break;
                    case SimpaySectionEnum.AirplaneTicket:
                        AirplaneRedeemTicket(callback.saleKey);
                        break;
                    case SimpaySectionEnum.TMTNServices:
                        TMTNServiceRedeem(callback.saleKey);
                        break;

                    case SimpaySectionEnum.BusTicket:
                        var bus = new BusTicket.Manager(chatId);
                        ans = bus.Redeem(callback.saleKey);
                        break;
                    case SimpaySectionEnum.AutoCharge:
                        var autoCharge = new AutoCharge.Manager(chatId);
                        ans = autoCharge.Redeem(callback.saleKey);
                        break;
                    case SimpaySectionEnum.TrafficFinesProduct:
                        var traffic = new TrafficFine.Manager(chatId);
                        ans = traffic.Redeem(callback.saleKey);
                        break;
                    case SimpaySectionEnum.BillPaymentProduct:
                        var billPayment = new BillPayment.Manager(chatId);
                        ans = billPayment.Redeem(callback.saleKey);
                        break;
                    case SimpaySectionEnum.PinCharge:
                        var pinCharge = new PinCharge.Manager(chatId);
                        ans = pinCharge.Redeem(callback.saleKey);
                        if (pinCharge.resultAction.hasError)
                        {
                            ans = pinCharge.resultAction.message;
                        }
                        break;

                    default:
                        var xpin = new XPIN.Manager();
                        ans = xpin.Redeem(callback.saleKey);
                        break;
                }

            } while (false);

            if (!String.IsNullOrEmpty(ans))
            {
                sendMenu(message: ans);
            }
        }
        private void CoreShowHistoryProducts()
        {
            do
            {
                telegramAPI.send("درحال بررسی سوابق شما.", cancelButton());

                var productList = SimpayCore.GetPurchaseHistoryProductsList();
                if (productList.Count == 0)
                {
                    sendMenu(message: "سوابقی برای شما درون سامانه ذخیره نشده است");
                    break;
                }

                var inlineK = new List<InlineKeyboardButton[]>();
                var colKey = new List<InlineKeyboardButton>();
                for (var i = 0; i < productList.Count; i++)
                {
                    colKey.Add(new InlineKeyboardButton
                    {
                        Text = productList[i].ProductShowName,
                        CallbackData = $"{SimpaySectionEnum.History}:producthistorystart:{productList[i].ProductID}"
                    });
                    inlineK.Add(colKey.ToArray());
                    colKey.Clear();
                }


                var r = new Telegram.Bot.Types.ReplyMarkups.InlineKeyboardMarkup();
                r.InlineKeyboard = inlineK.ToArray();
                telegramAPI.send("لطفا از میان فهرست زیر، سوابق مورد نظر را انتخواب کنید.", r);


            } while (false);


        }
        private void CoreShowHistoryUnfinishedPaymentInfo()
        {
            do
            {
                telegramAPI.send("درحال بررسی خریدهای ناموفق شما.", cancelButton());

                var productList = SimpayCore.GetUnfinishedPaymentsInfo();
                if (productList.Count == 0)
                {
                    sendMenu(message: " در سوابق شما خرید ناموفقی که وجه آن به حساب شما برگشت نشده باشد یافت نشد.");
                    break;
                }

                var inlineK = new List<InlineKeyboardButton[]>();
                var colKey = new List<InlineKeyboardButton>();
                for (var i = 0; i < productList.Count; i++)
                {
                    colKey.Add(new InlineKeyboardButton
                    {
                        Text = $"{productList[i].ProductShowName} {productList[i].Amount} ",
                        CallbackData = $"{SimpaySectionEnum.History}:redeem:{productList[i].ProductID}:{productList[i].SaleKey}"
                    });
                    inlineK.Add(colKey.ToArray());
                    colKey.Clear();
                }


                var r = new Telegram.Bot.Types.ReplyMarkups.InlineKeyboardMarkup();
                r.InlineKeyboard = inlineK.ToArray();
                telegramAPI.send("فهرست خریدهای ناتمام شما در زیر مشخص شده است. لطفا برای اتمام خرید دکمه های مربوطه را فشار دهید.", r);


            } while (false);


        }

        public void CoreHistoryCallBack(string data)
        {
            var action = data.Split(':')[1];
            do
            {
                switch (action.ToLower())
                {
                    case "producthistorystart":
                        CoreShowProductHistory(Convert.ToInt32(data.Split(':')[2]), 0);
                        break;
                    case "producthistorypaging":
                        CoreShowProductHistory(Convert.ToInt32(data.Split(':')[2]), Convert.ToInt32(data.Split(':')[3]));
                        break;
                    case "redeem":
                        var productId = Convert.ToInt32(data.Split(':')[2]);
                        var salekey = data.Split(':')[3];

                        CoreRedeemAgain(productId, salekey);
                        break;
                    default:
                        sendMenu(message: $"action, {action}, not recognized!");
                        break;
                }

            } while (false);



        }
        public void CoreShowProductHistory(long productId, long offset = 0)
        {

            var msgToSend = "";
            var inlineK = new List<InlineKeyboardButton[]>();
            var colKey = new List<InlineKeyboardButton>();

            do
            {
                var history = SimpayCore.GetPurchaseProductsHistory(productId, offset);
                var historyList = new List<Core.wsHistoryData.History_Output_Parameters_Detail>();
                if (history.Detail != null)
                    historyList.AddRange(history.Detail);


                if (historyList.Count == 0)
                {
                    msgToSend = "رکوردی برای مشاهده وجود ندارد";
                    break;
                }
                var msg = "";
                long recordId = 0;

                object extraInfo = new object();

                for (var i = 0; i < historyList.Count; i++)
                {
                    msg += " \n \n ";
                    msg += $" عنوان: {historyList[i].ProductShowName} ";
                    msg += " \n ";
                    if (historyList[i].ProductID >= 50 && historyList[i].ProductID <= 100)
                    {
                        var xpin = JsonConvert.DeserializeObject<Core.wsHistoryData.History_Output_Parameters_ExtralInfo_Xpin>(Utils.ConvertClassToJson(historyList[i].ExtraInfo));
                        msg += $"شرح محصول: {xpin.Description} \n";
                        msg += $"پین: {xpin.PinCode} \n\n ";
                    }
                    if (historyList[i].ProductID == (int)SimpaySectionEnum.AirplaneTicket)
                    {
                        var airplane = JsonConvert.DeserializeObject<Core.wsHistoryData.History_Output_Parameters_ExtralInfo_AirplaneTicket>(Utils.ConvertClassToJson(historyList[i].ExtraInfo));
                        msg += " اطلاعات بلیط رفت: \n \n ";
                        foreach (var item in airplane.WayGoTicketsList)
                        {
                            msg += $" شماره بلیط: {item.TicketNumber}  \n ";
                            msg += $" نام مسافر: {item.FirstName} {item.LastName} \n ";
                            msg += $" شرکت هواپیمائی: {item.AirlineCode} \n ";
                            msg += $" شماره پرواز: {item.AirlineNumber} \n ";
                            msg += $" لینک جهت چاپ بلیط:\n {item.Html} \n ";
                        }
                        if (airplane.TwoWay)
                        {
                            foreach (var item in airplane.WayReturnTicketsList)
                            {
                                msg += $" شماره بلیط: {item.TicketNumber}  \n ";
                                msg += $" نام مسافر: {item.FirstName} {item.LastName} \n ";
                                msg += $" شرکت هواپیمائی: {item.AirlineCode} \n ";
                                msg += $" شماره پرواز: {item.AirlineNumber} \n ";
                                msg += $" لینک جهت چاپ بلیط:\n {item.Html} \n ";
                            }
                        }
                    }
                    if (historyList[i].ProductID == (int)SimpaySectionEnum.BusTicket)
                    {
                        var bus = JsonConvert.DeserializeObject<Core.wsHistoryData.History_Output_Parameters_ExtralInfo_BusTicket>(Utils.ConvertClassToJson(historyList[i].ExtraInfo));
                        var fcDepartureDateTime = new FarsiCalendar(bus.DepartureDateTime);
                        msg += $" شماره بلیط: {bus.TicketNumber} \n \n ";
                        msg += " نام مسافر: \n ";
                        msg += $"{bus.PassengerName}  \n \n ";
                        msg += " تاریخ و ساعت حرکت: \n ";
                        msg += $"{fcDepartureDateTime.pDate }  \n \n ";
                        msg += " مبدا: \n ";
                        msg += $"{bus.SourceTerminalShowName}  \n  ";
                        msg += " مقصد: \n ";
                        msg += $"{bus.DestinationTerminalShowName}  \n  ";
                        msg += " نام شرکت: \n ";
                        msg += $"{bus.CorporationName}  \n  ";
                        msg += $" تعداد صندلی:  {bus.SeatsCount}  \n  ";
                        msg += $" بهای کل: {bus.TotalAmount.ToString("#,##")} ریال \n \n ";


                    }
                    if (historyList[i].ProductID == (int)SimpaySectionEnum.PinCharge)
                    {
                        var pinCharge = JsonConvert.DeserializeObject<Core.wsHistoryData.History_Output_Parameters_ExtralInfo_PinCharge>(Utils.ConvertClassToJson(historyList[i].ExtraInfo));
                        msg += $" نوع شارژ: {pinCharge.ChargeTypeShowName} ";
                        msg += " \n ";
                        msg += $"پین کد : {pinCharge.PinCode} ";
                        msg += " \n ";
                    }

                    if (historyList[i].ProductID == (int)SimpaySectionEnum.AutoCharge)
                    {
                        var autoCharge = JsonConvert.DeserializeObject<Core.wsHistoryData.History_Output_Parameters_ExtralInfo_AutoCharge>(Utils.ConvertClassToJson(historyList[i].ExtraInfo));
                        msg += $" نوع شارژ: {autoCharge.ChargeTypeShowName} ";
                        msg += " \n ";
                        msg += " شماره موبایل: \n ";
                        msg += autoCharge.MobileNumber;
                        msg += " \n ";
                    }
                    if (historyList[i].ProductID == (int)SimpaySectionEnum.TrafficFinesProduct)
                    {
                        var jsonTraffic = Utils.ConvertClassToJson(historyList[i].ExtraInfo);
                        var traffic = JsonConvert.DeserializeObject<Core.wsHistoryData.History_Output_Parameters_ExtralInfo_Traffic>(jsonTraffic);
                        for (var j = 0; j < traffic.Tickets.Length; j++)
                        {
                            var ticket = traffic.Tickets[j];
                            msg += "\n \n ";
                            msg += $" شماره پلاک خودرو: {ticket.LicensePlate} ";
                            msg += "\n ";
                            msg += $"قبض جریمه به شماره {ticket.BillID}  ";
                            msg += "\n ";
                            msg += $" بابت: {ticket.Type} ";
                            msg += "\n ";
                            msg += $" وضعیت:  {ticket.PaymentStatus} ";
                            msg += "\n ";
                            msg += $"کد رهگیری:  {ticket.PaymentTraceNumber} ";
                            msg += "\n ";

                        }
                    }
                    else if (historyList[i].ProductID == (int)SimpaySectionEnum.BillPaymentProduct)
                    {
                        var jsonBill = Utils.ConvertClassToJson(historyList[i].ExtraInfo);
                        var bill = JsonConvert.DeserializeObject<Core.wsHistoryData.History_Output_Parameters_ExtralInfo_BillPayment>(jsonBill);
                        msg += "\n ";
                        msg += $"نوع قبض:{bill.BillType}";
                        msg += "\n ";
                        msg += $"شناسه قبض:{bill.BillID}";
                        msg += "\n ";
                        msg += $"شناسه پرداخت:{bill.PaymentID}";
                        msg += "\n ";
                        msg += $"کد رهگیری:{bill.PaymentTraceNumber}";
                        msg += "\n ";
                    }

                    msg += $" مبلغ: {historyList[i].Amount.ToString("#,##")} ریال";
                    msg += " \n ";

                    msg += $" تاریخ: {historyList[i].HijryDateTime} ";
                    msg += " \n \n ";
                    msg += " ------------------------------ ";

                    recordId = historyList[i].RecordID;
                }
                msgToSend = msg;
                if (history.HasMore)
                {
                    colKey.Add(new InlineKeyboardButton
                    {
                        Text = "بیشتر",
                        CallbackData = $"{SimpaySectionEnum.History}:producthistorypaging:{productId}:{recordId}"
                    });
                    inlineK.Add(colKey.ToArray());
                    colKey.Clear();
                }

            } while (false);

            if (inlineK.Count > 0)
            {
                var r = new Telegram.Bot.Types.ReplyMarkups.InlineKeyboardMarkup();
                r.InlineKeyboard = inlineK.ToArray();
                if (offset > 0)
                    telegramAPI.editText(callbackQuery.Message.ID, msgToSend, r);
                else
                    telegramAPI.send(msgToSend, r);
            }
            else
            {
                if (offset > 0)
                    telegramAPI.editText(callbackQuery.Message.ID, msgToSend);
                else
                    telegramAPI.send(msgToSend);
            }

        }
        #endregion





        #region profilling
        private void userActivationAsking()
        {
            var inlineK = new List<InlineKeyboardButton[]>();
            var colK = new List<InlineKeyboardButton>();

            colK.Add(new InlineKeyboardButton()
            {
                Text = $"ثبت نام",
                CallbackData = $"{SimpaySectionEnum.Activation}"
            });
            inlineK.Add(colK.ToArray());
            colK.Clear();
            var markup = new Telegram.Bot.Types.ReplyMarkups.InlineKeyboardMarkup();
            markup.InlineKeyboard = inlineK.ToArray();



            var msgToSend = "توجه: \n \n ";
            msgToSend += " اگر مایل به ذخیره سوابق تراکنشات و استفاده از خدمات سیم پی میباشید ،حتما در سامانه ثبت نام فرمایید.";
            msgToSend += "\n ";
            msgToSend += " در غیر اینصورت با اینکه خریدهای شما انجام خواهد شد، اما در بخش سوابق قابل بازیابی نخواهد بود. ";
            msgToSend += " \n \n \n ";
            telegramAPI.send(msgToSend, markup);
        }

        private bool checkUserActivation()
        {
            var activated = true;
            do
            {
                var user = new UserModel(chatId);
                if (String.IsNullOrEmpty(user.mobileNumber) || String.IsNullOrEmpty(user.JsonWebToken))
                {
                    activated = false;
                    break;
                }
                activated = user.activated;
            } while (false);
            return activated;


        }

        private void activateUserMessage(string field = "", string value = "")
        {
            var messageToSend = "";

            if (String.IsNullOrEmpty(field))
                field = "mobile";

            do
            {

                switch (field.ToLower())
                {
                    case "mobile":
                        messageToSend = "لطفا شماره موبایل خود را وارد نمایید:";
                        currentAction.set(SimpaySectionEnum.Activation, "mobile");
                        break;
                    case "activationcode":
                        messageToSend = "کد ارسالی به موبایلتان را وارد نمایید:";
                        currentAction.set(SimpaySectionEnum.Activation, "activationcode");
                        break;

                    default:
                        break;
                }
            } while (false);
            telegramAPI.send(messageToSend, cancelButton());

        }
        private void activationUserProcess(string field, string value)
        {
            var nextStep = "";
            do
            {
                var user = new UserModel(chatId);
                switch (field.ToLower())
                {
                    case "mobile":
                        var mobile = new Mobile() { Number = value };
                        if (!mobile.IsNumberContentValid())
                        {
                            telegramAPI.send("شماره موبایل ارسالی صحیح نیست لطفا دوباره سعی نمایید.:", cancelButton());
                            break;
                        }

                        user.mobileNumber = mobile.InternationalNumber;
                        user.reportUser();
                        var sendActivationCodeResult = Profilling.SendActivationCode(user.mobileNumber);
                        if (Profilling.resultAction.hasError)
                        {
                            currentAction.remove();
                            sendMenu(message: Profilling.resultAction.message);
                            break;
                        }
                        if (!sendActivationCodeResult.TwoPhaseAuthentication)
                        {
                            user.activationCode = "111111";
                            user.reportUser();
                            ActivationUserDone(user);

                            break;
                        }
                        nextStep = "activationcode";
                        break;
                    case "activationcode":
                        user.activationCode = value;
                        user.reportUser();
                        ActivationUserDone(user);
                        break;


                    default:
                        break;
                }
            } while (false);
            if (nextStep != "")
            {
                activateUserMessage("activationcode");
            }

        }
        private void ActivationUserDone(UserModel user)
        {
            do
            {
                var CreateSessionResult = Profilling.CreateSession(user.mobileNumber, user.activationCode);
                if (Profilling.resultAction.hasError)
                {
                    currentAction.remove();
                    sendMenu(message: Profilling.resultAction.message);
                    break;
                }

                if (user.introduceBy != 0 && user.introduceBy != user.chatId) // if this user has refreale then should be report to the core
                {
                    var masterUser = new UserModel(user.introduceBy);
                    SimpayCore.ReportInvitation(masterUser.invitationCode, user.mobileNumber);
                }

                user.JsonWebToken = CreateSessionResult;
                user.activated = true;
                user.reportUser();
                currentAction.remove();
                sendMenu(message: "شماره شما در سامانه سیم پی ثبت گردید. از این پس کل سوابق خرید شما در سامانه در دسترس خواهد بود.");

            } while (false);

        }
        private void UnactivateUserMessage()
        {
            var messageToSend = "";
            var inlineK = new List<InlineKeyboardButton[]>();
            var colK = new List<InlineKeyboardButton>();

            do
            {
                messageToSend = "در صورت خروج خریدهای شما در سوابق ذخیره نخواهد شد. آیا مایلید روند خروج ادامه یابد؟";
                colK.Add(new InlineKeyboardButton()
                {
                    Text = $"بله",
                    CallbackData = $"{SimpaySectionEnum.Unactivation}:unactive"
                });
                colK.Add(new InlineKeyboardButton()
                {
                    Text = $"خیر",
                    CallbackData = $"{SimpaySectionEnum.Unactivation}:cancel"
                });
                inlineK.Add(colK.ToArray());
                colK.Clear();



            } while (false);
            var markup = new Telegram.Bot.Types.ReplyMarkups.InlineKeyboardMarkup();
            markup.InlineKeyboard = inlineK.ToArray();

            telegramAPI.send(messageToSend, markup);

        }
        private void UnactivateUserCallBack(string data)
        {
            var action = data.Split(':')[1];
            do
            {
                switch (action.ToLower())
                {
                    case "unactive":
                        UnactiveUser();
                        break;
                    case "cancel":
                        telegramAPI.editText(callbackQuery.Message.ID, "انصراف از عملیات جاری");
                        sendMenu();
                        break;
                    default:
                        telegramAPI.editText(callbackQuery.Message.ID, "دستور شناخته نشد");
                        sendMenu();
                        break;
                }
            } while (false);

        }

        private void UnactiveUser()
        {
            var user = new UserModel(chatId);
            user.activated = false;
            user.reportUser();
            if (callbackQuery != null)
            {
                telegramAPI.editText(callbackQuery.Message.ID, "ثبت نام شما در سامانه لغو گردید.");
                sendMenu();
            }
            else
            {
                sendMenu(message: "ثبت نام شما در سامانه لغو گردید.");
            }


        }

        #endregion


        #region Job
        public void sendMessageToChat(string UID, Telegram.Bot.Types.Enums.MessageType messageType, string fileId, string message, string[] chats)
        {
            var jobUID = Convert.ToInt32(UID);
            var JobHistory = new JobHistory(jobUID);
            var messageResult = "";
            var tAPI = new TelegramAPI(chats[0]);
            tAPI.disableNotification = true;
            for (int i = 0; i < chats.Length; i++)
            {
                tAPI.chatId = chats[i];
                switch (messageType)
                {
                    case MessageType.TextMessage:
                        tAPI.parseMode = ParseMode.Html;
                        tAPI.sendTextAsync(message, chats[i]);
                        break;
                    case MessageType.PhotoMessage:
                    case MessageType.AudioMessage:
                    case MessageType.VideoMessage:
                    case MessageType.VoiceMessage:
                    case MessageType.DocumentMessage:
                    case MessageType.StickerMessage:
                    case MessageType.LocationMessage:
                    case MessageType.ContactMessage:
                    case MessageType.ServiceMessage:
                    case MessageType.VenueMessage:
                        tAPI.fileToSend = new FileToSend
                        {
                            FileId = fileId
                        };

                        tAPI.caption = message;
                        tAPI.send(messageType);
                        break;
                    default:
                        break;
                }


                if (tAPI.resultAction.hasError)
                {
                    messageResult += $"N:{chats[i]}-";
                }
                else
                {
                    messageResult += $"Y:{chats[i]}-";
                }
            }
            JobHistory.result = messageResult == "" ? "OK" : messageResult;
            JobHistory.setInfo();
            (new Panel()).ReportBackResult(UID, "successful");

        }
        #endregion



        private void ContactUsShowMessage()
        {
            do
            {


                var msg = "";
                msg += $" ارتباط با سیم پی  {Environment.NewLine}";
                msg += $" منتظرتان هستیم  {Environment.NewLine}{Environment.NewLine} ";
                msg += $"  ضمن قدردانی از حسن انتخاب شما ، خوشحال خواهیم شد تا با ارائه نظرات ، انتقادات و پیشنهادات سازنده خود ، ما را در دسترسی به هدف اصلی مان که همان جلب رضایت شما مشتریان عزیز است ، یاری نمایید. {Environment.NewLine}";
                msg += $" خواهشمند است به منظور بررسی های لازم و انجام پی گیری های مورد نیاز ، ضمن ارائه نظرات خود به صورت شفاف ، جامع و کامل مشخصات فردی و شماره تماس خود را درج نمایید.  {Environment.NewLine}";

                msg += $"   {Environment.NewLine}{Environment.NewLine}";
                msg += $"ارتباط با بخش پشتیبانی: @simpayC {Environment.NewLine}";

                msg += $"وب سایت: http://simpay.ir {Environment.NewLine}";
                msg += $"پست الکترونیک : info@simpay.ir {Environment.NewLine}";

                msg += $"تلفن تماس:41576-021 {Environment.NewLine}";

                msg += $"دورنگار : 88649084-021  {Environment.NewLine}";
                msg += $"آدرس نماد اعتماد الکترونیکی : http://simpay.ir/enamad.html ";
                msg += $"   {Environment.NewLine}";
                msg += $"   {Environment.NewLine}";
                sendMenu(message: msg);

            } while (false);
        }

        private void CoreRedeemAgain(int productId, string salekey)
        {
            CallBackTransactionInput callback = new CallBackTransactionInput
            {
                Status = 1,
                SaleKey = salekey,
                ProductId = productId,
                Description = ""
            };
            var callbackOutput = CallBackWebhook(callback);
        }

        private void ShowUploadedFileSpecification()
        {
            var fileInfo = "";
            var lastFileId = "";
            switch (message.Type)
            {
                case MessageType.PhotoMessage:
                    foreach (var photo in message.Photo)
                    {
                        fileInfo += $"{photo.ID}{Environment.NewLine}";
                        fileInfo += $"{photo.Size}{Environment.NewLine}";
                        fileInfo += $"{photo.Height}{Environment.NewLine}";
                        fileInfo += $"{photo.Width}{Environment.NewLine}";
                        fileInfo += $"----------------{Environment.NewLine}";
                        lastFileId = photo.ID;
                    }

                    break;
                case MessageType.AudioMessage:
                    var audio = message.Audio;
                    fileInfo += $"{audio.ID}{Environment.NewLine}";
                    fileInfo += $"{audio.Title}{Environment.NewLine}";
                    fileInfo += $"{audio.MimeType}{Environment.NewLine}";
                    fileInfo += $"{audio.FileSize}{Environment.NewLine}";
                    fileInfo += $"----------------{Environment.NewLine}";
                    lastFileId = audio.ID;

                    break;
                case MessageType.VideoMessage:
                    var video = message.Video;
                    fileInfo += $"{video.ID}{Environment.NewLine}";
                    fileInfo += $"{video.Size}{Environment.NewLine}";
                    fileInfo += $"{video.Width}{Environment.NewLine}";
                    fileInfo += $"{video.Height}{Environment.NewLine}";
                    fileInfo += $"{video.Duration}{Environment.NewLine}";
                    fileInfo += $"----------------{Environment.NewLine}";
                    lastFileId = video.ID;

                    break;
                case MessageType.VoiceMessage:


                    break;
                case MessageType.DocumentMessage:
                    var doc = message.Document;
                    fileInfo += $"{doc.ID}{Environment.NewLine}";
                    fileInfo += $"{doc.Size}{Environment.NewLine}";
                    fileInfo += $"{doc.Name}{Environment.NewLine}";
                    fileInfo += $"{doc.MimeType}{Environment.NewLine}";
                    fileInfo += $"----------------{Environment.NewLine}";
                    lastFileId = doc.ID;

                    break;
                case MessageType.StickerMessage:
                    var sticker = message.Sticker;
                    fileInfo += $"{sticker.ID}{Environment.NewLine}";
                    fileInfo += $"{sticker.Size}{Environment.NewLine}";
                    fileInfo += $"{sticker.Height}{Environment.NewLine}";
                    fileInfo += $"{sticker.Width}{Environment.NewLine}";
                    fileInfo += $"----------------{Environment.NewLine}";
                    lastFileId = sticker.ID;

                    break;
                case MessageType.LocationMessage:

                    break;
                case MessageType.ContactMessage:

                    break;
                case MessageType.ServiceMessage:

                    break;
                case MessageType.VenueMessage:

                    break;
                case MessageType.GameMessage:

                    break;
                default:
                    break;
            }

            if (!String.IsNullOrEmpty(fileInfo))
            {
                telegramAPI.send($"file info: {Environment.NewLine} {fileInfo } {Environment.NewLine} ");

                telegramAPI.fileToSend = new FileToSend
                {
                    FileId = lastFileId
                };
                telegramAPI.caption = message.Caption;
                telegramAPI.send(message.Type);



            }

        }

    }


}
