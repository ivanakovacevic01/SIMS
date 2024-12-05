using InitialProject.Model;
using InitialProject.Serializer;
using System.Collections.Generic;
using System.Linq;

namespace InitialProject.Repository
{
    public class UserRepository
    {
        private const string FilePath = "../../../Resources/Data/users.csv";

        private readonly Serializer<User> _serializer;

        private List<User> _users;

        public UserRepository()
        {
            _serializer = new Serializer<User>();
            _users = _serializer.FromCSV(FilePath);
        }

        public User GetByUsername(string username)
        {
            _users = _serializer.FromCSV(FilePath);
            return _users.FirstOrDefault(u => u.Username == username);
        }

        public List<Guest1> GetAllGuests1()
        {
            Guest1Repository guest1Repository = new Guest1Repository();
            List<Guest1> result = new List<Guest1>();
            foreach(User user in _users)
            {
                if (user.Role == Role.GUEST1)
                    result.Add(guest1Repository.GetByUsername(user.Username));
            }
            return result;
        }
    }
}
