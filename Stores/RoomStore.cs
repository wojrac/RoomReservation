using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using RoomReservation.Models;

namespace RoomReservation.Stores
{
    public class RoomStore
    {
        private readonly RegistrationContext Db = new RegistrationContext();
        public Room AddRoom(Room room)
        {
            Db.Rooms.Add(room);
            Db.SaveChanges();
            return room;
        }
        public bool IsNumberReserved(int nrRoom)
        {
            List<Room> rooms = Db.Rooms.ToList();
            bool isExits = false;
            foreach(var item in rooms)
            {
                if (item.Number == nrRoom)
                    isExits = true;
            }
            return isExits;
        }
        public List<Room> GetAllRooms()
        {
            return Db.Rooms.ToList();
        }
        public bool DeleteRoomByID(long roomId)
        {
            Room room = Db.Rooms.Where(r => r.RoomId == roomId).FirstOrDefault();
            List<Reservation> reservations = Db.Reservations.ToList();
            bool isReservd = false;
            foreach (var item in reservations)
            {
                if (item.RoomId == roomId)
                    isReservd = true;
            }
            if (isReservd)
            {
                return false;
            }
            else
            {
                Db.Rooms.Remove(room);
                Db.SaveChanges();
                return true;
            }
        }
        public Room GetRoomById(long roomId)
        {
            Room room = Db.Rooms.Where(r => r.RoomId == roomId).FirstOrDefault();
            return room;
               
        }
        public void EditRoom(Room updatedRoom, long id)
        {
            var reservation = Db.Rooms.Where(r => r.RoomId == id).FirstOrDefault();
            reservation.UpdateRoom(updatedRoom);
            Db.SaveChanges();


        }

    }
}