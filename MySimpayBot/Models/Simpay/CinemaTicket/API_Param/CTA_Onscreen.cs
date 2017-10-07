
using System;

namespace Models.CinemaTicket
{
    public class CTA_Onscreen
    {
        public CTA_Onscreen_Data[] Data { get; set; }
        public bool Success { get; set; }
        public int MessageType { get; set; }
        public string Message { get; set; }
        public DateTime ResultDate { get; set; }
        public string Exception { get; set; }
        public string Info { get; set; }
    }
    public class CTA_Onscreen_Data
    {
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public int FilmCode { get; set; }
        public string FilmName { get; set; }
        public string FilmImageUrl { get; set; }
        public string ReleaseDate { get; set; }


    }
}


