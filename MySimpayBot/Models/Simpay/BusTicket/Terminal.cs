using Shared.WebService;
using System;
using System.Collections.Generic;
using System.Data;

namespace Models.BusTicket
{
    public class Terminal
    {
        public List<TerminalData> data { get; set; }

        public Terminal(bool Chache = true)
        {
            data = new List<TerminalData>();
            if (Chache)
                setDataList();
        }


        public string getShowName(int queryId)
        {
            var ans = "";
            do
            {
                var selectedResult = data.Find(delegate (TerminalData sd)
                {
                    return (sd.terminalCode == queryId);
                });

                if (selectedResult != null)
                    ans = selectedResult.terminalShowName;
            } while (false);

            return ans;
        }
        public void setDataList()
        {
            var list = new List<TerminalData>();
            do
            {
                var result = DataBase.GetBusTicketTerminal();
                if (result.ReturnCode != 1 || result.SPCode != 1)
                {
                    Log.Fatal(result.Text, DateTime.Now.Millisecond);
                    break;
                }
                if (result.DataSet.Tables[0].Rows.Count <= 0)
                {
                    break;
                }
                var DS = Converter.DBNull(result.DataSet);
                foreach (DataRow DR in DS.Tables[0].Rows)
                {
                    list.Add(new TerminalData()
                    {
                        terminalCode = (int)DR["terminalCode"],
                        terminalShowName = (string)DR["terminalName"]
                    });
                }

            } while (false);
            data.AddRange(list);

        }

        public void CacheList()
        {
            var result = DataBase.SetBusTicketTerminal(data);
            if (result.ReturnCode != 1 || result.SPCode != 1)
            {
                Log.Fatal(result.Text, DateTime.Now.Millisecond);
            }

        }

    }

    public class TerminalData
    {
        public int terminalCode { get; set; }
        public string terminalShowName { get; set; }
    }

}