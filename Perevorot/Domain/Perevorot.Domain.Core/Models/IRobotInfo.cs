namespace Winner.Domain.Core.Models
{
    public interface IRobotInfo
    {
        #region Public members

        int PositionX { get; }

        int PositionY { get; }

        int Angle { get; }

        int CannonAngle { get; }

        int Life { get; }

        #endregion
    }
}