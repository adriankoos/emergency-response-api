namespace EmergencyResponse.Api.Models
{
    public class Unit
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public UnitType Type { get; set; }
        public UnitStatus Status { get; set; } = UnitStatus.Available;
    }

    public enum UnitType
    {
        Firefighters,
        Ambulance,
        Police
    }

    public enum UnitStatus
    {
        Available,
        Busy
    }
}