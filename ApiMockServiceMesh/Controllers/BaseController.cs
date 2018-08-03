using ApiMockServiceMesh.DTO;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace ApiMockServiceMesh.Controllers
{
    public class BaseController : Controller
    {

        public async Task<IActionResult> Response(object result)
        {

            return Ok(new Result
            {
                Success = true,
                Data = result
            });
        }

    }
}
