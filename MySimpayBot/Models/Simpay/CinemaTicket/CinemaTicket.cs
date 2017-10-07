using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.CinemaTicket
{
    public class CinemaTicket
    {
        public long id { get; set; }
        public int filmCode { get; set; }
        public int cinemaCode { get; set; }
        public int sansCode { get; set; }
        public string seats { get; set; }
        public int seatCount
        {
            get { return string.IsNullOrEmpty(seats) ? 0 : seats.Count(f => f == ';'); }
        }

        public string firstName { get; set; }
        public string lastName { get; set; }
        public int amount { get; set; }
        public string saleKey { get; set; }
        public string status { get; set; }
        public CinemaTicketExtraInfo extraInfo { get; set; }

    }

    public class CinemaTicketExtraInfo
    {
        public string cinemaName { get; set; }
        public DateTime date { get; set; }
        public string filmName { get; set; }
        public string fullName { get; set; }
        public string reserveCode { get; set; }
        public string salonName { get; set; }
        public List<SeatNumberStructure> seats { get; set; }
        public int ticketCount { get; set; }
        public string time { get; set; }
        public int totalAmount { get; set; }

    }
    public class SeatNumberStructure
    {
        public int number { get; set; }
        public string rowNumber { get; set; }

    }
    public class OnScreen
    {

        public DateTime resultDateTime { get; set; }

        public bool success { get; set; }

        public int messageType { get; set; }

        public string message { get; set; }

        public string exception { get; set; }

        public string info { get; set; }
        public List<OnScreen_Data> data { get; set; }

    }


    public class OnScreen_Data
    {
        public int filmCode { get; set; }

        public string filmName { get; set; }

        public int? categoryId { get; set; }

        public string categoryName { get; set; }

        public string filmImageUrl { get; set; }
        public string releaseDateShamsi { get; set; }
    }
    public class OnScreenPaging
    {
        public int maxRecord { get; set; }
        public FilmData data { get; set; }
    }

    public class FilmData
    {
        public int categoryId { get; set; }
        public string categoryName { get; set; }
        public string profileLink { get; set; }
        public string rating { get; set; }
        public int filmId { get; set; }
        public int filmCode { get; set; }
        public string filmName { get; set; }
        public string director { get; set; }
        public string producer { get; set; }
        public string genre { get; set; }
        public string summary { get; set; }
        public string releaseDate { get; set; }
        public string runningTime { get; set; }
        public string casting { get; set; }
        public string distribution { get; set; }
        public string filmImageUrl { get; set; }
        public string filmTrailer { get; set; }

        public List<int> relatedCinemas { get; set; }

    }

    public class CinemaData
    {
        public int cinemaCode { get; set; }
        public string cinemaName { get; set; }
        public string city { get; set; }
        public string address { get; set; }
        public string latitude { get; set; }
        public string longitude { get; set; }
        public string phone { get; set; }
        public string photoUrl { get; set; }
        public bool buyTicketOnline { get; set; }

    }

    public class SansData
    {
        public int sansCode { get; set; }
        public int cinemaCode { get; set; }
        public string showDate { get; set; }
        public string showDay { get; set; }
        public int filmCode { get; set; }
        public string filmName { get; set; }
        public string filmImage { get; set; }
        public string sansHour { get; set; }
        public string salon { get; set; }
        public int sansPrice { get; set; }
        public bool buyTicket { get; set; }
        public string discount { get; set; }
        public int sansPriceDiscount { get; set; }
        public int discountRemain { get; set; }

    }

    public class SansePaging
    {
        public int maxRecord { get; set; }
        public SansData data { get; set; }
    }

    public class SeatsInfo
    {
        public int maxSeatsColumnsCount { get; set; }
        public List<SeatData> seats { get; set; }
    }

    public class SeatData
    {

        public int seatNumber { get; set; }

        public string realRowNumber { get; set; }

        public string realSeatNumber { get; set; }

        public string rowNumber { get; set; }

        public int state { get; set; }

        public bool selected { get; set; }

    }

    public class SeatPaging
    {
        public int maxColSection { get; set; }
        public int rowCount { get; set; }

        public List<SeatData> data { get; set; }
    }



}
