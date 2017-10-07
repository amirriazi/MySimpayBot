using Newtonsoft.Json;
using Shared.WebService;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;

namespace Models.CinemaTicket
{
    public class Manager
    {

        public long chatId { get; set; }

        public GeneralResultAction resultAction { get; set; }
        public CinemaTicket data { get; set; }

        public List<FilmData> films { get; set; }
        public List<CinemaData> cinemas { get; set; }


        public Manager(long currentChatId)
        {
            chatId = currentChatId;
            films = new List<FilmData>();
            cinemas = new List<CinemaData>();
        }
        public Manager(long currentChatId, long id)
        {
            chatId = currentChatId;
            data = new CinemaTicket { id = id };
            if (id != 0)
            {
                getInfo();
            }
        }

        public Manager(long currentChatId, bool getLastId)
        {
            chatId = currentChatId;
            data = new CinemaTicket { id = 0 };

            if (getLastId)
            {
                getInfo(true);
            }
        }
        public void getInfo(bool baseOnChatId = false)
        {
            do
            {
                var result = new QueryResult();
                if (baseOnChatId)
                {
                    result = DataBase.GetCinemaTicketTransactionByChatId(chatId);
                }
                else
                {
                    result = DataBase.GetCinemaTicketTransaction(data.id);
                }

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
                foreach (DataRow record in DS.Tables[0].Rows)
                {
                    data = new CinemaTicket()
                    {
                        id = (long)record["id"],
                        filmCode = (int)record["filmCode"],
                        cinemaCode = (int)record["cinemaCode"],
                        sansCode = (int)record["sansCode"],
                        seats = (string)record["seats"],
                        firstName = (string)record["firstName"],
                        lastName = (string)record["lastName"],
                        amount = (int)record["amount"],
                        saleKey = (string)record["saleKey"],
                        status = (string)record["status"],
                    };
                }


            } while (false);

        }

        public void setInfo()
        {
            do
            {
                var result = DataBase.SetCinemaTicketTransaction(chatId, data.id, data.filmCode, data.cinemaCode, data.sansCode, data.seats, data.firstName, data.lastName, data.amount, data.saleKey, data.status);
                if (result.ReturnCode != 1 || result.SPCode != 1)
                {
                    Log.Fatal(result.Text, DateTime.Now.Millisecond);
                    break;
                }
                var DS = Converter.DBNull(result.DataSet);
                var DR = DS.Tables[0].Rows[0];
                data.id = Convert.ToInt32(DR["id"]);
            } while (false);


        }

        public OnScreenPaging GetOnScreenMoviesData(long id, int pageNumber)
        {
            var RES = new OnScreenPaging() { maxRecord = 0 };
            RES.data = new FilmData();
            do
            {
                var result = DataBase.GetCinemaTicketOnScreenData(chatId, 0, pageNumber);
                if (result.ReturnCode != 1 || result.SPCode != 1)
                {
                    Log.Fatal(result.Text, DateTime.Now.Millisecond);
                    break;
                }
                var DS = Converter.DBNull(result.DataSet);
                foreach (DataRow record in DS.Tables[0].Rows)
                {
                    RES.data = new FilmData()
                    {
                        filmCode = (int)record["filmCode"],
                        filmName = (string)record["filmName"],
                        filmId = (int)record["filmId"],
                        categoryId = (int)record["categoryId"],
                        categoryName = (string)record["categoryName"],
                        profileLink = (string)record["profileLink"],
                        rating = (string)record["rating"],
                        director = (string)record["director"],
                        producer = (string)record["producer"],
                        genre = (string)record["genre"],
                        summary = (string)record["summary"],
                        releaseDate = (string)record["releaseDate"],
                        runningTime = (string)record["runningTime"],
                        casting = (string)record["casting"],
                        distribution = (string)record["distribution"],
                        filmImageUrl = (string)record["filmImageUrl"],
                        filmTrailer = (string)record["filmTrailer"],
                    };
                }

                RES.maxRecord = (int)DS.Tables[1].Rows[0][0];


            } while (false);
            return RES;

        }

        public FilmData GetFilmInfo(int filmCode)
        {
            var res = new FilmData();
            do
            {
                var result = DataBase.GetCinemaTicketOnScreenData(chatId, filmCode, 1);
                if (result.ReturnCode != 1 || result.SPCode != 1)
                {
                    Log.Fatal(result.Text, DateTime.Now.Millisecond);
                    break;
                }
                var DS = Converter.DBNull(result.DataSet);
                foreach (DataRow record in DS.Tables[0].Rows)
                {
                    res = new FilmData()
                    {
                        filmCode = (int)record["filmCode"],
                        filmName = (string)record["filmName"],
                        filmId = (int)record["filmId"],
                        categoryId = (int)record["categoryId"],
                        categoryName = (string)record["categoryName"],
                        profileLink = (string)record["profileLink"],
                        rating = (string)record["rating"],
                        director = (string)record["director"],
                        producer = (string)record["producer"],
                        genre = (string)record["genre"],
                        summary = (string)record["summary"],
                        releaseDate = (string)record["releaseDate"],
                        runningTime = (string)record["runningTime"],
                        casting = (string)record["casting"],
                        distribution = (string)record["distribution"],
                        filmImageUrl = (string)record["filmImageUrl"],
                        filmTrailer = (string)record["filmTrailer"],
                    };
                }


            } while (false);
            return res;

        }


        public List<CinemaData> GetCinemaOfMovie(int filmCode)
        {
            var RES = new List<CinemaData>();
            do
            {
                var ctaCF = CinematicketAPI.GetCinemaOfFilm(filmCode);
                if (CinematicketAPI.resultAction.hasError)
                {
                    Log.Error(CinematicketAPI.resultAction.message, 0);
                    break;
                }
                if (ctaCF.Data == null || ctaCF.Data.Length == 0)
                {
                    break;
                }
                foreach (var item in ctaCF.Data)
                {
                    if (item.BuyTicketOnline == "True")
                    {
                        RES.Add(new CinemaData()
                        {
                            cinemaCode = item.CinemaCode,
                            cinemaName = item.CinemaName,
                            address = item.Address,
                            city = item.City,
                            latitude = item.Latitude,
                            longitude = item.Longitude,
                            phone = item.Phone,
                            photoUrl = item.Photo_Url

                        });

                    }
                }



            } while (false);
            return RES;
        }
        public void UpdateInformation()
        {
            do
            {
                if (!HasCinemaTicketOnScreenExpired())// after 5 hours
                {
                    break;
                }

                var onScreenMovies = new OnScreen();
                onScreenMovies.data = new List<OnScreen_Data>();
                var onScreenResult = new CTA_Onscreen();// CinematicketAPI.GetOnScreen(36);

                films = new List<FilmData>();
                cinemas = new List<CinemaData>();
                //21, 30: مربوط به تئاتر
                int[] arrCat = new int[] { 36, 27, 35, 52 };
                for (int i = 0; i < arrCat.Length; i++)
                {
                    onScreenResult = CinematicketAPI.GetOnScreen(arrCat[i]);
                    if (CinematicketAPI.resultAction.hasError)
                    {
                        Log.Error(CinematicketAPI.resultAction.message, 0);
                        break;
                    }
                    else
                    {
                        onScreenMovies = new OnScreen()
                        {
                            resultDateTime = onScreenResult.ResultDate,
                            messageType = onScreenResult.MessageType,
                            message = onScreenResult.Message,
                            success = onScreenResult.Success,
                            info = onScreenResult.Info,
                            exception = onScreenResult.Exception
                        };
                        if (onScreenResult.Data != null && onScreenResult.Data.Length > 0)
                        {
                            foreach (var item in onScreenResult.Data)
                            {
                                var ctaFilm = CinematicketAPI.GetFilm(item.FilmCode);
                                if (ctaFilm.Data != null)
                                {
                                    films.Add(new FilmData()
                                    {
                                        filmId = ctaFilm.Data.FilmId,
                                        filmCode = ctaFilm.Data.FilmCode,
                                        filmName = ctaFilm.Data.FilmName,
                                        filmImageUrl = ctaFilm.Data.FilmImageUrl,
                                        filmTrailer = ctaFilm.Data.FilmTrailer,
                                        casting = ctaFilm.Data.Casting,
                                        categoryId = ctaFilm.Data.CategoryId,
                                        categoryName = ctaFilm.Data.CategoryName,
                                        director = ctaFilm.Data.Director,
                                        distribution = ctaFilm.Data.Distribution,
                                        genre = ctaFilm.Data.Genre,
                                        producer = ctaFilm.Data.Producer,
                                        profileLink = ctaFilm.Data.ProfileLink,
                                        rating = ctaFilm.Data.Rating,
                                        releaseDate = ctaFilm.Data.ReleaseDate,
                                        runningTime = ctaFilm.Data.RunningTime,
                                        summary = ctaFilm.Data.Summary
                                    });

                                }

                            }

                        }
                    }


                }

                if (films.Count != 0)
                {
                    SetOnScreenMovies(onScreenMovies);
                }
                else
                {
                    BlankOnScreenData();
                }
                if (films.Count != 0)
                {
                    foreach (var film in films)
                    {
                        var ctaCinema = CinematicketAPI.GetCinemaOfFilm(film.filmCode);

                        if (ctaCinema.Data != null && ctaCinema.Data.Length > 0)
                        {
                            foreach (var cinema in ctaCinema.Data)
                            {
                                var tmp = new CinemaData()
                                {
                                    cinemaCode = cinema.CinemaCode,
                                    cinemaName = cinema.CinemaName,
                                    address = cinema.Address,
                                    buyTicketOnline = cinema.BuyTicketOnline == "True" ? true : false,
                                    city = cinema.City,
                                    latitude = cinema.Latitude,
                                    longitude = cinema.Longitude,
                                    phone = cinema.Phone,
                                    photoUrl = cinema.Photo_Url
                                };

                                if (cinemas.FindIndex(obj => obj.cinemaCode == tmp.cinemaCode) == -1)
                                {
                                    cinemas.Add(tmp);
                                }

                            }
                        }

                    }
                    SetOnScreenMovies(onScreenMovies);
                }
                else
                {
                    BlankOnScreenData();
                }




            } while (false);
        }

        public void GetOnScreenMovies()
        {
            do
            {
                var result = DataBase.GetCinemaTicketOnScreen();
                if (result.ReturnCode != 1 || result.SPCode != 1)
                {
                    Log.Fatal(result.Text, DateTime.Now.Millisecond);
                    break;
                }
                var DS = Converter.DBNull(result.DataSet);
                var DR = DS.Tables[0].Rows[0];
                var onScreenMovies = new OnScreen();
                foreach (DataRow record in DS.Tables[0].Rows)
                {
                    onScreenMovies = new OnScreen
                    {
                        resultDateTime = (DateTime)record["resultDateTime"],
                        success = (bool)record["success"],
                        messageType = (int)record["messageType"],
                        message = (string)record["message"],
                        exception = (string)record["exception"],
                        info = (string)record["info"],
                    };
                }
                onScreenMovies.data = new List<OnScreen_Data>();
                if (DS.Tables[1].Rows.Count > 0)
                {
                    foreach (DataRow record in DS.Tables[1].Rows)
                    {
                        onScreenMovies.data.Add(new OnScreen_Data()
                        {
                            filmCode = (int)record["filmCode"],
                            filmName = (string)record["filmName"],
                            categoryId = (int)record["categoryId"],
                            categoryName = (string)record["categoryName"],
                            filmImageUrl = (string)record["filmImageUrl"],
                            releaseDateShamsi = (string)record["releaseDateShamsi"],
                        });
                    }
                }


            } while (false);

        }


        public void InitialSanseOfCinema(int cinemaCode, int filmCode = 0)
        {
            var sansList = new List<SansData>();
            do
            {
                var ctaSCF = CinematicketAPI.GetSanseOfCinema(cinemaCode, filmCode);
                if (CinematicketAPI.resultAction.hasError)
                {
                    Log.Error(CinematicketAPI.resultAction.message, 0);
                    break;
                }
                if (ctaSCF.Data == null || ctaSCF.Data.Length == 0)
                {
                    break;
                }
                foreach (var item in ctaSCF.Data)
                {
                    if (item.BuyTicket)
                    {
                        sansList.Add(new SansData()
                        {
                            sansCode = item.SansCode,
                            cinemaCode = cinemaCode,
                            salon = item.Salon,
                            sansHour = item.SansHour,
                            sansPrice = item.SansPrice,
                            sansPriceDiscount = item.SansPrice_Discount,
                            buyTicket = item.BuyTicket,
                            discount = item.Discount,
                            discountRemain = item.Discount_Remain,
                            filmCode = item.FilmCode,
                            filmName = item.FilmName,
                            filmImage = item.FilmImage,
                            showDate = item.ShowDate,
                            showDay = item.ShowDay

                        });

                    }
                }
                SetSans(sansList);
            } while (false);
        }
        public SansePaging getSans(long id, int pageNumber)
        {
            var res = new SansePaging() { maxRecord = 0 };
            do
            {


                var result = DataBase.GetCinemaTicketSans(id, pageNumber);
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
                foreach (DataRow record in DS.Tables[0].Rows)
                {
                    res.data = new SansData()
                    {
                        sansCode = (int)record["sansCode"],
                        cinemaCode = (int)record["cinemaCode"],
                        showDate = (string)record["showDate"],
                        showDay = (string)record["showDay"],
                        filmCode = (int)record["filmCode"],
                        filmName = (string)record["filmName"],
                        sansHour = (string)record["sansHour"],
                        salon = (string)record["salon"],
                        sansPrice = (int)record["sansPrice"],
                        discount = (string)record["discount"],
                        sansPriceDiscount = (int)record["sansPriceDiscount"],
                        discountRemain = (int)record["discountRemain"],

                    };
                }

                res.maxRecord = (int)DS.Tables[1].Rows[0][0];

            } while (false);
            return res;
        }


        private void SetOnScreenMovies(OnScreen onScreenMovies)
        {
            do
            {
                var result = DataBase.SetCinemaTicketOnScreen(onScreenMovies.success, onScreenMovies.messageType, onScreenMovies.message, onScreenMovies.exception, onScreenMovies.info, films, cinemas);
                if (result.ReturnCode != 1 || result.SPCode != 1)
                {
                    Log.Fatal(result.Text, DateTime.Now.Millisecond);
                    break;
                }


            } while (false);


        }
        private void BlankOnScreenData()
        {

            do
            {

                var result = DataBase.SetCinemaTicketOnScreenBlank();
                if (result.ReturnCode != 1 || result.SPCode != 1)
                {
                    Log.Fatal(result.Text, DateTime.Now.Millisecond);
                    break;
                }
            } while (false);

        }
        private bool HasCinemaTicketOnScreenExpired()
        {
            var expired = false;
            do
            {
                var result = DataBase.HasCinemaTicketOnScreenExpired();
                if (result.ReturnCode != 1 || result.SPCode != 1)
                {
                    Log.Fatal(result.Text, DateTime.Now.Millisecond);
                    break;
                }
                var DS = Converter.DBNull(result.DataSet);
                var DR = DS.Tables[0].Rows[0];
                expired = (bool)DR["expired"];

            } while (false);
            return expired;
        }

        private void SetSans(List<SansData> sansList)
        {
            do
            {
                var result = DataBase.SetCinemaTicketSans(data.id, sansList);
                if (result.ReturnCode != 1 || result.SPCode != 1)
                {
                    Log.Fatal(result.Text, DateTime.Now.Millisecond);
                    break;
                }


            } while (false);


        }



        public void InitialSeatsList(int cinemaCode, int sansCode)
        {
            do
            {
                var wsInput = new wsCinemaTicket.GetCinemaSansSeatsList_Input
                {
                    Identity = new Core.wsInterface.Identity
                    {
                        JsonWebToken = SimpayCore.GetJsonToken(),
                        ServiceName = "cinematicket",
                        ActionName = "GetCinemaSansSeatsList"
                    },
                    Parameters = new wsCinemaTicket.GetCinemaSansSeatsList_Input_Parameters()
                    {
                        SessionID = SimpayCore.getSessionId(),
                        CinemaCode = cinemaCode,
                        SansCode = sansCode
                    }
                };
                var wsOutputResult = SimpayCore.InterfaceApiCall(wsInput);

                if (String.IsNullOrEmpty(wsOutputResult))
                {
                    Log.Error("Error: Cannot read request message!", 0);
                    resultAction = new GeneralResultAction("GetSeatsList", true, "result is empty");
                    break;
                }

                var wsOutput = JsonConvert.DeserializeObject<wsCinemaTicket.GetCinemaSansSeatsList_Output>(wsOutputResult);

                if (wsOutput.Status.Code == "G00002")
                {

                    Log.Error("Error: " + wsOutput.Status.Description, 0);
                    resultAction = new GeneralResultAction("GetSeatsList", true, wsOutput.Status.Description);
                    break;

                }
                else if (wsOutput.Status.Code != "G00000")
                {
                    resultAction = new GeneralResultAction("GetSeatsList", true, wsOutput.Status.Description);
                    break;
                }
                if (wsOutput.Parameters == null)
                {
                    resultAction = new GeneralResultAction("getListOfSourceState", true, "پیدا نشد");
                    break;
                }
                var seatInfo = new SeatsInfo()
                {
                    maxSeatsColumnsCount = wsOutput.Parameters.MaxSeatsColumnsCount,
                    seats = new List<SeatData>()

                };

                foreach (var item in wsOutput.Parameters.Detail)
                {
                    seatInfo.seats.Add(new SeatData()
                    {
                        realRowNumber = item.RealRowNumber,
                        realSeatNumber = item.RealSeatNumber,
                        rowNumber = item.RowNumber,
                        seatNumber = item.SeatNumber,
                        state = item.State,
                        selected = false,

                    });
                }

                setSeats(seatInfo);

                resultAction = new GeneralResultAction();
            } while (false);

        }

        public void setTotalAmount()
        {
            do
            {
                var wsInput = new wsCinemaTicket.CalculateAmount_Input
                {
                    Identity = new Core.wsInterface.Identity
                    {
                        JsonWebToken = SimpayCore.GetJsonToken(),
                        ServiceName = "cinematicket",
                        ActionName = "CalculateAmount"
                    },
                    Parameters = new wsCinemaTicket.CalculateAmount_Input_Parameters()
                    {
                        SessionID = SimpayCore.getSessionId(),
                        CinemaCode = data.cinemaCode,
                        SansCode = data.sansCode,
                        Count = data.seatCount
                    }
                };
                var wsOutputResult = SimpayCore.InterfaceApiCall(wsInput);

                if (String.IsNullOrEmpty(wsOutputResult))
                {
                    Log.Error("Error: Cannot read request message!", 0);
                    resultAction = new GeneralResultAction("setTotalAmount", true, "result is empty");
                    break;
                }

                var wsOutput = JsonConvert.DeserializeObject<wsCinemaTicket.CalculateAmount_Output>(wsOutputResult);

                if (wsOutput.Status.Code == "G00002")
                {

                    Log.Error("Error: " + wsOutput.Status.Description, 0);
                    resultAction = new GeneralResultAction("setTotalAmount", true, wsOutput.Status.Description);
                    break;

                }
                else if (wsOutput.Status.Code != "G00000")
                {
                    resultAction = new GeneralResultAction("setTotalAmount", true, wsOutput.Status.Description);
                    break;
                }
                if (wsOutput.Parameters == null)
                {
                    resultAction = new GeneralResultAction("setTotalAmount", true, "پیدا نشد");
                    break;
                }
                data.amount = wsOutput.Parameters.TotalAmount;
                setInfo();
                resultAction = new GeneralResultAction();
            } while (false);

        }

        public void OrderTicket()
        {
            do
            {
                var wsInput = new wsCinemaTicket.OrderTicket_Input
                {
                    Identity = new Core.wsInterface.Identity
                    {
                        JsonWebToken = SimpayCore.GetJsonToken(),
                        ServiceName = "cinematicket",
                        ActionName = "OrderTicket"
                    },
                    Parameters = new wsCinemaTicket.OrderTicket_Input_Parameters()
                    {
                        SessionID = SimpayCore.getSessionId(),
                        CinemaCode = data.cinemaCode,
                        SansCode = data.sansCode,
                        Count = data.seatCount
                    }
                };
                var wsOutputResult = SimpayCore.InterfaceApiCall(wsInput);

                if (String.IsNullOrEmpty(wsOutputResult))
                {
                    Log.Error("Error: Cannot read request message!", 0);
                    resultAction = new GeneralResultAction("OrderTicket", true, "result is empty");
                    break;
                }

                var wsOutput = JsonConvert.DeserializeObject<wsCinemaTicket.OrderTicket_Output>(wsOutputResult);

                if (wsOutput.Status.Code == "G00002")
                {

                    Log.Error("Error: " + wsOutput.Status.Description, 0);
                    resultAction = new GeneralResultAction("OrderTicket", true, wsOutput.Status.Description);
                    break;

                }
                else if (wsOutput.Status.Code != "G00000")
                {
                    resultAction = new GeneralResultAction("OrderTicket", true, wsOutput.Status.Description);
                    break;
                }
                if (wsOutput.Parameters == null)
                {
                    resultAction = new GeneralResultAction("OrderTicket", true, "پیدا نشد");
                    break;
                }
                data.amount = wsOutput.Parameters.TotalAmount;
                data.saleKey = wsOutput.Parameters.SaleKey;
                setInfo();
                resultAction = new GeneralResultAction();
            } while (false);

        }

        // After customer paid the ticket amount this method should be called!
        public void GetSaleInfoForTicketBuying()
        {
            do
            {
                var wsInput = new wsCinemaTicket.GetSaleInfoForTicketBuying_Input
                {
                    Identity = new Core.wsInterface.Identity
                    {
                        JsonWebToken = SimpayCore.GetJsonToken(),
                        ServiceName = "cinematicket",
                        ActionName = "GetSaleInfoForTicketBuying"
                    },
                    Parameters = new wsCinemaTicket.GetSaleInfoForTicketBuying_Input_Parameters()
                    {
                        SessionID = SimpayCore.getSessionId(),
                        SaleKey = data.saleKey,
                    }
                };
                var wsOutputResult = SimpayCore.InterfaceApiCall(wsInput);

                if (String.IsNullOrEmpty(wsOutputResult))
                {
                    Log.Error("Error: Cannot read request message!", 0);
                    resultAction = new GeneralResultAction("GetSaleInfoForTicketBuying", true, "result is empty");
                    break;
                }

                var wsOutput = JsonConvert.DeserializeObject<wsCinemaTicket.GetSaleInfoForTicketBuying_Output>(wsOutputResult);

                if (wsOutput.Status.Code == "G00002")
                {

                    Log.Error("Error: " + wsOutput.Status.Description, 0);
                    resultAction = new GeneralResultAction("GetSaleInfoForTicketBuying", true, wsOutput.Status.Description);
                    break;

                }
                else if (wsOutput.Status.Code != "G00000")
                {
                    resultAction = new GeneralResultAction("GetSaleInfoForTicketBuying", true, wsOutput.Status.Description);
                    break;
                }
                if (wsOutput.Parameters == null)
                {
                    resultAction = new GeneralResultAction("GetSaleInfoForTicketBuying", true, "پیدا نشد");
                    break;
                }
                data.status = "paid";
                data.cinemaCode = wsOutput.Parameters.CinemaCode;
                data.sansCode = wsOutput.Parameters.SansCode;
                if (data.seatCount != wsOutput.Parameters.TicketCount)
                {
                    resultAction = new GeneralResultAction("GetSaleInfoForTicketBuying", true, "تعداد صندلیهای درخواستی هماهنگی ندارد!");
                    break;
                }
                setInfo();
                resultAction = new GeneralResultAction();
            } while (false);

        }
        public void BuyTicket()
        {
            do
            {
                var arrIntSeats = new int[data.seatCount];
                var arrStringSeats = data.seats.Split(';');
                for (int i = 0; i < arrStringSeats.Length - 1; i++)
                {
                    arrIntSeats[i] = Convert.ToInt32(arrStringSeats[i]);
                }

                var wsInput = new wsCinemaTicket.BuyTicket_Input
                {
                    Identity = new Core.wsInterface.Identity
                    {
                        JsonWebToken = SimpayCore.GetJsonToken(),
                        ServiceName = "cinematicket",
                        ActionName = "BuyTicket"
                    },
                    Parameters = new wsCinemaTicket.BuyTicket_Input_Parameters()
                    {
                        SessionID = SimpayCore.getSessionId(),
                        FirstName = data.firstName,
                        LastName = data.lastName,
                        SaleKey = data.saleKey,
                        SeatsNumber = arrIntSeats,
                    }
                };
                var wsOutputResult = SimpayCore.InterfaceApiCall(wsInput);

                if (String.IsNullOrEmpty(wsOutputResult))
                {
                    Log.Error("Error: Cannot read request message!", 0);
                    resultAction = new GeneralResultAction("OrderTicket", true, "result is empty");
                    break;
                }

                var wsOutput = JsonConvert.DeserializeObject<wsCinemaTicket.BuyTicket_Output>(wsOutputResult);

                if (wsOutput.Status.Code == "G00002")
                {

                    Log.Error("Error: " + wsOutput.Status.Description, 0);
                    resultAction = new GeneralResultAction("OrderTicket", true, wsOutput.Status.Description);
                    break;

                }
                else if (wsOutput.Status.Code != "G00000")
                {
                    resultAction = new GeneralResultAction("OrderTicket", true, wsOutput.Status.Description);
                    break;
                }
                data.status = "done";
                setInfo();
                resultAction = new GeneralResultAction();
            } while (false);

        }

        public void PrintTicket()
        {
            do
            {
                var wsInput = new wsCinemaTicket.PrintTicket_Input
                {
                    Identity = new Core.wsInterface.Identity
                    {
                        JsonWebToken = SimpayCore.GetJsonToken(),
                        ServiceName = "cinematicket",
                        ActionName = "PrintTicket"
                    },
                    Parameters = new wsCinemaTicket.PrintTicket_Input_Parameters()
                    {
                        SessionID = SimpayCore.getSessionId(),
                        SaleKey = data.saleKey
                    }
                };
                var wsOutputResult = SimpayCore.InterfaceApiCall(wsInput);

                if (String.IsNullOrEmpty(wsOutputResult))
                {
                    Log.Error("Error: Cannot read request message!", 0);
                    resultAction = new GeneralResultAction("PrintTicket", true, "result is empty");
                    break;
                }

                var wsOutput = JsonConvert.DeserializeObject<wsCinemaTicket.PrintTicket_Output>(wsOutputResult);

                if (wsOutput.Status.Code == "G00002")
                {

                    Log.Error("Error: " + wsOutput.Status.Description, 0);
                    resultAction = new GeneralResultAction("PrintTicket", true, wsOutput.Status.Description);
                    break;

                }
                else if (wsOutput.Status.Code != "G00000")
                {
                    resultAction = new GeneralResultAction("PrintTicket", true, wsOutput.Status.Description);
                    break;
                }
                if (wsOutput.Parameters == null)
                {
                    resultAction = new GeneralResultAction("PrintTicket", true, "پیدا نشد");
                    break;
                }
                data.extraInfo = new CinemaTicketExtraInfo
                {
                    cinemaName = wsOutput.Parameters.CinemaName,
                    date = wsOutput.Parameters.Date,
                    filmName = wsOutput.Parameters.FilmName,
                    fullName = wsOutput.Parameters.FullName,
                    reserveCode = wsOutput.Parameters.ReserveCode,
                    salonName = wsOutput.Parameters.SalonName,
                    ticketCount = wsOutput.Parameters.TicketCount,
                    time = wsOutput.Parameters.Time,
                    totalAmount = wsOutput.Parameters.TotalAmount,
                    seats = new List<SeatNumberStructure>()
                };
                foreach (var seat in wsOutput.Parameters.Seats)
                {
                    data.extraInfo.seats.Add(new SeatNumberStructure
                    {
                        number = seat.Number,
                        rowNumber = seat.RowNumber
                    });
                }

                resultAction = new GeneralResultAction();
            } while (false);

        }


        private void setSeats(SeatsInfo seatInfo)
        {
            do
            {
                var result = DataBase.SetCinemaTicketSeat(data.id, seatInfo.maxSeatsColumnsCount, seatInfo.seats);
                if (result.ReturnCode != 1 || result.SPCode != 1)
                {
                    Log.Fatal(result.Text, DateTime.Now.Millisecond);
                    break;
                }


            } while (false);

        }

        public SeatPaging getSeats(long id, int row, int colSection = 1)
        {
            var res = new SeatPaging()
            {
                data = new List<SeatData>(),
                maxColSection = 0,
                rowCount = 0
            };
            do
            {


                var result = DataBase.GetCinemaTicketSeats(id, row, colSection);
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
                foreach (DataRow record in DS.Tables[0].Rows)
                {
                    res.data.Add(new SeatData
                    {
                        seatNumber = (int)record["seatNumber"],
                        realRowNumber = (string)record["realRowNumber"],
                        realSeatNumber = (string)record["realSeatNumber"],
                        rowNumber = (string)record["rowNumber"],
                        state = (int)record["state"],
                        selected = (bool)record["selected"],
                    });

                }

                res.maxColSection = (int)DS.Tables[1].Rows[0]["maxColSection"];
                res.rowCount = (int)DS.Tables[1].Rows[0]["rowCount"];

            } while (false);
            return res;
        }

        public void setQuery(string query)
        {
            var result = DataBase.SetCinemaTicketQuery(chatId, query);
            if (result.ReturnCode != 1 || result.SPCode != 1)
            {
                Log.Fatal(result.Text, DateTime.Now.Millisecond);
            }
        }
        public Image DrawPlan()
        {
            Image img = new Bitmap(500, 500);

            var drawing = Graphics.FromImage(img);

            var rectLength = 30;
            var startX = 10;
            var startY = 10;
            var rects = new Rectangle[50];
            for (int i = 0; i < rects.Length; i++)
            {
                rects[i].Width = rectLength;
                rects[i].Height = rectLength;

                rects[i].X = startX;
                rects[i].Y = startY;
                startX += rectLength;
                if (startX > 500)
                {
                    startX = 10;
                    startY += rectLength;
                }
            }
            var pen = new Pen(Color.Black);

            drawing.Clear(Color.White);
            drawing.DrawRectangles(pen, rects);

            drawing.Dispose();
            return img;
        }


    }
}
