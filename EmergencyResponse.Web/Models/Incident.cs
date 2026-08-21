namespace EmergencyResponse.Web.Models
{
    public class Incident
    {
        public int Id { get; set; }
        public string Type { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Location { get; set; } = string.Empty;
        public int Status { get; set; }
        public int Urgency { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}