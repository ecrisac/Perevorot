namespace Winner.Domain.Core.Models
{
    public interface IRobotBehavior
    {
        #region Public members

        void Move(int amount);

        void RotateCannon(int degrees);

        void Turn(int degrees);

        void Fire();

        void Stop();

        #endregion
    }
}