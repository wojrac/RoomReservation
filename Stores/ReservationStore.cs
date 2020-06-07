using RoomReservation.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;

namespace RoomReservation.Stores
{
    public class ReservationStore
    {
        private readonly RegistrationContext DB = new RegistrationContext();
        public Reservation AddReservation(Reservation reservation)
        {
            DB.Reservations.Add(reservation);
            DB.SaveChanges();
            return reservation;
        }
        public Reservation GetReservationById(long id)
        {
            var reservation = DB.Reservations.Where(r => r.ReservationId == id).FirstOrDefault();
            return reservation;
        }
        public List<int> GetAllRoomsNumbers()
        {
            var rooms = DB.Rooms.ToList();
            List<int> nums = new List<int>();
            foreach(var room in rooms)
            {
                nums.Add(room.Number);
            }
            return nums;
        }
        public long GetIdByNumber(int num)
        {
            Room room = DB.Rooms.Where(r => r.Number == num).FirstOrDefault();
            return room.RoomId;
        }
        private float getPrizeByNumber(int num)
        {
            Room room = DB.Rooms.Where(r => r.Number == num).FirstOrDefault();
            return room.PrizePerDay;
        }
        public float getWholeCost(int num,DateTime start,DateTime end)
        {
            float prize = this.getPrizeByNumber(num);
            TimeSpan ts = end.Subtract(start);
            int days = ts.Days;
            float wholeCost = days * prize;
            return wholeCost;          
        }
       public bool IsRoomFree(int numRoom, DateTime start, DateTime end)
        {
            long idSpecifiedRoom = DB.Rooms.Where(r => r.Number == numRoom).FirstOrDefault().RoomId;
            List<Reservation> reservationsOfSpecifiedRoom = DB.Reservations.Where(n => n.RoomId == idSpecifiedRoom).ToList();
            foreach(var item in reservationsOfSpecifiedRoom)
            {          
                TimeSpan startEndSpan1 = start.Subtract(item.EndDate);
                int diffSE1 = startEndSpan1.Days;
                TimeSpan startEndSpan2 = item.StartDate.Subtract(end);
                int diffSE2 = startEndSpan2.Days;          
                if (!(diffSE1 > 0 || diffSE2 > 0)) 
                {                
                    return false;
                }          
            }
            return true;
        }
        public bool IsRoomFreeEdit(int numRoom, DateTime start, DateTime end, long resId)
        {
            // bool isAllFree = true;
            long idSpecifiedRoom = DB.Rooms.Where(r => r.Number == numRoom).FirstOrDefault().RoomId;
            List<Reservation> reservationsOfSpecifiedRoom = DB.Reservations.Where(n => n.RoomId == idSpecifiedRoom).ToList();
            List<Reservation> consideratedReservations = new List<Reservation>();
            foreach(var item in reservationsOfSpecifiedRoom)
            {
                if (item.ReservationId != resId)
                    consideratedReservations.Add(item);
            }
            
            foreach (var item in consideratedReservations)
            {
          
                TimeSpan startEndSpan1 = start.Subtract(item.EndDate);
                int diffSE1 = startEndSpan1.Days;
                TimeSpan startEndSpan2 = item.StartDate.Subtract(end);
                int diffSE2 = startEndSpan2.Days;      
                if (!(diffSE1 > 0 || diffSE2 > 0))
                {

                    return false;
                }

            }
            return true;
        }
        public List<Reservation> GetAllReservationsByUserId(string nick)
        {
            var userId = DB.Users.Where(n => n.NickName == nick).FirstOrDefault().UserId;
            List<Reservation> res = new List<Reservation>();
            res = DB.Reservations.Where(n => n.UserId == userId).ToList();
            return res;
        }
        public void DeleteReservation(long id)
        {
            var res = DB.Reservations.Where(r => r.ReservationId == id).FirstOrDefault();
            DB.Reservations.Remove(res);
            DB.SaveChanges();
        }
        public void EditReservation(Reservation updatedReservation, long id)
        {
            var reservation = DB.Reservations.Where(r => r.ReservationId == id).FirstOrDefault();
            reservation.UpdateReservation(updatedReservation);
            DB.SaveChanges();
         

        }
        

    }
}