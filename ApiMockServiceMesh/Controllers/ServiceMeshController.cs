using ApiMockServiceMesh.Enum;
using ApiMockServiceMesh.Util;
using ApiMockServiceMesh.DTO;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Diagnostics;

namespace ApiMockServiceMesh.Controllers
{
    public class ServiceMeshController : BaseController
    {

        [HttpPost]
        [Route("v1/Company")]
        public async Task<IActionResult> Post([FromBody]List<EndPoint> command)
        {
            var result = new Dictionary<string, object>();


            Parallel.ForEach(command, (x) =>
            {
                try
                {
                    using (var client = new HttpClientUtil<Result>())
                    {
                        var objResult = new Result();

                        var sw = new Stopwatch();

                        sw.Start();

                        objResult = client.Post(x.Api, x).Result;

                        sw.Stop();

                        objResult.Time = sw.Elapsed;

                        result.Add(x.Api, objResult);
                    }
                }
                catch (Exception ex)
                {
                    result.Add(x.Api, ex.Message);
                }

            });


            return await Response(result);
        }


        [HttpPost]
        [Route("v1/Company/Address")]
        public async Task<IActionResult> Address([FromBody]EndPoint command)
        {
            object reponse = "R. João Pessoa, 93 - Centro, São Caetano do Sul - SP";

            if (command == null)
            {
                return await Response("Object Company/Address null !!!");
            }

            reponse = validateStatus(reponse, command.Status);

            if (command.Pass != null)
            {
                reponse = sendEndPoint(command.Pass);
            }

            return await Response(reponse);
        }

        [HttpPost]
        [Route("v1/Company/Name")]
        public async Task<IActionResult> Name([FromBody]EndPoint command)
        {
            object reponse = "Via Varejo";

            if (command == null)
                return await Response("Object Company/Name null !!!");

            reponse = validateStatus(reponse, command.Status);

            if (command.Pass != null)
            {
                reponse = sendEndPoint(command.Pass);
            }

            return await Response(reponse);
        }

        [HttpPost]
        [Route("v1/Company/ZipCode")]
        public async Task<IActionResult> ZipCode([FromBody]EndPoint command)
        {
            object reponse = "09520-010";

            if (command == null)
                return await Response("Object null !!!");

            reponse = validateStatus(reponse, command.Status);

            if (command.Pass != null)
            {
                reponse = sendEndPoint(command.Pass);
            }

            return await Response(reponse);
        }

        private Result sendEndPoint(EndPoint command)
        {
            var result = new Result();

            if (command != null)
            {
                var api = command.Api;

                using (var client = new HttpClientUtil<Result>())
                {
                    result = client.Post(api, command).Result;
                }

                if (command.Pass != null)
                {
                    sendEndPoint(command.Pass);
                }
            }

            return result;
        }

        private static object validateStatus(object reponse, Status status)
        {
            var result = string.Empty;

            switch (status)
            {
                case Status.SUCCESS:
                    result = reponse.ToString();
                    break;
                case Status.ERROR:
                    throw new Exception("ERROR INSERIDO !!!");
                case Status.TIMEOUT:
                    Thread.Sleep(900000);
                    result = reponse.ToString();
                    break;
            }

            return new Result
            {
                Success = true,
                Data = result
            };
        }
    }
}