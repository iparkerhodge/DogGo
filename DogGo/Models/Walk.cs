using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DogGo.Models
{
    public class Walk
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public int Duration { get; set; }

        public int WalkerId { get; set; }
        public Walker Walker { get; set; }
        public int DogId { get; set; }
        public Dog Dog { get; set; }

        public int OwnerId { get; set; }
        public Owner Client { get; set; }
    }
}
