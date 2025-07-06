using WebApi_API.Models.DTO;

namespace WebApi_API.Data
{
    public static class VilaStore
    {
        public static List<VilaDto> vilaList = new List<VilaDto>()
        {
            new VilaDto() { Id = 1, Name = "Villa 1",Occupancy = 11,SqFt = 22},
            new VilaDto() { Id = 2, Name = "Villa 2",Occupancy = 33,SqFt = 44},
        };
    }
}
