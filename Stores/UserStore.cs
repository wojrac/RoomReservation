using RoomReservation.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Diagnostics;

namespace RoomReservation.Stores
{
    public class UserStore
    {
        private readonly RegistrationContext Db = new RegistrationContext();
        public User AddUser(User user)
        {
            Db.Users.Add(user);
            Db.SaveChanges();
            return user;
        }
        public User GetUserById(long id)
        {
            var user = Db.Users.Where(u => u.UserId == id).FirstOrDefault();
            return user;
        }
        public User GetUserByNick(string nick)
        {
            var user = Db.Users.Where(u => u.NickName == nick).FirstOrDefault();
            return user;
        }
        public bool IsNickExists(string nick)
        {
            bool isExists = false;
            var users = Db.Users;
            foreach(var user in users)
            {
                if (user.NickName == nick)
                    isExists = true;
            }
            return isExists;
          
        }
        public bool IsPassesOk(string nick,string passwd)
        {
            bool isOk = false;
            var users = Db.Users;
            foreach(var user in users)
            {
                if(user.NickName == nick)
                {
                    if (user.Password == passwd)
                        isOk = true;
                }
            }
            return isOk;
        }
    }
}