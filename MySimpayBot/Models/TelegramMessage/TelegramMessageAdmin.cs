using Shared.WebService;
using System;
using System.Data;

namespace Models
{
    public partial class TelegramMessage
    {
        private bool AdminCommands()
        {
            var ans = false;
            do
            {
                if (!thisUser.isAdmin)
                {
                    break;
                }
                var command = message.Text.ToLower();
                if (command.StartsWith("#usercount"))
                {
                    ans = true;
                    AdminGetUserCount(command);
                    break;
                }
                if (command.StartsWith("#useractivatedcount"))
                {
                    ans = true;
                    AdminGetUserCount(command);
                    break;
                }
                if (command == "#link")
                {
                    ans = true;
                    AdminGetLinkCount(false);
                    break;
                }
                if (command == "#linkb")
                {
                    ans = true;
                    AdminGetLinkCount(true);
                    break;
                }


            } while (false);
            return ans;

        }

        private void AdminGetUserCount(string command)
        {
            var msgToSend = "";
            do
            {
                var result = DataBase.Admin(command);
                if (result.ReturnCode != 1 || result.SPCode != 1)
                {
                    msgToSend = result.Text;
                    break;
                }
                if (result.DataSet.Tables[0].Rows.Count <= 0)
                {
                    msgToSend = "Record is empty!";
                    break;
                }
                var DS = Converter.DBNull(result.DataSet);
                var DR = DS.Tables[0].Rows[0];
                var userCount = (int)DR[0];
                telegramAPI.send($"{command}: {userCount}");

            } while (false);

        }

        private void AdminGetLinkCount(bool ExcludeBlockedUser = false)
        {
            var msgToSend = "";
            do
            {
                var result = DataBase.AdminGetLinkCounter(ExcludeBlockedUser);
                if (result.ReturnCode != 1 || result.SPCode != 1)
                {
                    msgToSend = result.Text;
                    break;
                }
                if (result.DataSet.Tables[0].Rows.Count <= 0)
                {
                    msgToSend = "Record is empty!";
                    break;
                }
                var DS = Converter.DBNull(result.DataSet);
                var DR = DS.Tables[0].Rows[0];


                foreach (DataRow record in DS.Tables[0].Rows)
                {
                    msgToSend += $" لینک: {(string)record["action"]} {Environment.NewLine }";
                    msgToSend += $" تعداد ورودی: {(int)record["jointCount"]} {Environment.NewLine }";
                    msgToSend += $" تعداد ثبت نام کرده: {(int)record["registeredCount"]} {Environment.NewLine }";
                    msgToSend += $" تعداد تراکنش : {(int)record["transactionCount"]} {Environment.NewLine }";
                    msgToSend += $" تعداد بلاک شده: {(int)record["blockedCount"]} {Environment.NewLine }";
                    msgToSend += "--------------------------------------------------------";
                    msgToSend += $" {Environment.NewLine } {Environment.NewLine }";
                    telegramAPI.send(msgToSend);
                    msgToSend = "";
                }
                telegramAPI.send("انتهای گزارش");


                //telegramAPI.sendUploadingDocumentStatus();

                //var stream = new System.IO.MemoryStream();
                //stream.Position = 0;
                //ExcelLibrary.DataSetHelper.CreateWorkbook(stream, DS);



                //telegramAPI.fileToSend = new Telegram.Bot.Types.FileToSend
                //{
                //    Content = stream,
                //    Filename = "link.xlsx"
                //};
                //telegramAPI.caption = "فایل گزارش!";
                //telegramAPI.send(Telegram.Bot.Types.Enums.MessageType.DocumentMessage);


            } while (false);
            if (!String.IsNullOrEmpty(msgToSend))
            {
                telegramAPI.send(msgToSend);
            }
        }

    }
}