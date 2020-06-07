using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace RoomReservation.Models
{
    public class Room
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long RoomId { get; set; }
        public int Number { get; set; }
        public float PrizePerDay { get; set; }
        [NotMapped]
        public List<Room> AllRooms { get; set; }
        public void UpdateRoom(Room updatedRoom)
        {
            //this.RoomId = updatedReservation.RoomId;
            this.PrizePerDay = updatedRoom.PrizePerDay;
        }

    }
}