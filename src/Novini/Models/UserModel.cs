using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Novini.Models
{
    public class UserModel
    {
        public int Id { get; set; }
        public string Login { get; set; }
        public string PasswordHash { get; set; }
        public byte[] Salt { get; set; }
    }
}
