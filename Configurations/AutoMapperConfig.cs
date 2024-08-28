using AutoMapper;
using SpreadSheet.Models;

namespace SpreadSheet.Configurations
{
    public class AutoMapperConfig : Profile
    {
        public AutoMapperConfig()
        {
            CreateMap<Cell, Cell>();
            CreateMap<CellViewModel, CellViewModel>();
            CreateMap<Cell, CellViewModel>();
            CreateMap<CellViewModel, Cell>();

        }
    }
}
