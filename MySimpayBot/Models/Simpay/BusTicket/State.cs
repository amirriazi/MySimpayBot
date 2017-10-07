using Shared.WebService;
using System;
using System.Collections.Generic;
using System.Data;

namespace Models.BusTicket
{
    public class State
    {
        public List<StateData> data { get; set; }

        public State(bool Chache = true)
        {
            data = new List<StateData>();
            if (Chache)
                setDataList();
        }


        public string getShowName(int queryId)
        {
            var ans = "";
            do
            {
                var selectedResult = data.Find(delegate (StateData sd)
                {
                    return (sd.stateCode == queryId);
                });

                if (selectedResult != null)
                    ans = selectedResult.stateShowName;
            } while (false);

            return ans;
        }
        public void setDataList()
        {
            var list = new List<StateData>();
            do
            {
                var result = DataBase.GetBusTicketState();
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
                    list.Add(new StateData()
                    {
                        stateCode = (int)DR["stateCode"],
                        stateShowName = (string)DR["stateName"]
                    });
                }

            } while (false);
            data.AddRange(list);

        }

        public void CacheList()
        {
            var result = DataBase.SetBusTicketState(data);
            if (result.ReturnCode != 1 || result.SPCode != 1)
            {
                Log.Fatal(result.Text, DateTime.Now.Millisecond);
            }

        }

    }

    public class StateData
    {
        public int stateCode { get; set; }
        public string stateShowName { get; set; }
    }

}