using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.RepositoryInterfaces;
using Application.ServiceInterfaces;
using Application.Services;
using Application.Services.Common;
using Core.Entities;
using Infrastructure.SQL.Context;
using Infrastructure.SQL.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace Endpoint.Configs
{
    public static class AddServices
    {
        public static void AddTweeterServices(this IServiceCollection services)
        {
            services.AddSingleton<ITagCrudService, TagCrudService>();
            services.AddSingleton<ITagRepository, TagRepository>();
            services.AddScoped<IUserCrudService, UserCrudService>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddSingleton<ITweetRepository, TweetRepository>();
            services.AddSingleton<ITweetCrudService, TweetCrudService>();
            services.AddScoped<IRoleCrudService, RoleCrudService>();
            services.AddScoped<IRoleRepository, RoleRepository>();
            services.AddScoped<IAccountRepository, AccountRepository>();
            services.AddScoped<IAccountManageService, AccountManageService>();

            services.AddSingleton<VisitSummaryTweet>();

            services.AddIdentity<User, Role>()
                .AddEntityFrameworkStores<DatabaseContext>()
                .AddRoles<Role>();

        }
    }
}
