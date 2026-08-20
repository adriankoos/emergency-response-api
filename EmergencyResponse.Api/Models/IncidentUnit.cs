namespace EmergencyResponse.Api.Models
{
    public class IncidentUnit
    {
        public int Id { get; set; }

        public int IncidentId { get; set; }
        public Incident? Incident { get; set; }

        public int UnitId { get; set; }
        public Unit? Unit { get; set; }
    }
}