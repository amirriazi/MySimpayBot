using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.CinemaTicket
{
    public class CTA_Sanse
    {
        public CTA_Sanse_Data[] Data { get; set; }
        public bool Success { get; set; }
        public int MessageType { get; set; }
        public string Message { get; set; }
        public DateTime ResultDate { get; set; }
        public string Exception { get; set; }
        public string Info { get; set; }

    }
    public class CTA_Sanse_Data
    {
        public int SansCode { get; set; }
        public string ShowDate { get; set; }
        public string ShowDay { get; set; }
        public int FilmCode { get; set; }
        public string FilmName { get; set; }
        public string FilmImage { get; set; }
        public string SansHour { get; set; }
        public string Salon { get; set; }
        public int SansPrice { get; set; }
        public bool BuyTicket { get; set; }
        public string Discount { get; set; }
        public int SansPrice_Discount { get; set; }
        public int Discount_Remain { get; set; }

    }
}
