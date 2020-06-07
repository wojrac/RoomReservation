using RoomReservation.Models;
using RoomReservation.Stores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Diagnostics;

namespace RoomReservation.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            TempData["IsLogged"] = false;
            return View();
        }
        public ActionResult Register()
        {
            TempData["IsLogged"] = false;
            return View();
        }
        [HttpPost]
        public ActionResult AddUser(User user)
        {
            TempData["IsLogged"] = false;
            UserStore userStore = new UserStore();
            bool isExists = userStore.IsNickExists(user.NickName);
            if (!ModelState.IsValid || isExists)
            {
                if (isExists)
                {
                    //user.Message = "Nick już istenieje"; 
                    TempData["Message"] = "Nick jest zajęty, spróbuj inny";
                }
                return View("Register", user);
            }
            else
            {
                userStore.AddUser(user);
                return View("Register");
            }
        }
        public ActionResult Login()
        {
            TempData["IsLogged"] = false;
            return View();
        }
        [HttpPost]
        public ActionResult LogIn2(string nick, string password)
        {
            UserStore userStore = new UserStore();
            Models.User user = new Models.User();
            string Nick = nick;
            string Pass = password;
            //Debug.WriteLine(Nick+ " "+ Pass);
            bool isOk = userStore.IsPassesOk(Nick, Pass);
            //TempData["BadDates"] = " ";
            if (isOk)
            {
                user = userStore.GetUserByNick(Nick);
                //user.NickName = nick;
                //ViewBag.nick = nick;
                // return View("Logged",user);
                TempData["IsLogged"] = true;
                TempData["mydata"] = user;
                return RedirectToAction("Login3", "Home");

            }
            else
            {
                TempData["IsLogged"] = false;
                TempData["Alert"] = "Złe dane";
                return View("Login");
            }
        }
        public ActionResult Login3()
        {
            TempData["IsLogged"] = true;
            User user = (User)TempData["mydata"];
            // string us = user.NickName;
            //user.NickName = ViewBag.nick;

            return View("Logged", user);
        }

        public ActionResult Logout()
        {
            TempData["IsLogged"] = false;
            return View("Login");
        }

        public ActionResult Contact()
        {
            TempData["IsLogged"] = true;
            ViewBag.Message = "Your contact page.";
            return View();
        }
        [HttpPost]
        public ActionResult CreateReservation(string userNick) //Nums jest nullem gdy podamy zle daty
        {
            TempData["IsLogged"] = true;
            //TempData["BadDates"] = " ";
            ReservationStore reservationStore = new ReservationStore();
            UserStore userStore = new UserStore();
            List<int> nums = reservationStore.GetAllRoomsNumbers();
            Reservation res = new Reservation();
            //musze pobrac id user po nicku
            User user = userStore.GetUserByNick(userNick);
            res.UserId = user.UserId;   //ok mam tu userId, ktory jest zalogowany
            res.Nums = nums;
            TempData["mydata"] = user;
            return View(res);
        }
        
        public ActionResult ShowReservations(string userNick)
        {
            TempData["IsLogged"] = true;
            ReservationStore reservationStore = new ReservationStore();
            Reservation reservation = new Reservation();
            reservation.Reservations = reservationStore.GetAllReservationsByUserId(userNick);
            UserStore userStore = new UserStore();
            var user = userStore.GetUserByNick(userNick);
            // TempData["nick"] = userNick;
            //ViewBag.nick = userNick;
            ViewData["nick"] = userNick;
            TempData["mydata"] = user;
            return View("ReservationsOfUser",reservation);
        }
        [HttpPost]
        public ActionResult DeleteReservation(long resId, string userNick)
        {
            TempData["IsLogged"] = true;
            //string uNick = (string)TempData["nick"];
            // Reservation reservation = (Reservation)TempData["res"]; 
            ReservationStore reservationStore = new ReservationStore();
            reservationStore.DeleteReservation(resId);
            string nick = userNick;
            // return View("ShowReservations",reservation);
            return RedirectToAction("ShowReservations", new { userNick = nick});
            
        }
        [HttpPost]
        public ActionResult GoToEdit(long resId)
        {
            TempData["IsLogged"] = true;
            ReservationStore reservationStore = new ReservationStore();
            Reservation reservation = reservationStore.GetReservationById(resId);
            //List<int> nums = reservationStore.GetAllRoomsNumbers();
            //reservation.Nums = nums;
            return View("EditReservation",reservation);
        }
        
        [HttpPost]
        public ActionResult Edit( DateTime startDay, DateTime endDay, long resId)
        {
            TempData["IsLogged"] = true;
            ReservationStore reservationStore = new ReservationStore();
            Reservation oldRes = reservationStore.GetReservationById(resId);
            Reservation updReservation = oldRes;
           
           // updReservation.Room.RoomId = reservationStore.GetIdByNumber(rooms);
            
            TempData["BadDates"] = "";
            if (!updReservation.ValidateDatesOfReservation())
            {
                TempData["BadDates"] = "Zle daty!, Edycja nieudana";
                return View("EditReservation", updReservation);
            }
            else
            {
                if (!reservationStore.IsRoomFreeEdit(oldRes.Room.Number, startDay, endDay,resId))
                {
                    TempData["BadDates"] = "Pokój zajęty, Edycja nieudana";
                    return View("EditReservation", updReservation);
                }
                else
                {
                    updReservation.StartDate = startDay;
                    updReservation.EndDate = endDay;
                    updReservation.WholeCost = reservationStore.getWholeCost(oldRes.Room.Number, startDay, endDay);
                    reservationStore.EditReservation(updReservation, resId);
                    TempData["BadDates"] = "Dokonano edycji rezerwacji";
                    string nick = oldRes.User.NickName;
                    return RedirectToAction("ShowReservations", new { userNick = nick });
                }
            }
           
        }
        [HttpPost]
        public ActionResult AddReservation(int rooms,DateTime startDay, DateTime endDay , long hiddenUserId)
        {
            //ReservationStore reservationStore1 = new ReservationStore();
            TempData["IsLogged"] = true;
            int numberOfRoom = rooms;
            Reservation reservation = new Reservation();
            ReservationStore reservationStore = new ReservationStore();
            List<int> nums = reservationStore.GetAllRoomsNumbers();
            UserStore userStore = new UserStore();           
            reservation.UserId = hiddenUserId;
            User user = userStore.GetUserById(hiddenUserId);
            reservation.RoomId = reservationStore.GetIdByNumber(numberOfRoom);
            reservation.StartDate =startDay;
            reservation.EndDate = endDay;
            reservation.Nums = nums;
            reservation.WholeCost = reservationStore.getWholeCost(numberOfRoom, startDay, endDay);
            //TempData["BadDates"] = "";
            if (!reservation.ValidateDatesOfReservation())
            {
                TempData["BadDates"] = "Zle daty!, Rezerwacja nieudana";
                return View("CreateReservation",reservation);
            }
            else
            {
                if (!reservationStore.IsRoomFree(rooms, startDay, endDay))
                {
                    TempData["BadDates"] = "Pokój zajęty, Rezerwacja nieudana";
                    return View("CreateReservation", reservation);
                }
                else
                {
                    reservationStore.AddReservation(reservation);
                    TempData["BadDates"] = "Dokonano rezerwacji";
                    return View("CreateReservation", reservation);
                }
            }
        }

        [HttpPost]
        public ActionResult CreateRoom(string userNick, Role role)
        {
            TempData["IsLogged"] = true;
            User user = new User();
            user.NickName = userNick;
            user.UserRole = role;
            TempData["mydata"] = user;
            return View();
        }
        [HttpGet]
        public ActionResult ShowRooms(string userNick, Role role)
        {
            TempData["IsLogged"] = true;
            // TempData["BadDelete"] = "";
            User user = new User();
            user.NickName = userNick;
            user.UserRole = role;
            TempData["mydata"] = user;
            Room room = new Room();
            RoomStore roomStore = new RoomStore();
            room.AllRooms = roomStore.GetAllRooms();
            TempData["user2"] = user;
            return View("AllRooms",room);
        }
        [HttpPost]
        public ActionResult DeleteRoom(long roomId)
        {
            TempData["IsLogged"] = true;
            User user = (User)TempData["user2"];
            RoomStore roomStore = new RoomStore();
            if(!roomStore.DeleteRoomByID(roomId))
            {
                TempData["BadDelete"] = "Nie mozna usunac pokoju";
            }
            else
            {
                TempData["BadDelete"] = "Pokoj usunieto";
            }
            return RedirectToAction("ShowRooms", new { userNick = user.NickName, role = user.UserRole });
        }
        [HttpPost]
        public ActionResult GoToEditRoom(long roomId)
        {
            TempData["IsLogged"] = true;
            RoomStore roomStore = new RoomStore();
            Room room = roomStore.GetRoomById(roomId);
            
            return View("EditRoom", room);
        }
        [HttpPost]
        public ActionResult EditRoom(float prizePerDay, long roomId)
        {
            TempData["IsLogged"] = true;
            User user = (User)TempData["user2"];
            RoomStore roomStore = new RoomStore();
            Room oldRoom = roomStore.GetRoomById(roomId);
            Room updRoom = oldRoom;
            updRoom.PrizePerDay = prizePerDay;
            roomStore.EditRoom(updRoom,roomId);
            return RedirectToAction("ShowRooms", new { userNick = user.NickName, role = user.UserRole });
        }
        [HttpPost]
        public ActionResult AddRoom(int numberRoom, float prizePerDay)
        {
            TempData["IsLogged"] = true;
            TempData["BadNumber"] = "";
            Room room = new Room();
            RoomStore roomStore = new RoomStore();
            if (roomStore.IsNumberReserved(numberRoom) )
            {
                TempData["BadNumber"] = "Taki numer już istnieje. Dodawanie nie powiodlo sie";
                return View("CreateRoom");
            }
            /*else if(numberRoom =="" || prizePerDay ==0)
            {
                TempData["BadNumber"] = "Nie podano wystarczajych danych";
                return View("CreateRoom");
            }*/
            else
            {
                room.Number = numberRoom;
                room.PrizePerDay = prizePerDay;
                roomStore.AddRoom(room);
                TempData["BadNumber"] = "Dodano pokój";
                return View("CreateRoom");
            }
        }
      
    }
}