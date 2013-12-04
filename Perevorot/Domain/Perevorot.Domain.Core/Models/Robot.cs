namespace Winner.Domain.Core.Models
{
    public class Robot : RobotFullInfo, IRobot, IRobotHiddenBehavior
    {
        public void Move(int amount)
        {
            throw new System.NotImplementedException();
        }

        public void RotateCannon(int degrees)
        {
            throw new System.NotImplementedException();
        }

        public void Turn(int degrees)
        {
            throw new System.NotImplementedException();
        }

        public void Fire()
        {
            throw new System.NotImplementedException();
        }

        public void Stop()
        {
            throw new System.NotImplementedException();
        }

        public void OnIdle(IRobot robot)
        {
            throw new System.NotImplementedException();
        }

        public void OnScanFound(IRobot robot, IRobotInfo rivalRobot)
        {
            throw new System.NotImplementedException();
        }

        public void OnWallCollision(IRobot robot)
        {
            throw new System.NotImplementedException();
        }

        public void OnHit(IRobot robot)
        {
            throw new System.NotImplementedException();
        }

        public long GroupId { get; private set; }

        public bool SetPositionX(int positionX)
        {
            throw new System.NotImplementedException();
        }

        public bool SetPositionY(int positionX)
        {
            throw new System.NotImplementedException();
        }

        public void Hit()
        {
            throw new System.NotImplementedException();
        }
    }
}