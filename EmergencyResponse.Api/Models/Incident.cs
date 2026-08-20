namespace EmergencyResponse.Api.Models
{
    public class Incident
    {
        public int Id { get; set; }
        public string Type { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Location { get; set; } = string.Empty;
        public IncidentStatus Status { get; set; } = IncidentStatus.Reported;
        public UrgencyLevel Urgency { get; set; } = UrgencyLevel.Medium;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public List<IncidentUnit> IncidentUnits { get; set; } = new();
    }

    public enum IncidentStatus
    {
        Reported,
        InProgress,
        Resolved
    }

    public enum UrgencyLevel
    {
        Low,
        Medium,
        High,
        Critical
    }
}