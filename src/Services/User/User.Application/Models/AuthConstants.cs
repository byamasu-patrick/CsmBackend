using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace User.Application.Models
{
    public static class AuthConstants
    {
        #region Roles
        public const string ADMIN = "Admin";
        public const string SHOP_OWNER = "Shop-Owner";
        public const string PREMIUM_USER = "Premium-User";
        public const string FREE_USER = "Free-User";
       
        #endregion
        #region PasswordCheck
        public const string ALPHANUMERICAL = "^(?=.*[0-9])(?=.*[a-zA-Z])([a-zA-Z0-9!@#$&%*]+)$";
        #endregion
    }
}
