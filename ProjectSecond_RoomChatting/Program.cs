using Autofac;
using Autofac.Extensions.DependencyInjection;
using DataService.HashService;
using DataService.Repositories.UnitOfWork;
using DataServices.Entities;
using Services.AccountServices;
using Services.AuthServices;
using Services.MessageServices;
using Services.RoomChattingServices;
using Services.UserServices;
using Microsoft.AspNetCore.SignalR;
using ProjectSecond_RoomChatting.wwwroot.SignalIR_Hubs;
using Autofac.Core;
using Services.RedisServices;

namespace ProjectSecond_RoomChatting
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews().AddRazorRuntimeCompilation();
            builder.Services.AddSession();
            builder.Services.AddSignalR();
            builder.Services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = "localhost: 6379";
            });

            builder.Services.AddSession();
            builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());
            builder.Host.ConfigureContainer<ContainerBuilder>(builder =>
            {
                builder.RegisterType<RoomChattingContext>().AsSelf();
                builder.RegisterType<UnitOfWork>().As<IUnitOfWork>();
                builder.RegisterType<AccountService>().As<IAccountService>();
                builder.RegisterType<UserService>().As<IUserService>();
                builder.RegisterType<HashService>().As<IHashService>();
                builder.RegisterType<AuthService>().As<IAuthService>();
                builder.RegisterType<ChattingService>().As<IChattingService>();
                builder.RegisterType<MessageService>().As<IMessageService>();
                builder.RegisterType<RedisService>().As<IRedisService>();
                builder.RegisterType<Hubs>().AsSelf();
            });
            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            
            app.UseHttpsRedirection();

            app.UseStaticFiles();
            app.UseRouting();
            app.UseSession();
            app.UseAuthorization();
            app.MapHub<Hubs>("/chatHub");

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Auth}/{action=Index}");

            app.Run();
        }
    }
}