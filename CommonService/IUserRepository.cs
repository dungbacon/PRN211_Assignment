using QuanLyKho_Project.Models;
using System.Collections.Generic;
using System.Net;

namespace QuanLyKho_Project.CommonService
{
    public interface IUserRepository
    {
        User AuthenticateUser(NetworkCredential credential);
        void Add(User user);
        void Edit(User user);
        void Remove(int id);
        User GetById(int id);
        User GetByName(string name);
        IEnumerable<User> GetAll();
    }
}
