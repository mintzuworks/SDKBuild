using System.Net;
using Mintzuworks.Domain;

public static class ResponseUtils
{
    public static bool IsResultOK(this CommonResult result)
    {
        return result != null && result.httpCode == (int)HttpStatusCode.OK;
    }

    public static bool IsResultFail(this CommonResult result)
    {
        return result != null && result.httpCode == (int)HttpStatusCode.BadRequest;
    }

    public static bool IsResultConflict(this CommonResult result)
    {
        return result != null && result.httpCode == (int)HttpStatusCode.Conflict;
    }

    public static bool IsResultCode(this CommonResult result, HttpStatusCode expectedCode)
    {
        return result != null && result.httpCode == (int)expectedCode;
    }
}
