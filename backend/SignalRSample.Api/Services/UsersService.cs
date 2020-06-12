using System;
using System.Collections.Generic;
using System.Linq;
using SignalRSample.Api.Models;

namespace SignalRSample.Api.Services
{
    // REVIEW: Es wäre hier auch denkbar, dass Du (wie es der IdSrv macht) bspw. eine InMemoryDatabase nutzt.
    // In einem "echten" Spielserver wird die Userliste höchstwahrscheinlich in einer Datenbank stecken.
    // Plus, Du könntest ein IUsersService definieren und dann eben hier den InMemoryUsersService implementieren.
    // Generell würde ich sonst die Methoden hier in der Klasse als Task auslegen und mit async/await arbeiten,
    // da das in einem echten Server eben bedingt durch den Zugriff auf eine weitere Ressource sinnvoller erscheint.
    public class UsersService
    {
        // REVIEW: Hier wäre es auch möglich, wenn Du bei dieser Art der Implementierung bleibst,
        // auch auf ein Dictionary zurückzugreifen, da Du im Falle eines echten Servers mehr ID-basierte Zugriffe hast
        // als listenbasierte Zugriffe. Das würde dann unten auch die .All und .FirstOrDefault-Zugriffe schneller machen.
        private static readonly List<User> _users = new List<User>();

        public List<User> GetAllUsers()
        {
            return _users;
        }

        public void AddUser(string connectionId, string userName)
        {
            // REVIEW: Performancetechnisch würde ich hier auf ein _users.Any(user => user.ConnectionId == connectionId) wechseln.
            // Bei .All müssen alle _users durchlaufen werden, bei .Any wird beim ersten Auffinden einer passenden connectionId sofort abgebrochen.
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
