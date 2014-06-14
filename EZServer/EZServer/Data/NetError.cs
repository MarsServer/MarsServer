using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EZServer
{
    public class NetError
    {
        public const string SQL_ERROR = "SQL is not Exit";


        public const string LOGIN_ERROR = "Login Error";
        public const string LOGIN_USER_ERROR = "Login Invaid User";
        public const string LOGIN_PW_ERROR = "Login Invaid PW";

        public const string REGISTER_ERROR = "Register Error";
        public const string REGISTER_USER_ERROR = "Register User Error";
        public const string REGISTER_NAME_ERROR = "Register Name Error";

        public const string INVAID_ERROR = "ID Error";
        public const string LIMIT_ERROR = "Limit Error";


        public const string UNKNOW_ERROR = "Unknown Error";
    }
}
