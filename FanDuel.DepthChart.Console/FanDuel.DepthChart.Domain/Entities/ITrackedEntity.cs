namespace FanDuel.DepthChart.Domain.Entities
{
    public interface ITrackedEntity
    {
        DateTime CreatedDateTime { get; set; }
        int CreatedBy { get; set; }
    }
}
