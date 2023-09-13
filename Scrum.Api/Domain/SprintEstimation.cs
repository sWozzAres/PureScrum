namespace Scrum.Api.Domain
{
    public class SprintEstimation(Guid sprintId)
    {
        public Guid SprintId { get; set; } = sprintId;
        public Sprint Sprint { get; private set; } = null!;
        public float NoneTotal { get; set; } = 0F;
        public float ReadyTotal { get; set; } = 0F;
        public float DoneTotal { get; set; } = 0F;
        public DateTime InstantTime { get; set; } = DateTime.Now;
    }
}
