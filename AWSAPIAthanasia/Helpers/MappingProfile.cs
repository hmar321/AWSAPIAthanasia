using ApiAthanasia.Models.Tables;
using ApiAthanasia.Models.Views;
using AutoMapper;

namespace ApiAthanasia.Helpers
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<ProductoSimpleView, PedidoProducto>();
            CreateMap<PedidoProducto, ProductoSimpleView>();
        }
    }
}
