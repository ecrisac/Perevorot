namespace Winner.Domain.Core.Models
{
    public interface IBullet
    {
        #region Public members

        int PositionX { get; set; }

        int PositionY { get; set; }

        void Fly();

        #endregion
    }
}