using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RoomsOfDoom;

namespace TestRoomsOfDoom
{
    [TestClass]
    public class EnemyTest : HittableTest
    {

        public EnemyTest() : base()
        {

        }

        public override IHittable getHittable()
        {
            return new Enemy("test",'T', 100);
        }
    }
}
