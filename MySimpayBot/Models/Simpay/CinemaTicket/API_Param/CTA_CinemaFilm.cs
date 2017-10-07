using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.CinemaTicket
{
    public class CTA_CinemaFilm
    {
        public CTA_CinemaFilm_Data[] Data { get; set; }
        public bool Success { get; set; }
        public int MessageType { get; set; }
        public string Message { get; set; }
        public DateTime ResultDate { get; set; }
        public string Exception { get; set; }
        public string Info { get; set; }

    }
    public class CTA_CinemaFilm_Data
    {
        public int CinemaCode { get; set; }
        public string CinemaName { get; set; }
        public string City { get; set; }
        public string Address { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public string Phone { get; set; }
        public string Photo_Url { get; set; }
        public string BuyTicketOnline { get; set; }
    }

}
