using Shared.WebService;
using System;
using System.Collections.Generic;
using Telegram.Bot.Types;


namespace Models
{
    public partial class TelegramMessage
    {

        #region PinCharge

        private void PinChargeCallBack(string data)
        {

            Def.MyDbLogger.method = Utils.GetCurrentMethod();
            var action = data.Split(':')[1];
            var typeId = 0;
            var amount = 0;

            do
            {
                switch (action.ToLower())
                {
                    case "showoperatorcharge":
                        typeId = Convert.ToInt16(data.Split(':')[2]);
                        PinChargeShowChargeDetail(typeId);
                        break;
                    case "buycharge":
                        typeId = Convert.ToInt16(data.Split(':')[2]);
                        amount = Convert.ToInt32(data.Split(':')[3]);
                        PinChargeBuy(typeId, amount);
                        break;
                    default:
                        break;
                }
            } while (false);
        }

        private void PinChargeSelectChargeMessage(PinCharge.ChargeListTypeEnum chargeType = PinCharge.ChargeListTypeEnum.Header, int typeId = 0)
        {
            Def.MyDbLogger.method = Utils.GetCurrentMethod();
            switch (chargeType)
            {
                case PinCharge.ChargeListTypeEnum.Header:
                    PinChargeShowChargeOperators();
                    break;
                case PinCharge.ChargeListTypeEnum.Detail:
                    break;
                default:
                    break;
            }


        }

        private void PinChargeShowChargeOperators()
        {
            Def.MyDbLogger.method = Utils.GetCurrentMethod();
            var messageToSend = "";
            var inlineK = new List<InlineKeyboardButton[]>();
            var colKey = new List<InlineKeyboardButton>();
            var ChargeListKeyboard = new Telegram.Bot.Types.ReplyMarkups.InlineKeyboardMarkup();
            telegramAPI.send("هم اکنون در حال یافتن شارژهای موجود هستم. لطفا صبر  نمایید.", cancelButton());

            do
            {
                var pincharge = new PinCharge.Manager(chatId);
                var list = pincharge.getOperatorList();
                if (list.Count == 0)
                {
                    sendMenu(message: "متاسفانه هم اکنون شارژی موجود نیست");
                    break;
                }

                messageToSend = "از فهرست زیر اپراتور مورد نظر را انتخاب کنید.";

                for (var i = 0; i < list.Count; i++)
                {
                    colKey.Add(new InlineKeyboardButton()
                    {
                        Text = $"{list[i].OperatorName} ",
                        CallbackData = $"{SimpaySectionEnum.PinCharge}:showoperatorcharge:{list[i].TypeID}"
                    });
                    inlineK.Add(colKey.ToArray());
                    colKey.Clear();
                }
                ChargeListKeyboard.InlineKeyboard = inlineK.ToArray();

            } while (false);

            telegramAPI.send(messageToSend, ChargeListKeyboard);

        }

        private void PinChargeShowChargeDetail(int typeId)
        {
            Def.MyDbLogger.method = Utils.GetCurrentMethod();
            var messageToSend = "";
            var inlineK = new List<InlineKeyboardButton[]>();
            var colKey = new List<InlineKeyboardButton>();
            var ChargeListKeyboard = new Telegram.Bot.Types.ReplyMarkups.InlineKeyboardMarkup();
            do
            {
                var pincharge = new PinCharge.Manager(chatId);
                var list = pincharge.GetChargesList(typeId);
                if (list.Count == 0)
                {
                    sendMenu(message: "متاسفانه هم اکنون شارژی موجود نیست");
                    break;
                }

                messageToSend = "از فهرست زیر شارژ مورد نظر را انتخاب کنید.";

                for (var i = 0; i < list.Count; i++)
                {
                    colKey.Add(new InlineKeyboardButton()
                    {
                        Text = $"{list[i].Name} ",
                        CallbackData = $"{SimpaySectionEnum.PinCharge}:buycharge:{list[i].TypeID}:{list[i].Amount}"
                    });
                    inlineK.Add(colKey.ToArray());
                    colKey.Clear();
                }
                ChargeListKeyboard.InlineKeyboard = inlineK.ToArray();

            } while (false);

            telegramAPI.editText(callbackQuery.Message.ID, messageToSend, ChargeListKeyboard);

        }


        public void PinChargeBuy(int typeId, int amount)
        {
            do
            {
                var pinCharge = new PinCharge.Manager(chatId);
                var buySaleKey = pinCharge.getSaleKey(typeId, amount);
                if (String.IsNullOrEmpty(buySaleKey))
                {
                    sendMenu(message: "متاسفانه در عملیات خرید پین شارژ مشکلی وجود دارد. بعدا مراجعه فرمایید!");
                    break;
                }

                PaymentStartProcess(buySaleKey);



                //if (pinCharge.resultAction.hasError)
                //{
                //    sendMenu(message: pinCharge.resultAction.message);
                //    break;
                //}
                //var resultLink = SimpayCore.getPaymentLink(buySaleKey);



                //var message = $"برای خرید پین خریداری شده به مبلغ {amount.ToString("#,##")} ریال از دکمه زیر استفاده کنید:";
                //sendPaymentMessage(resultLink, message);

            } while (false);
        }

        #endregion


    }
}
