using Arcoro.Common.Model.Enum;

namespace Arcoro.Common.Model.Notification
{
    public class CreateRegistration
    {
        public SubscriptionMode Mode { get; set; }
        public string TypeId { get; set; }
    }
}