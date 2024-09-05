using MoreForYou.Services.Models.MasterModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MoreForYou.Services.Contracts
{
    public interface INotificationService
    {
        NotificationModel CreateNotification(NotificationModel model);

        NotificationModel GetNotification(int id);

        Task<bool> UpdateNotification(NotificationModel model);

        List<NotificationModel> GetUnseenNotifications();
        List<NotificationModel> GetAllUnseenNotificationsForEmployee(long EmployeeNumber);

        NotificationModel GetNotificationByRequestWorkflowId(long requestWorkflowId);

    }
}
