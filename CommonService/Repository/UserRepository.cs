using QuanLyKho_Project.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace QuanLyKho_Project.CommonService.Repository
{
    public class UserRepository : IUserRepository
    {
        public void Add(User user)
        {
            throw new NotImplementedException();
        }

        public User AuthenticateUser(NetworkCredential credential)
        {
            var user = DataProvider.Ins.DB.Users.Where(o => o.UserName == credential.UserName && o.Password == credential.Password).FirstOrDefault();
            return user;
        }

        public void Edit(User user)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<User> GetAll()
        {
            throw new NotImplementedException();
        }

        public User GetById(int id)
        {
            throw new NotImplementedException();
        }

        public User GetByName(string name)
        {
            var user = DataProvider.Ins.DB.Users.Where(o => o.UserName == name).FirstOrDefault();
            return user;
        }

        public void Remove(int id)
        {
            throw new NotImplementedException();
        }
    }
}
