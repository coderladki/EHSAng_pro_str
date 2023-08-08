using System.Threading.Tasks;

namespace CRM.Server.Web.Api.Services
{
    public interface INotificationService
    {
        Task<bool> SendNotification(NotificationModel notificationModel);
    }
}
