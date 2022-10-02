using Domain.Entities.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Label : EntityBase
    {
        public string Title { get; set; } = default!;
        public string? Description { get; set; }

        public int UserId { get; set; }
        public User User { get; set; } = default!;

    }
}
