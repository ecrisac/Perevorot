using NUnit.Framework;

namespace Winner.Domain.Services.Tests
{
    [TestFixture]
    class TestRuntimeCompilation
    {
        [Test]
        public void TestRuntimeGeneration()
        {
            var assemblyResults = new RuntimeCompiler()
                .CreateInstance(@"  using System;
                                    using Winner.Domain.Core.Models;
                                    public class RobotBehavior : IRobotBehavior
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
                                    }");

            Assert.NotNull(assemblyResults, "assembly should exist");

        }
    }
}
