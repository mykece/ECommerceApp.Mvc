using ETicaret.Applicationn.DTOs.CategoryDTOs;
using ETicaret.Applicationn.DTOs.OrderDetailDTOs;
using ETicaret.Applicationn.DTOs.OrderDTO;
using ETicaret.Domain.Entities;
using ETicaret.Domain.Utilities.Concretes;
using ETicaret.Domain.Utilities.Interfaces;
using ETicaret.Infrastructure.Repositories.OrderDetailRepositories;
using ETicaret.Infrastructure.Repositories.OrderRepositories;
using Mapster;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETicaret.Applicationn.Services.OrderServices
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IOrderDetailRepository _orderDetailRepository;

        public OrderService(IOrderRepository orderRepository, IOrderDetailRepository orderDetailRepository)
        {
            _orderRepository = orderRepository;
            _orderDetailRepository = orderDetailRepository;
        }


        public async Task<IDataResult<OrderDTO>> CreateAsync(OrderCreateDTO orderCreateDTO)
        {

            DataResult<OrderDTO> result = new ErrorDataResult<OrderDTO>();

            var strategy = await _orderRepository.CreateExecutionStrategy();
            await strategy.ExecuteAsync(async () =>
            {
                var transactionScope = await _orderRepository.BeginTransactionAsync().ConfigureAwait(false);
                try
                {
                    orderCreateDTO.ShipVia = " ";
                    orderCreateDTO.ShipCity = " ";
                    orderCreateDTO.ShipAdress = " ";
                    orderCreateDTO.ShipCountry = " ";
                    orderCreateDTO.ShipPostalCode =0;


                    var order = await _orderRepository.AddAsync(orderCreateDTO.Adapt<Order>());

                    if (order == null)
                    {
                        result = new ErrorDataResult<OrderDTO>("Order eklenirken bir hata oluştu.");
                        transactionScope.Rollback();
                        return;
                    }
                    var orderDetails = orderCreateDTO.OrderDetailCreateDTOs;
                    if (orderDetails == null)
                    {
                        result = new ErrorDataResult<OrderDTO>("Order eklenirken bir hata oluştu.");
                        transactionScope.Rollback();
                        return;
                    }
                    foreach ( var item in orderDetails )
                    {
                        item.OrderId = order.Id;
                        
                        var orderDetail = await _orderDetailRepository.AddAsync(item.Adapt<OrderDetail>());
                        if(orderDetail == null)
                        {
                            result = new ErrorDataResult<OrderDTO>("Order eklenirken bir hata oluştu.");
                            transactionScope.Rollback();
                            return;
                        }
                        
                    }

                    await _orderDetailRepository.SaveChangesAsync();
                    await _orderDetailRepository.SaveChangesAsync();
                    result = new SuccessDataResult<OrderDTO>("Order ekleme başarılı");
                    transactionScope.Commit();


                }
                catch (Exception ex)
                {
                    result = new ErrorDataResult<OrderDTO>("Order could not be added!" + ex.Message);
                    transactionScope.Rollback();

                }
                finally
                {
                    transactionScope.Dispose();
                }
            });
            return result;
        }

        public async Task<IResult> DeleteAsync(Guid id)
        {
            DataResult<OrderDTO> result = new ErrorDataResult<OrderDTO>();

            var strategy = await _orderRepository.CreateExecutionStrategy();
            await strategy.ExecuteAsync(async () =>
            {
                var transactionScope = await _orderRepository.BeginTransactionAsync().ConfigureAwait(false);
                try
                {
                    var order = await _orderRepository.GetByIdAsync(id);
                    if(order==null)
                    {
                        result = new ErrorDataResult<OrderDTO>("Order eklenirken bir hata oluştu.");
                        transactionScope.Rollback();
                        return;
                    }
                    await _orderRepository.DeleteAsync(order);
                    var orderDetails = await _orderDetailRepository.GetAllAsync(x => x.OrderId == id);
                    if (orderDetails == null || orderDetails.ToList().Count<1)
                    {
                        result = new ErrorDataResult<OrderDTO>("Order eklenirken bir hata oluştu.");
                        transactionScope.Rollback();
                        return;
                    }
                    foreach (var item in orderDetails.ToList())
                    {
                         await _orderDetailRepository.DeleteAsync(item);
                        
                    }
                    await _orderDetailRepository.SaveChangesAsync();
                    await _orderDetailRepository.SaveChangesAsync();
                    result = new SuccessDataResult<OrderDTO>("Order ekleme başarılı");
                    transactionScope.Commit();

                }
                catch (Exception ex)
                {
                    result = new ErrorDataResult<OrderDTO>("Order could not be added!" + ex.Message);
                    transactionScope.Rollback();

                }
                finally
                {
                    transactionScope.Dispose();
                }
            });
            return result;
        }

        public async Task<IDataResult<List<OrderListDTO>>> GetAllAsync()
        {
            var orders = await _orderRepository.GetAllAsync();
            var orderListDTOs = orders.ToList().Adapt<List<OrderListDTO>>();
            if (orderListDTOs == null)
            {
                return new ErrorDataResult<List<OrderListDTO>>(orderListDTOs, "Order Listeleme başarısız.");
            }
            else if(orderListDTOs.Count == 0)
            {
                return new SuccessDataResult<List<OrderListDTO>>(orderListDTOs, "Listelenecek order bulunamadı.");
            }

            //OrderDetail'in service'ini yazmadığımız için order'a bağlı tüm orderdetails'ler ilgili order'ın içinde gelecek.
            foreach(var item in orderListDTOs)
            {
                var orderDetails = (await _orderDetailRepository.GetAllAsync(x => x.OrderId == item.Id)).ToList();
                if (orderDetails != null)
                {
                    item.OrderDetailListDTOs = orderDetails.Adapt<List<OrderDetailListDTO>>();
                }
                continue;
            }
            return new SuccessDataResult<List<OrderListDTO>>(orderListDTOs, "Order Listeleme Başarılı");

        }

        public async Task<IDataResult<OrderDTO>> GetByIdAsync(Guid id)
        {
            var order = await _orderRepository.GetByIdAsync(id);
            if(order == null)
            {
                return new ErrorDataResult<OrderDTO>(order.Adapt<OrderDTO>(),"Order bulunamadı!");
            }
            var orderDetails = (await _orderDetailRepository.GetAllAsync(x=>x.OrderId == id)).ToList();
            var orderDTO = order.Adapt<OrderDTO>();
            orderDTO.OrderDetailDTOs = orderDetails.Adapt<List<OrderDetailDTO>>();
            return new SuccessDataResult<OrderDTO>(orderDTO, "Order başarıyla bulundu");
        }

        public async Task<IDataResult<OrderDTO>> UpdateAsync(OrderUpdateDTO orderUpdateDTO)
        {
            DataResult<OrderDTO> result = new ErrorDataResult<OrderDTO>();

            var strategy = await _orderRepository.CreateExecutionStrategy();
            await strategy.ExecuteAsync(async () =>
            {
                var transactionScope = await _orderRepository.BeginTransactionAsync().ConfigureAwait(false);
                try
                {
                    var order = await _orderRepository.UpdateAsync(orderUpdateDTO.Adapt<Order>());
                    if (order == null)
                    {
                        result = new ErrorDataResult<OrderDTO>("Order güncellenirken bir hata oluştu.");
                        transactionScope.Rollback();
                        return;
                    }
                    var orderDetailUpdateDTOs = orderUpdateDTO.OrderDetailUpdateDTOs;
                    if (orderDetailUpdateDTOs == null)
                    {
                        result = new ErrorDataResult<OrderDTO>("Order güncellenirken bir hata oluştu.");
                        transactionScope.Rollback();
                        return;
                    }
                    var deletedOrders = (await _orderDetailRepository.GetAllAsync(x => x.OrderId == orderUpdateDTO.Id));
                    foreach (var item in deletedOrders)
                    {
                        await _orderDetailRepository.DeleteAsync(item);
                    }
                    foreach (var item in orderDetailUpdateDTOs)
                    {
                       
                        var orderDetail = await _orderDetailRepository.AddAsync(item.Adapt<OrderDetail>());
                        if (orderDetail == null)
                        {
                            result = new ErrorDataResult<OrderDTO>("Order güncellenirken bir hata oluştu.");
                            transactionScope.Rollback();
                            return;
                        }

                    }
                    await _orderDetailRepository.SaveChangesAsync();
                    await _orderDetailRepository.SaveChangesAsync();
                    result = new SuccessDataResult<OrderDTO>("Order güncelleme başarılı");
                    transactionScope.Commit();


                }
                catch (Exception ex)
                {
                    result = new ErrorDataResult<OrderDTO>("Order could not be added!" + ex.Message);
                    transactionScope.Rollback();

                }
                finally
                {
                    transactionScope.Dispose();
                }
            });
            return result;
        }
    }
}
