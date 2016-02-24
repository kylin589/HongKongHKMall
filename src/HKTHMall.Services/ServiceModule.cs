using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Autofac;
using Autofac.Features.ResolveAnything;
using HKTHMall.Services.AC;
using HKTHMall.Services.Orders;
using HKTHMall.Services.Products;
using HKTHMall.Services.ShoppingCart;
using HKTHMall.Services.SKU;
using HKTHMall.Services.Sys;
using HKTHMall.Services.Users;
using HKTHMall.Services.WebLogin;
using HKTHMall.Services.YHUser;
using IZJ_UserBalanceService = HKTHMall.Services.Users.IZJ_UserBalanceService;

namespace HKTHMall.Services
{
    public class ServiceModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            // Configure Autofac to create a new instance of any type that implements ICommand when such type is requested

            builder.RegisterSource(new AnyConcreteTypeNotAlreadyRegisteredSource(t => t.IsAssignableTo<ISKU_AttributesService>()));

            builder.RegisterSource(new AnyConcreteTypeNotAlreadyRegisteredSource(t => t.IsAssignableTo<IOrderTrackingLogService>()));
            builder.RegisterSource(new AnyConcreteTypeNotAlreadyRegisteredSource(t => t.IsAssignableTo<IPaymentOrderService>()));

            builder.RegisterSource(new AnyConcreteTypeNotAlreadyRegisteredSource(t => t.IsAssignableTo<IShoppingCartService>()));
            builder.RegisterSource(new AnyConcreteTypeNotAlreadyRegisteredSource(t => t.IsAssignableTo<ISKU_ProductService>()));
            builder.RegisterSource(new AnyConcreteTypeNotAlreadyRegisteredSource(t => t.IsAssignableTo<ITHAreaService>()));
            builder.RegisterSource(new AnyConcreteTypeNotAlreadyRegisteredSource(t => t.IsAssignableTo<IUserAddressService>()));
            builder.RegisterSource(new AnyConcreteTypeNotAlreadyRegisteredSource(t => t.IsAssignableTo<IProductService>()));
            builder.RegisterSource(new AnyConcreteTypeNotAlreadyRegisteredSource(t => t.IsAssignableTo<IOrderService>()));
            builder.RegisterSource(new AnyConcreteTypeNotAlreadyRegisteredSource(t => t.IsAssignableTo<IYH_UserService>()));
            builder.RegisterSource(new AnyConcreteTypeNotAlreadyRegisteredSource(t => t.IsAssignableTo<IZJ_UserBalanceService>()));
            builder.RegisterSource(new AnyConcreteTypeNotAlreadyRegisteredSource(t => t.IsAssignableTo<IYH_PasswordErrorService>()));
            builder.RegisterSource(new AnyConcreteTypeNotAlreadyRegisteredSource(t => t.IsAssignableTo<IParameterSetService>()));
            builder.RegisterSource(new AnyConcreteTypeNotAlreadyRegisteredSource(t => t.IsAssignableTo<IPurchaseOrderSerivce>()));
        }

    }
}
