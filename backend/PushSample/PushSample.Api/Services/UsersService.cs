using System;
using System.Collections.Generic;
using System.Linq;
using PushSample.Api.Models;

namespace PushSample.Api.Services
{
    public class UsersService
    {
        private static readonly List<User> _users = new List<User>();

        public List<User> GetAllUsers()
        {
            return _users;
        }

        public void AddUser(string connectionId, string userName)
        {
            if (_users.All(u => u.ConnectionId != connectionId))
            {
                _users.Add(new User
                {
                    ConnectionId = connectionId,
                    Name = String.IsNullOrWhiteSpace(userName) ? connectionId : userName
                });
            }
        }

        public void RemoveUser(string connectionId)
        {
            var user = _users.FirstOrDefault(u => u.ConnectionId == connectionId);
            if (user != null)
            {
                _users.Remove(user);
            }
        }
    }
}