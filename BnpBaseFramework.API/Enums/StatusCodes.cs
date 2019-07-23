using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BnpBaseFramework.API.Enums
{
    public enum StatusCodes
    {
        Moved = 301,
        OK = 200,
        RequestCompleted=201,
        Accepted=202,
        NonAuthoritativeInformatio=203,
        NoContent=204,
        Redirect = 302,
        PageNotFound=404,
        InternalServerError=500,
        BadRequest=400,
        Unauthorized=401
    }
}
