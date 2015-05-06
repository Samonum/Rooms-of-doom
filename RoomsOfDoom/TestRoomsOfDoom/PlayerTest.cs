using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RoomsOfDoom;

namespace TestRoomsOfDoom
{
    [TestClass]
    public class PlayerTest : HittableTest
    {
        public PlayerTest() : base()
        {
        }

        public override IHittable getHittable()
        {
            return new Player();
        }
    }
}
