using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class CallBackTransactionInput
    {
        public string SaleKey { get; set; }
        public int Status { get; set; }
        public int ProductId { get; set; }
        public string Description { get; set; }

    }
    public class CallBackTransactionOutput
    {
        public bool HasError { get; set; }
        public string Message { get; set; }
    }

}
