using Arcoro.Common.Model.Enum;

namespace Arcoro.Common.Model.Notification
{
    public class Registration
    {
        public string CreatedOnUtc { get; set; }
        public SubscriptionMode Mode { get; set; }
        public string TypeId { get; set; }
    }
}