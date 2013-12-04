namespace Winner.Domain.Core.Models
{
    public interface IRobotEvents
    {
        #region Public members

        void OnIdle(IRobot robot);

        void OnScanFound(IRobot robot, IRobotInfo rivalRobot);

        void OnWallCollision(IRobot robot);

        void OnHit(IRobot robot);

        #endregion
    }
}