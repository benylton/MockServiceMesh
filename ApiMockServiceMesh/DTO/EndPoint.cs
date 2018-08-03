using ApiMockServiceMesh.Enum;

namespace ApiMockServiceMesh.DTO
{

    public class EndPoint
    {
        public string Api { get; set; }
        public Status Status { get; set; }
        public EndPoint Pass { get; set; }
    }
}
