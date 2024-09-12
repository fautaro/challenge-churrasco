using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class User
    {
        public int Id { get; set; }
        public DateTime Created { get; set; }
        public string Email { get; set; }
        public string First_Name { get; set; }
        public string Last_Name { get; set; }
        public string Password { get; set; }
        public DateTime Updated { get; set; }
        public string Username { get; set; }
        public string Role { get; set; }
        public bool Active { get; set; }



    }
}
