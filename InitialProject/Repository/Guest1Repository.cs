using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InitialProject.Model;
using InitialProject.Serializer;

namespace InitialProject.Repository
{
    public class Guest1Repository
    {
        private const string FilePath = "../../../Resources/Data/guests1.csv";

        private readonly Serializer<Guest1> _serializer;

        private List<Guest1> _guests;

        public Guest1Repository()
        {
            _serializer = new Serializer<Guest1>();
            _guests = _serializer.FromCSV(FilePath);
        }

        public List<Guest1> GetAll()
        {
            return _guests;
        }
        public Guest1 GetByUsername(string userName)
        {
            return _guests.Find(n => n.Username.Equals(userName));
        }
    }
}
