using System;
using System.Collections.Generic;
using GrapeCity.Forguncy.ServerApi;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace BulkInsertDataToDataBaseExtension.Server
{
    public class BulkInsertDataToDataBaseExtensionMiddlewareInjector : MiddlewareInjector
    {
        public override List<ServiceItem> ConfigureServices(List<ServiceItem> serviceItems, IServiceCollection services)
        {
            serviceItems.Insert(0, new ServiceItem()
            {
                Id = "fe46136a-2a0b-44a5-b749-9b1f0a4f9491",
                ConfigureServiceAction = () =>
                {
                    // 这里可以注册中间件需要的服务，相当于 Asp.net 中的 public void ConfigureServices(IServiceCollection services) 方法
                    //services.AddXXXService();
                },
                Description = "我的自定义中间件服务"
            });
            return base.ConfigureServices(serviceItems, services);
        }
        public override List<MiddlewareItem> Configure(List<MiddlewareItem> middlewareItems, IApplicationBuilder app)
        {
            middlewareItems.Insert(0, new MiddlewareItem()
            {
                Id = "fe46136a-2a0b-44a5-b749-9b1f0a4f9491",
                ConfigureMiddleWareAction = () =>
                {
                    app.UseMiddleware<BulkInsertDataToDataBaseExtensionMiddleware>();
                },
                Description = "我的自定义中间件"
            });
            return base.Configure(middlewareItems, app);
        }
    }
}
