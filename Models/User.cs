using RoomReservation.Stores;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace RoomReservation.Models
{
    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long UserId { get; set; }
        [Required(ErrorMessage = "Pole Wymagane")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Pole Wymagane")]
        public string Surname { get; set; }
        [Required(ErrorMessage = "Pole Wymagane")]
        public string NickName { get; set; }
        [Required(ErrorMessage = "Pole Wymagane")]
        [DataType(DataType.Password)]
        [StringLength(18,ErrorMessage = "Hasło musi mieć co najmniej 6 znaków", MinimumLength = 6)]
        public string Password { get; set; }
        [Required(ErrorMessage = "Pole Wymagane")]
        [DataType(DataType.Password)]
        [Compare("Password")]
        public string RepeatedPasswd { get; set; }
        public Role UserRole { get; set; }

 //       [NotMapped]
      ///  public string Message { get; set; } = "  ";



    }
}