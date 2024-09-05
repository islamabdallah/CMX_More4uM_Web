using MoreForYou.Services.Models.API;
using MoreForYou.Services.Models.MasterModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace MoreForYou.Services.Contracts
{
    public interface IUserNotificationService
    {
        UserNotificationModel CreateUserNotification(UserNotificationModel model);

        List<UserNotificationModel> GetUserNotification(string userId);

        List<UserNotificationModel> GetFiftyUserNotification(string userId);

        List<NotificationAPIModel> CreateNotificationAPIModel(List<UserNotificationModel> userNotificationModels, int languageId);

        bool UpdateUserNotification(UserNotificationModel model);

        int GetUserUnseenNotificationCount(long employeeNumber);
        UserNotificationModel GetUserNotificationByUserIdAndNotificationId(string userId, long notificationId);
        public bool UpdateUserUnseenNotification(long employeeNumber);

    }
}
