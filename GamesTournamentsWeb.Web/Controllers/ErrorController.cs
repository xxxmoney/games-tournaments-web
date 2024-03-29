﻿using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Runtime.InteropServices.JavaScript;
using GamesTournamentsWeb.Infrastructure.Dto;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace GamesTournamentsWeb.Web.Controllers;

[ApiExplorerSettings(IgnoreApi = true)]    
public class ErrorController : ControllerBase
{
    [Route("error")]
    public Error Error()
    {
        var context = HttpContext.Features.Get<IExceptionHandlerFeature>();
        var exception = context.Error;
        var code = exception is ValidationException ? HttpStatusCode.BadRequest : HttpStatusCode.InternalServerError;         
        Response.StatusCode = (int)code;
        return new Error(exception);
    }
}