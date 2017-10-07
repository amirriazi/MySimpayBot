using System.Collections.Generic;
using System.Data;

namespace Shared.WebService
{
    public class QueryResult
    {
        private int sPCode;
        private string text;
        private int returnCode;
        private DataSet dataSet;

        public int SPCode
        {
            get { return sPCode; }
            set { sPCode = value; }
        }
        public string Text
        {
            get { return text; }
            set { text = value; }
        }
        public int ReturnCode
        {
            get { return returnCode; }
            set { returnCode = value; }
        }
        public DataSet DataSet
        {
            get { return dataSet; }
            set { dataSet = value; }
        }
        public List<Dictionary<string, object>> Output { get; set; } = new List<Dictionary<string, object>>();
    }
}