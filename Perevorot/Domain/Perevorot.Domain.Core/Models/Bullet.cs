namespace Winner.Domain.Core.Models
{
    public class Bullet : IBullet
    {
        public int PositionX { get; set; }

        public int PositionY { get; set; }

        public void Fly()
        {
            throw new System.NotImplementedException();
        }
    }
}