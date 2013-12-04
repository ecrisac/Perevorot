namespace Winner.Domain.Core.Models
{
    public interface IRobotFullInfo : IRobotInfo
    {
        #region Public members

        int GunCooldownTime { get; }

        #endregion
    }
}