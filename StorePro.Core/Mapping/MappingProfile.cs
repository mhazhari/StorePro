using AutoMapper;
using StorePro.Models.Dtos;
using StorePro.Models.Entities;
using System.Linq;

namespace StorePro.Core.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // ========== Product Mappings ==========
            CreateMap<TblItm, ProductDto>()
                .ForMember(dest => dest.Units, opt => opt.MapFrom(src => src.TblItmOnUnits))
                .ReverseMap();

            CreateMap<CreateProductDto, TblItm>();
            CreateMap<UpdateProductDto, TblItm>();

            CreateMap<TblItm, SearchProductDto>()
                .ForMember(dest => dest.Units, opt => opt.MapFrom(src => src.TblItmOnUnits));

            // ========== Unit Mappings ==========
            CreateMap<TblItmOnUnit, ProductUnitDto>().ReverseMap();

            // ========== Order Mappings ==========
            CreateMap<OrderMain, OrderDto>()
                .ForMember(dest => dest.OrderDetails, opt => opt.MapFrom(src => src.OrderProducts));

            CreateMap<CreateOrderDto, OrderMain>();
            CreateMap<UpdateOrderDto, OrderMain>();

            CreateMap<OrderMain, OrderPrintDto>()
                .ForMember(dest => dest.TotalAmount, opt => opt.MapFrom(src => src.Amount))
                .ForMember(dest => dest.DiscountAmount, opt => opt.MapFrom(src => src.BillDiscount))
                .ForMember(dest => dest.TaxAmount, opt => opt.MapFrom(src => src.BillSTax))
                .ForMember(dest => dest.NetAmount, opt => opt.MapFrom(src => src.Remainder))
                .ForMember(dest => dest.Details, opt => opt.MapFrom(src => src.OrderProducts));

            // ========== Order Product Mappings ==========
            CreateMap<OrderProducts, OrderProductDto>();
            CreateMap<CreateOrderProductDto, OrderProducts>();

            // ========== Voucher Mappings ==========
            CreateMap<VoucherMain, VoucherDto>();
            CreateMap<CreateVoucherDto, VoucherMain>();

            // ========== Account Mappings ==========
            CreateMap<ChartAccMain, object>();
        }
    }
}