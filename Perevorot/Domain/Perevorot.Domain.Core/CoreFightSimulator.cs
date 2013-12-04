namespace Winner.Domain.Core
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Models;

    public class CoreFightSimulator
    {
        private readonly Robot _alphaRobot;
        private readonly Robot _betaRobot;
        private IList<Bullet> _firedBullets;

        public CoreFightSimulator(Robot alphaRobot, Robot betaRobot)
        {
            if (alphaRobot == null)
                throw new ArgumentNullException("alphaRobot");
            if (betaRobot == null)
                throw new ArgumentNullException("betaRobot");
            _alphaRobot = alphaRobot;
            _betaRobot = betaRobot;
        }

        public FightSimulationResult Simulate()
        {
            //initialization of all the games objects
            Init();

            while (_alphaRobot.Life > 0 && _betaRobot.Life >0)
            {
                //make robot's actions
                _alphaRobot.OnIdle(_alphaRobot);
                _betaRobot.OnIdle(_betaRobot);

                //check if bullets are hitting something
                if (!_firedBullets.Any())
                {
                    MoveBullets();
                }
            }

            if (_alphaRobot.Life == 0)
            {
                return new FightSimulationResult(_alphaRobot.GroupId);
            }
            return _betaRobot.Life == 0 ? new FightSimulationResult(_betaRobot.GroupId) : null;
        }

        private void MoveBullets()
        {
            foreach (var firedBullet in _firedBullets)
            {
                firedBullet.Fly();
                if (firedBullet.PositionX == _alphaRobot.PositionX && firedBullet.PositionY == _alphaRobot.PositionY)
                {
                    _alphaRobot.Hit();
                }
                if (firedBullet.PositionX == _betaRobot.PositionX && firedBullet.PositionY == _betaRobot.PositionY)
                {
                    _betaRobot.Hit();
                }
            }
        }

        private void Init()
        {
            var random = new Random();
            _alphaRobot.SetPositionX(random.Next());
            _alphaRobot.SetPositionY(random.Next());
        }
    }
}