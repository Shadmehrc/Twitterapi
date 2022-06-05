using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.RepositoryInterfaces;
using Core.Entities;
using Dapper;
using Infrastructure.SQL.Context;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace Infrastructure.SQL.Repositories
{
    public class NotificationRepository :  INotificationRepository
    {
        private readonly string _connectionString;
        private readonly DatabaseContext _context;

        public NotificationRepository(IConfiguration configuration, DatabaseContext context)
        {
            _context = context;
            _connectionString = configuration["connection"];
        }

        public Task<bool> SendTaggedUserNotification(string userId, string notifReceiverId)
        {
            var sqlConnection = new SqlConnection(_connectionString);
            var result = sqlConnection.Query<bool>("SendUserTaggedNotification",
                new { UserId = userId, notifReceiverId = notifReceiverId },
                commandType: CommandType.StoredProcedure);
            return Task.FromResult(true);
        }

        public Task<List<Notification>> GetUserNotifications(string userId)
        {
            var result = _context.Notifications.Where(x => x.UserId == userId).ToList();
            return Task.FromResult(result);

        }
    }
}
