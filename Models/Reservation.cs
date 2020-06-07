using RoomReservation.Stores;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace RoomReservation.Models
{
    public class Reservation
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ReservationId { get; set; }

        [ForeignKey("User")]
        public long UserId { get; set; }

        public virtual User User { get; set; }

        [ForeignKey("Room")]
        public long RoomId { get; set; }
       
        public virtual Room Room { get; set; }
        // [Required(ErrorMessage = "Pole Wymagane")]
        public DateTime StartDate { get; set; }
        // [Required(ErrorMessage = "Pole Wymagane")]
        public DateTime EndDate { get; set; }
        public float WholeCost { get; set; }
        public List<int> Nums { get; set; }
        [NotMapped]
        public List<Reservation> Reservations;

        public bool ValidateDatesOfReservation()
        {
            DateTime now = DateTime.Now;
            TimeSpan tsNow = now.Subtract(this.StartDate);
            TimeSpan ts = this.EndDate.Subtract(this.StartDate);
            int days = ts.Days;
            int daysNow =  tsNow.Days;
            if (days >= 0 && daysNow <=0)
                return true;
            else
                return false;
        }
        public void UpdateReservation(Reservation updatedReservation)
        {
            //this.RoomId = updatedReservation.RoomId;
            this.StartDate = updatedReservation.StartDate;
            this.EndDate = updatedReservation.EndDate;
        }
      


    }
}