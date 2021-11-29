namespace Arcoro.Common.Model.Notification
{
    public class ChangeEvent
    {
        public string LastUpdatedOnUtc { get; set; }
        public string RootEntityId { get; set; }
        public string TypeId { get; set; }
        public long Version { get; set; }
    }
}