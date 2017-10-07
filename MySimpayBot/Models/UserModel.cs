using System;
using Shared.WebService;

namespace Models
{
    public class UserModel
    {
        public long chatId { get; set; }
        public long userId { get; set; }

        public string firstName { get; set; }
        public string lastName { get; set; }
        public string userName { get; set; }
        public long introduceBy { get; set; }
        public string mobileNumber { get; set; }
        public bool activated { get; set; }
        public string activationCode { get; set; }
        public string JsonWebToken { get; set; }
        public string invitationCode { get; set; }
        public string linkAction { get; set; }
        public bool isAdmin { get; set; }
        public bool isNewUser { get; set; }

        public UserModel()
        {

        }

        public UserModel(long userChatId)
        {
            chatId = userChatId;
            Def.MyDbLogger.UserUID = chatId.ToString();
            getInfo();

        }



        private void getInfo()
        {
            do
            {
                var result = DataBase.GetUser(chatId);

                if (result.ReturnCode != 1 || result.SPCode != 1)
                {
                    Log.Fatal(result.Text, DateTime.Now.Millisecond);
                    break;
                }

                if (result.DataSet.Tables[0].Rows.Count > 0)
                {
                    var ds = Converter.DBNull(result.DataSet);
                    var DR = ds.Tables[0].Rows[0];
                    userId = (int)DR["userId"];
                    firstName = (string)DR["firstName"];
                    lastName = (string)DR["lastName"];
                    userName = (string)DR["userName"];
                    introduceBy = (int)DR["introducedBy"];
                    mobileNumber = (string)DR["mobileNumber"];
                    activated = (bool)DR["activated"];
                    activationCode = (string)DR["activationCode"];
                    JsonWebToken = (string)DR["JsonWebToken"];
                    invitationCode = (string)DR["invitationCode"];
                    isAdmin = (bool)DR["isAdmin"];
                    isNewUser = false;
                }

            } while (false);

        }
        public void reportUser()
        {
            do
            {
                var result = DataBase.ReportUser(chatId, firstName, lastName, userName, introduceBy, mobileNumber, activated, activationCode, JsonWebToken, invitationCode, linkAction);

                if (result.ReturnCode != 1 || result.SPCode != 1)
                {
                    Log.Fatal(result.Text, DateTime.Now.Millisecond);
                    break;
                }
                if (result.DataSet.Tables[0].Rows.Count > 0)
                {
                    var ds = Converter.DBNull(result.DataSet);
                    var DR = ds.Tables[0].Rows[0];
                    userId = (int)DR["userId"];
                    isNewUser = (bool)DR["isNewUser"];
                }

            } while (false);

        }
        public void reportBlocked(long thisChatId)
        {
            chatId = thisChatId;
            reportBlocked();
        }

        public void reportBlocked()
        {
            do
            {
                var result = DataBase.ReportUserBlocked(chatId);

                if (result.ReturnCode != 1 || result.SPCode != 1)
                {
                    Log.Fatal(result.Text, DateTime.Now.Millisecond);
                    break;
                }

            } while (false);

        }


    }
}