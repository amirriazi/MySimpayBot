
using System;

namespace Models.CinemaTicket
{
    public class CTA_Film
    {
        public CTA_Film_Data Data { get; set; }
        public bool Success { get; set; }
        public int MessageType { get; set; }
        public string Message { get; set; }
        public DateTime ResultDate { get; set; }
        public string Exception { get; set; }
        public string Info { get; set; }

    }

    public class CTA_Film_Data
    {
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public string ProfileLink { get; set; }
        public string Rating { get; set; }
        public int FilmId { get; set; }
        public int FilmCode { get; set; }
        public string FilmName { get; set; }
        public string Director { get; set; }
        public string Producer { get; set; }
        public string Genre { get; set; }
        public string Summary { get; set; }
        public string ReleaseDate { get; set; }
        public string RunningTime { get; set; }
        public string Casting { get; set; }
        public string Distribution { get; set; }
        public string FilmImageUrl { get; set; }
        public string FilmTrailer { get; set; }
    }
}
