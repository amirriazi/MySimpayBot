using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Shared.WebService;
using Newtonsoft.Json;
using System.Data;

namespace Models
{
    public class JobHistory
    {
        public long jobUID { get; set; }

        public string description { get; set; }

        public DateTime? createDateTime { get; set; }

        public DateTime? callDateTime { get; set; }

        public string result { get; set; }

        public JobHistory()
        {

        }
        public JobHistory(long currentUID)
        {
            jobUID = currentUID;
            getInfo();
        }

        public void getInfo()
        {
            do
            {
                var resultDB = DataBase.GetJobHistory(jobUID);
                if (resultDB.ReturnCode != 1 || resultDB.SPCode != 1)
                {
                    Log.Fatal(resultDB.Text, DateTime.Now.Millisecond);
                    break;
                }
                if (resultDB.DataSet.Tables[0].Rows.Count <= 0)
                {
                    break;
                }

                var DS = Converter.DBNull(resultDB.DataSet);
                foreach (DataRow record in DS.Tables[0].Rows)
                {

                    jobUID = (long)record["jobUID"];
                    description = (string)record["description"];
                    createDateTime = (DateTime?)record["createDateTime"];
                    callDateTime = (DateTime?)record["callDateTime"];
                    result = (string)record["result"];
                }


            } while (false);

        }
        public void setInfo()
        {
            do
            {
                if (String.IsNullOrEmpty(description))
                {
                    description = "";
                }
                var resultDB = DataBase.SetJob(jobUID, description, result);
                if (resultDB.ReturnCode != 1 || resultDB.SPCode != 1)
                {
                    Log.Fatal(resultDB.Text, DateTime.Now.Millisecond);
                    break;
                }
                var DS = Converter.DBNull(resultDB.DataSet);
                var DR = DS.Tables[0].Rows[0];
                jobUID = (long)DR["jobUID"];
                //Log.Info($"Get JobUID ={jobUID }", DateTime.Now.Millisecond);

            } while (false);


        }

    }
}