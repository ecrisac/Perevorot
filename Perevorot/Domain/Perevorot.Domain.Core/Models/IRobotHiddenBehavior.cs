namespace Winner.Domain.Core.Models
{
    public interface IRobotHiddenBehavior
    {
        #region Public members

        long GroupId { get; }
        bool SetPositionX(int positionX);
        bool SetPositionY(int positionY);
        void Hit();

        #endregion
    }
}