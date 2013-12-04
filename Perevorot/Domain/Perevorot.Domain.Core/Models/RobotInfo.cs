namespace Winner.Domain.Core.Models
{
    public class RobotInfo : IRobotInfo
    {
        #region IRobotInfo Members

        public int PositionX { get; private set; }

        public int PositionY { get; private set; }

        public int Angle { get; private set; }

        public int CannonAngle { get; private set; }

        public int Life { get; private set; }

        #endregion
    }
}