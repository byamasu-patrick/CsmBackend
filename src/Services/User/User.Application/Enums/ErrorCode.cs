using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace User.Application.Enums
{
    public enum BaseError
    {
        NoError = 200,
        UnknownError = 500,
        ParameterError = 501
    }

    public enum AuthError
    {
        GenericAuthError = 1000,
        EmailNotConfirmed = 1001,
        WrongEmailOrPassword = 1002,
        UserAlreadyExists = 1003,
        TokenNotFound = 1004
    }

}
