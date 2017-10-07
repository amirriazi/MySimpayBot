using Shared.WebService;
using System;
using System.Collections.Generic;
using Telegram.Bot.Types;

namespace Models
{
    public partial class TelegramMessage
    {
        #region GiftCard_xpin
        private void XpinRequestInfo(XPIN.XpinCategoryEnum Category, string fieldName = "", string fieldValue = "", string currentId = "", bool forceNewMessage = false)
        {
            Def.MyDbLogger.method = Utils.GetCurrentMethod();

            var hasError = false;
            var xpin = new XPIN.Manager(chatId, Category, Convert.ToInt32(currentId == "" ? "0" : currentId));

            var messagetoSend = "";
            var keyboardList = new Telegram.Bot.Types.ReplyMarkups.InlineKeyboardMarkup();
            if (String.IsNullOrEmpty(fieldName))
            {
                telegramAPI.send("در حال فراخوانی فهرست محصولات .", cancelButton());
                fieldName = "productid";
            }


            do
            {
                switch (fieldName)
                {
                    case "productid":
                        var productKeyboardList = XpinGetProductList(xpin);
                        if (productKeyboardList == null)
                        {
                            hasError = true;
                            messagetoSend = "متاسفانه هم اکنون این بخش فعال نیست.";
                            break;
                        }
                        messagetoSend = "لطفا نوع محصولات درخواستی را مشخص نمایید:";
                        keyboardList = productKeyboardList;
                        break;
                    case "subproductid":
                        var subProductKeyboardList = XpinGetSubProductList(xpin);
                        if (subProductKeyboardList == null)
                        {
                            hasError = true;
                            messagetoSend = "متاسفانه موجودی این محصول صفر میباشد.";
                            break;
                        }
                        messagetoSend = $"لطفا محصول درخواستی خود را برای {xpin.data.productName} از فهرست زیر انتخاب نمایید:";
                        keyboardList = subProductKeyboardList;
                        break;

                    default:
                        hasError = true;
                        messagetoSend = "Field Name was not specified!";
                        break;

                }
            } while (false);
            if (hasError)
            {
                sendMenu(message: messagetoSend);
            }
            else
            {
                if (callbackQuery != null && !forceNewMessage)
                {
                    telegramAPI.editText(callbackQuery.Message.ID, messagetoSend, keyboardList);
                }
                else
                {
                    telegramAPI.send(messagetoSend, keyboardList);
                }

            }


        }

        private Telegram.Bot.Types.ReplyMarkups.InlineKeyboardMarkup XpinGetProductList(XPIN.Manager xpin)
        {

            var inlineK = new List<InlineKeyboardButton[]>();
            var colKey = new List<InlineKeyboardButton>();
            var keyboardList = new Telegram.Bot.Types.ReplyMarkups.InlineKeyboardMarkup();

            do
            {
                xpin.GetProductList();
                if (xpin.resultAction.hasError)
                {
                    telegramAPI.send(xpin.resultAction.message);
                    break;
                }
                var productList = xpin.product;
                if (productList.Count < 1)
                {
                    keyboardList = null;
                    break;
                }

                for (var i = 0; i < productList.Count; i++)
                {
                    colKey.Add(new InlineKeyboardButton
                    {
                        Text = $@"{productList[i].productName}",
                        CallbackData = $"{SimpaySectionEnum.XpinProduct}:productid:{xpin.data.id}:{productList[i].productId}"
                    });
                    inlineK.Add(colKey.ToArray());
                    colKey.Clear();
                }
                keyboardList.InlineKeyboard = inlineK.ToArray();



            } while (false);


            return keyboardList;

        }

        private Telegram.Bot.Types.ReplyMarkups.InlineKeyboardMarkup XpinGetSubProductList(XPIN.Manager xpin)
        {

            var inlineK = new List<InlineKeyboardButton[]>();
            var colKey = new List<InlineKeyboardButton>();
            var keyboardList = new Telegram.Bot.Types.ReplyMarkups.InlineKeyboardMarkup();

            do
            {
                xpin.GetSubProductList();
                if (xpin.resultAction.hasError)
                {
                    telegramAPI.send(xpin.resultAction.message);
                    break;
                }
                var subProductList = xpin.subProduct;
                if (subProductList.Count < 1)
                {
                    keyboardList = null;
                    break;
                }

                for (var i = 0; i < subProductList.Count; i++)
                {
                    if (subProductList[i].subProductIsActive)
                    {
                        colKey.Add(new InlineKeyboardButton
                        {
                            Text = $@"{subProductList[i].subProductName} ({subProductList[i].subProductAmount.ToString("##,#")} ریال)",
                            CallbackData = $"{SimpaySectionEnum.XpinProduct}:subproductid:{xpin.data.id}:{subProductList[i].subProductId}"
                        });
                        inlineK.Add(colKey.ToArray());
                        colKey.Clear();
                    }
                }
                keyboardList.InlineKeyboard = inlineK.ToArray();
            } while (false);

            return keyboardList;

        }

        private void XpinGiftCardCallBackQuery(string data)
        {
            Def.MyDbLogger.method = Utils.GetCurrentMethod();
            var action = data.Split(':')[1];
            var currentId = data.Split(':')[2]; ;

            var xpin = new XPIN.Manager(chatId, Convert.ToInt32(currentId == "" ? "0" : currentId));
            do
            {
                switch (action.ToLower())
                {
                    case "productid":
                        var productId = Convert.ToInt32(data.Split(':')[3]);
                        var product = xpin.getProduct(productId);

                        xpin.data.productID = product.productId;
                        xpin.data.productName = product.productName;

                        xpin.setInfo();
                        XpinRequestInfo(XPIN.XpinCategoryEnum.GiftCard, "subproductid", xpin.data.subProductID.ToString(), xpin.data.id.ToString());
                        break;
                    case "subproductid":
                        var subProductId = Convert.ToInt32(data.Split(':')[3]);
                        var subProduct = xpin.getSubProduct(subProductId);
                        xpin.data.subProductID = subProduct.subProductId;
                        xpin.data.subProductName = subProduct.subProductName;
                        xpin.data.amount = subProduct.subProductAmount;
                        xpin.setInfo();

                        XpinPayment(xpin.data.id);
                        break;


                    default:
                        break;
                }
            } while (false);
        }

        private void XpinPayment(long id)
        {
            Def.MyDbLogger.method = Utils.GetCurrentMethod();
            telegramAPI.send("لطفا تا نهائی نمودن سفارش صبر فرمایید.");
            var xpin = new XPIN.Manager(chatId, id);
            do
            {
                xpin.ReservePin();
                if (xpin.resultAction.hasError)
                {
                    sendMenu(message: "متاسفانه عملیات رزرو پین موفقیت آمیز نبود. لطفا مجددا درخواست نمایید.");
                    break;
                }

                PaymentStartProcess(xpin.data.saleKey);

                //var saleProductInfo = SimpayCore.GetSalePaymentInfo(xpin.data.saleKey);
                //if (saleProductInfo == null)
                //{
                //    sendMenu(message: "متاسفانه عملیات دریافت اطلاعات پین رزرو شده موفقیت آمیز نبود. لطفا مجددا درخواست نمایید.");
                //    break;
                //}



                //var resultLink = SimpayCore.getPaymentLink(xpin.data.saleKey);
                //var message = "";
                //message += " درخواست شما برای خرید محصول به شرح زیر ثبت گردید:";
                //message += " \n \n ";
                //message += $" نوع محصول: \n";
                //message += $"{saleProductInfo.ProductName}";
                //message += " \n ";
                //message += $" شرح کامل محصول: \n ";
                //message += $"{saleProductInfo.Description}";
                //message += " \n ";
                //message += $" مبلغ نهائی: \n ";
                //message += $"{saleProductInfo.Amount.ToString("##,#")} ریال ";
                //message += " \n \n ";
                //message += "در صورت تایید، با زدن دکمه زیر به صفحه پرداخت هدایت خواهید شد:";
                //message += " \n \n ";

                //sendPaymentMessage(resultLink, message);


            } while (false);



        }

        #endregion

    }
}