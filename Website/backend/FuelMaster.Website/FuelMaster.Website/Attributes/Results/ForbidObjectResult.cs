

using Microsoft.AspNetCore.Mvc;

namespace FuelMaster.Website.Attributes.Results;

public class ForbidObjectResult : ObjectResult
{
    public ForbidObjectResult(object value) : base(value)
    {
        StatusCode = 403;
    }
}