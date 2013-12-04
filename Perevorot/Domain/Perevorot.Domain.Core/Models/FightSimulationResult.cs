namespace Winner.Domain.Core.Models
{
    public class FightSimulationResult
    {
        public FightSimulationResult(long groupId)
        {
            WinnerId = groupId;
        }

        public long WinnerId { get; private set; }
    }
}