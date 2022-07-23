using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Model
{
    public class CreateUserArgs
    {
        public string Email { get; set; } = null!;

        public string Password { get; set; } = null!;

        public string Name { get; set; } = null!;

        public byte Status { get; set; }

        public int RoleId { get; set; }
    }
    public class UpdateUserArgs
    {
        public int Id { get; set; }
        public string Email { get; set; } = null!;

        public string Password { get; set; } = null!;

        public string Name { get; set; } = null!;

        public byte Status { get; set; }

        public int RoleId { get; set; }
    }
}
