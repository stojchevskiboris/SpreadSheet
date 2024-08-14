using AutoMapper;
using SpreadSheet.Models;

namespace SpreadSheet.Configurations
{
    public class AutoMapperConfig : Profile
    {
        public AutoMapperConfig()
        {
            CreateMap<Cell, Cell>();
        }
    }
}
