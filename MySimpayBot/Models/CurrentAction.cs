using Shared.WebService;
using System;

namespace Models
{
    public class CurrentAction
    {
        public long chatId { get; set; }
        public SimpaySectionEnum section { get; set; }
        public string action { get; set; }
        public string parameter { get; set; }

        public CurrentAction(long thisChatId)
        {
            chatId = thisChatId;
            getInfo();
        }

        public void getInfo()
        {
            do
            {
                var result = DataBase.GetCurrentAction(chatId);

                if (result.ReturnCode != 1 || result.SPCode != 1)
                {
                    Log.Fatal(result.Text, DateTime.Now.Millisecond);
                    break;
                }

                if (result.DataSet.Tables[0].Rows.Count > 0)
                {

                    var ds = Converter.DBNull(result.DataSet);
                    var DR = ds.Tables[0].Rows[0];
                    section = (SimpaySectionEnum)DR["section"];
                    action = (string)DR["action"];
                    parameter = (string)DR["parameter"];
                }

            } while (false);
        }
        public void set(SimpaySectionEnum newSection, string newAction, string newParameters = "")
        {
            section = newSection;
            action = newAction;
            parameter = newParameters;
            var result = DataBase.ReportCurrentAction(chatId, section, action, newParameters);

            if (result.ReturnCode != 1 || result.SPCode != 1)
            {
                Log.Fatal(result.Text, DateTime.Now.Millisecond);
            }

        }
        public void remove()
        {
            if (chatId != 0)
            {
                var result = DataBase.RemoveCurrentAction(chatId);

                if (result.ReturnCode != 1 || result.SPCode != 1)
                {
                    Log.Fatal(result.Text, DateTime.Now.Millisecond);
                }

            }
        }

    }
}