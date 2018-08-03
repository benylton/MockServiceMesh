using System;

namespace ApiMockServiceMesh.DTO
{
    public class Result
    {
        public Result()
        {
            MachineName = Environment.MachineName;
        }


        public string MachineName { get; private set; }
        public bool Success { get; set; }
        public TimeSpan Time { get; set; }
        public object Data { get; set; }
    }
}
