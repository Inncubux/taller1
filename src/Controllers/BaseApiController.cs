using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;

namespace ECommerce.src.Controllers
{
    /// <summary>
    /// Base class for all API controllers. It inherits from ControllerBase to provide common functionality for API controllers.
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    public class BaseApiController : ControllerBase
    {

    }
}