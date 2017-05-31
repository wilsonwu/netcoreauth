using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace netcoreauth.model
{
    public class Token
    {
        [Key]
        public int Id { get; set;}
        public int User_Id { get; set; }
        public string Token_Type { get; set; }
        public string JWT_Token { get; set; }
    }
}
