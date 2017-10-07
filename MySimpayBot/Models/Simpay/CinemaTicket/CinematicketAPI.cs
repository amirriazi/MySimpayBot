using Newtonsoft.Json;
using Shared.WebService;
using System;
using System.Collections.Generic;
using System.Data;

namespace Models.CinemaTicket
{

    public class CinematicketAPI
    {
        public const string API_URL = "http://api.cinematicket.org/mobile/v1";
        public const string WEB_URL = "http://www.cinematicket.org";
        public static GeneralResultAction resultAction { get; set; }

        public static CTA_Onscreen GetOnScreen(int categoryId)
        {

            var RES = new CTA_Onscreen();
            resultAction = new GeneralResultAction();
            do
            {
                var onScreenResult = CallAPI($"onscreen/select/{categoryId}/200");
                if (onScreenResult == "")
                {
                    resultAction = new GeneralResultAction("GetOnScreen", true, "متاسفانه فعلا فیلمی جهت مشاهده پیدا نکردم.");
                    break;
                }
                var OnScreen = JsonConvert.DeserializeObject<CTA_Onscreen>(onScreenResult);
                RES = OnScreen;
                //for (int i = 0; i < OnScreen.Data.Length; i++)
                //{
                //    RES.Data[i].CategoryId = OnScreen.Data[i].CategoryId;
                //    RES.Data[i].CategoryName = OnScreen.Data[i].CategoryName;
                //    RES.Data[i].FilmCode = OnScreen.Data[i].FilmCode;
                //    RES.Data[i].FilmName = OnScreen.Data[i].FilmName;
                //    RES.Data[i].FilmImageUrl = OnScreen.Data[i].FilmImageUrl;

                //}
            } while (false);
            return RES;


        }

        public static CTA_Film GetFilm(int filmCode)
        {
            var RES = new CTA_Film();
            resultAction = new GeneralResultAction();
            do
            {
                var getFilmResult = CallAPI($"film/get/{filmCode}");
                if (getFilmResult == "")
                {
                    resultAction = new GeneralResultAction("GetOnScreen", true, "متاسفانه اطلاعات فیلم درخواستی در دسترس نیست.");
                    break;
                }
                RES = JsonConvert.DeserializeObject<CTA_Film>(getFilmResult);
                //for (int i = 0; i < OnScreen.Data.Length; i++)
                //{
                //    RES.Data[i].CategoryId = OnScreen.Data[i].CategoryId;
                //    RES.Data[i].CategoryName = OnScreen.Data[i].CategoryName;
                //    RES.Data[i].FilmCode = OnScreen.Data[i].FilmCode;
                //    RES.Data[i].FilmName = OnScreen.Data[i].FilmName;
                //    RES.Data[i].FilmImageUrl = OnScreen.Data[i].FilmImageUrl;

                //}
            } while (false);
            return RES;



        }


        public static CTA_CinemaFilm GetCinemaOfFilm(int filmCode)
        {

            var RES = new CTA_CinemaFilm();

            resultAction = new GeneralResultAction();
            do
            {
                var onScreenResult = CallAPI($"cinema/film/{filmCode}");
                if (onScreenResult == "")
                {
                    resultAction = new GeneralResultAction("GetCinemaOfFilm", true, "متاسفانه سنمایی جهت این فیلم برای مشاهده پیدا نکردم.");
                    break;
                }
                var result = JsonConvert.DeserializeObject<CTA_CinemaFilm>(onScreenResult);
                RES = result;
            } while (false);
            return RES;


        }

        public static CTA_Sanse GetSanseOfCinema(int cinemaCode, int filmCode = 0)
        {

            var RES = new CTA_Sanse();

            resultAction = new GeneralResultAction();
            do
            {
                var onScreenResult = CallAPI($"sanse/select/{cinemaCode}/{filmCode}");
                if (onScreenResult == "")
                {
                    resultAction = new GeneralResultAction("GetSanseOfCinemaFilm", true, "متاسفانه سانس پخش فیلم در این سانس یافت نشد.");
                    break;
                }
                var result = JsonConvert.DeserializeObject<CTA_Sanse>(onScreenResult);
                RES = result;
            } while (false);
            return RES;


        }



        private static string CallAPI(string method, object Param = null)
        {
            var RES = "";
            var header = new System.Collections.Specialized.NameValueCollection();
            header.Add("Content-Type", "application/json; charset=utf-8");
            try
            {
                var postParameters = Param != null ? Utils.ConvertClassToJson(Param) : null;
                var result = Utils.WebRequestByUrl($"{API_URL}/{method}", postParameters, header);
                Log.Info($"Cinematicket-param:{postParameters} \n \n  response={Utils.ConvertClassToJson(result)}", 0);
                if (result.status == System.Net.WebExceptionStatus.Success)
                {
                    RES = result.responseText;
                }
                else
                {
                    Log.Fatal(result.statusMessage, 0);
                }

            }
            catch (Exception ex)
            {
                Log.Fatal(ex.Message, 0);
                throw;
            }
            return RES;
        }

    }
}
