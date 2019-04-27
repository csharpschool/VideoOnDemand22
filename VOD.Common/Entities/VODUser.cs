using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Claims;

namespace VOD.Common.Entities
{
    public class VODUser : IdentityUser
    {
        public string Token { get; set; }
        public DateTime TokenExpires { get; set; }

        [NotMapped]
        public IList<Claim> Claims { get; set; } = new List<Claim>();
    }
}
