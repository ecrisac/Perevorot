namespace Winner.Domain.Core.Models
{
    public class RobotFullInfo : RobotInfo, IRobotFullInfo
    {
        #region IRobotFullInfo Members

        public int GunCooldownTime { get; private set; }

        #endregion
    }
}