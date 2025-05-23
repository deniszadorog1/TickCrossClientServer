using System.Text.RegularExpressions;

namespace TickCrossLib.Services
{
    public static class RegexService
    {
        public static bool RegistrationLoginValid(string login)
        {
            Regex loginReg = new Regex(@"^(?=.*[a-zA-Z])(?=.*\d)[a-zA-Z\d]{8,}$");
            return loginReg.Match(login).Success;
        }

        public static bool RegistrationPasswordValid(string password)
        {
            Regex passwordReg = new Regex(@"^[a-zA-Z0-9_]{3,20}$");
            return passwordReg.Match(password).Success;
        }

        public static bool LoginValidation(string login)
        {
            Regex rg = new Regex(@"^[a-zA-Z0-9_]{3,20}$");
            return rg.Match(login).Success;
        }

    }
}
