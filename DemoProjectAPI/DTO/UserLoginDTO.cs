using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DemoProjectAPI.DTO
{
   public class UserLoginDTO
    {
        public UserLoginDTO() { }
        public string UserID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MobileNumber { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string StatementType { get; set; }
    }
    public class LoginStatusDTO
    {
        public string MobileStatus { get; set; }
        public string Response { get; set; }
        public string UserID { get; set; }
    }
}