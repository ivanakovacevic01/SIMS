using InitialProject.Serializer;
using System;

namespace InitialProject.Model
{
    public enum Role { OWNER, GUEST1, GUEST2 ,GUIDE}
    public class User : ISerializable
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public Role Role { get; set; }

        public User() { }

        public User(string username, string password, Role role)
        {
            Username = username;
            Password = password;
            Role = role;

        }

        public string[] ToCSV()
        {
            string[] csvValues = { Id.ToString(), Username, Password, Role.ToString() };
            return csvValues;
        }

        public void FromCSV(string[] values)
        {
            Id = Convert.ToInt32(values[0]);
            Username = values[1];
            Password = values[2];

            if (values[3].Equals("OWNER"))
                Role = Role.OWNER;
            else if (values[3].Equals("GUIDE"))
                Role = Role.GUIDE;
            else if (values[3].Equals("GUEST1"))
                Role = Role.GUEST1;
            else
                Role = Role.GUEST2;
        }
    }
}
