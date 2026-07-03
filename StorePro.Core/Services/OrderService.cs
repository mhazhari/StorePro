using AutoMapper;
using StorePro.Data.Repositories;
using StorePro.Models.Dtos;
using StorePro.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StorePro.Core.Services
{
    public class OrderService : IOrderService
    {
        private readonly IRepository<OrderMain> _orderRepository;
        private readonly IRepository<OrderProducts> _orderProductRepository;
        private readonly IRepository<TblItm> _productRepository;
        private readonly IMapper _mapper;

        public OrderService(
            IRepository<OrderMain> orderRepository,
            IRepository<OrderProducts> orderProductRepository,
            IRepository<TblItm> productRepository,
            IMapper mapper)
        {
            _orderRepository = orderRepository;
            _orderProductRepository = orderProductRepository;
            _productRepository = productRepository;
            _mapper = mapper;
        }

        public async Task<OrderDto> GetOrderByIdAsync(Guid orderId)
        {
            var order = await _orderRepository.GetByIdAsync(orderId);
            if (order == null) return null;

            return _mapper.Map<OrderDto>(order);
        }

        public async Task<IEnumerable<OrderDto>> GetAllOrdersAsync()
        {
            var orders = await _orderRepository.FindAsync(o => o.deleted_DateTime == null);
            return _mapper.Map<IEnumerable<OrderDto>>(orders);
        }

        public async Task<IEnumerable<OrderDto>> GetOrdersByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            var orders = await _orderRepository.FindAsync(o =>
                o.OrderDate >= startDate &&
                o.OrderDate <= endDate &&
                o.deleted_DateTime == null
            );

            return _mapper.Map<IEnumerable<OrderDto>>(orders);
        }

        public async Task<OrderDto> CreateOrderAsync(CreateOrderDto dto)
        {
            var order = new OrderMain
            {
                OMainGuID = Guid.NewGuid(),
                OrderType = dto.OrderType,
                OrderDate = DateTime.Now,
                StoreGUID = dto.StoreGUID,
                Account_Guid = dto.Account_Guid,
                SOLD_TO_Info = dto.SOLD_TO_Info,
                PaymentMethod = dto.PaymentMethod,
                BillDiscount = dto.BillDiscount ?? 0,
                BillSTax = dto.BillSTax ?? 0,
                Remark = dto.Remark,
                UserID = dto.UserID,
                Ins_Date = DateTime.Now
            };

            decimal totalAmount = 0;

            foreach (var detail in dto.OrderDetails)
            {
                var product = await _productRepository.GetByIdAsync(detail.ProductID);
                if (product == null) continue;

                var orderProduct = new OrderProducts
                {
                    OMainGuID = order.OMainGuID,
                    ProdOrderGuid = Guid.NewGuid(),
                    StoreGuid = dto.StoreGUID,
                    ProductID = detail.ProductID,
                    Unit_GUID = detail.Unit_GUID,
                    ITm_Name = product.ITm_Name,
                    Qty = detail.Qty,
                    Price = detail.Price,
                    SubTotal = detail.Qty * detail.Price,
                    Discount = detail.Discount ?? 0,
                    STax = detail.STax ?? 0,
                    ExpDate = DateTime.Now.AddYears(1),
                    Ins_Date = DateTime.Now
                };

                orderProduct.NetAmount = orderProduct.SubTotal - orderProduct.Discount + orderProduct.STax;
                order.OrderProducts.Add(orderProduct);
                totalAmount += orderProduct.NetAmount;
            }

            order.Amount = totalAmount;
            order.Remainder = totalAmount - (order.BillDiscount ?? 0);

            await _orderRepository.AddAsync(order);
            await _orderRepository.SaveChangesAsync();

            return _mapper.Map<OrderDto>(order);
        }

        public async Task<OrderDto> UpdateOrderAsync(Guid orderId, UpdateOrderDto dto)
        {
            var order = await _orderRepository.GetByIdAsync(orderId);
            if (order == null) return null;

            var oldDetails = await _orderProductRepository.FindAsync(op => op.OMainGuID == orderId);
            foreach (var detail in oldDetails)
            {
                _orderProductRepository.Remove(detail);
            }

            decimal totalAmount = 0;
            foreach (var detail in dto.OrderDetails)
            {
                var product = await _productRepository.GetByIdAsync(detail.ProductID);
                if (product == null) continue;

                var orderProduct = new OrderProducts
                {
                    OMainGuID = orderId,
                    ProdOrderGuid = Guid.NewGuid(),
                    StoreGuid = order.StoreGUID,
                    ProductID = detail.ProductID,
                    Unit_GUID = detail.Unit_GUID,
                    ITm_Name = product.ITm_Name,
                    Qty = detail.Qty,
                    Price = detail.Price,
                    SubTotal = detail.Qty * detail.Price,
                    Discount = detail.Discount ?? 0,
                    STax = detail.STax ?? 0,
                    ExpDate = DateTime.Now.AddYears(1),
                    Ins_Date = DateTime.Now
                };

                orderProduct.NetAmount = orderProduct.SubTotal - orderProduct.Discount + orderProduct.STax;
                await _orderProductRepository.AddAsync(orderProduct);
                totalAmount += orderProduct.NetAmount;
            }

            order.SOLD_TO_Info = dto.SOLD_TO_Info;
            order.PaymentMethod = dto.PaymentMethod;
            order.BillDiscount = dto.BillDiscount ?? 0;
            order.Remark = dto.Remark;
            order.Amount = totalAmount;
            order.Remainder = totalAmount - (order.BillDiscount ?? 0);
            order.Upd_Date = DateTime.Now;

            _orderRepository.Update(order);
            await _orderRepository.SaveChangesAsync();

            return _mapper.Map<OrderDto>(order);
        }

        public async Task<bool> CancelOrderAsync(Guid orderId)
        {
            var order = await _orderRepository.GetByIdAsync(orderId);
            if (order == null) return false;

            order.deleted_DateTime = DateTime.Now;
            _orderRepository.Update(order);

            return await _orderRepository.SaveChangesAsync();
        }

        public async Task<OrderPrintDto> GetOrderForPrintAsync(Guid orderId)
        {
            var order = await _orderRepository.GetByIdAsync(orderId);
            if (order == null) return null;

            return _mapper.Map<OrderPrintDto>(order);
        }
    }
}