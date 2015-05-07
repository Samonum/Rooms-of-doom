using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RoomsOfDoom;
using System.Drawing;

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

        [TestMethod]
        public void TestMovement()
        {
            //arrange
            Player p = new Player();
            Random r = new Random();
            MonsterCreator M = new MonsterCreator(r, 10);
            Pack testPack = M.GeneratePack(1);
            p.Location = new Point(5, 5);
            //act and assert
            p.Move(Direction.Up, testPack);
            Assert.IsTrue(p.Location.X == 5 && p.Location.Y == 4);
            p.Move(Direction.Left, testPack);
            Assert.IsTrue(p.Location.X == 4 && p.Location.Y == 4);
            p.Move(Direction.Down, testPack);
            Assert.IsTrue(p.Location.X == 4 && p.Location.Y == 5);
            p.Move(Direction.Right, testPack);
            Assert.IsTrue(p.Location.X == 5 && p.Location.Y == 5);
        }

        [TestMethod]
        public void TestMovementObstructed()
        {
            //arrange
            Player p = new Player();
            Random r = new Random();
            MonsterCreator M = new MonsterCreator(r, 10);
            Pack testPack = M.GeneratePack(1);
            p.Location = new Point(5,1);
            //act and assert
            p.Move(Direction.Up, testPack);
            Assert.IsTrue(p.Location.X == 5 && p.Location.Y == 1);
            p.Location = new Point(1, 5);
            p.Move(Direction.Left, testPack);
            Assert.IsTrue(p.Location.X == 1 && p.Location.Y == 5);
            p.Location = new Point(6, Arena.Height -2);
            p.Move(Direction.Down, testPack);
            Assert.IsTrue(p.Location.X == 6 && p.Location.Y == Arena.Height -2);
            p.Location = new Point(Arena.Width - 2, 5);
            p.Move(Direction.Right, testPack);
            Assert.IsTrue(p.Location.X == Arena.Width -2 && p.Location.Y == 5);
        }

        [TestMethod]
        public void TestMovementObstructedByEnemy()
        {
            //arrange
            Player p = new Player();
            Random r = new Random();
            MonsterCreator M = new MonsterCreator(r, 10);
            Pack testPack = M.GeneratePack(1);
            Arena a = new Arena(Exit.Right, testPack, p, Exit.Right, r);
            testPack[0].Location = new Point(5, 5);
            p.Location = new Point(5,4);
            //act and assert
            p.Move(Direction.Down, testPack);
            Assert.IsTrue(p.Location.X == 5 && p.Location.Y == 4);

        }

        [TestMethod]
        public void PlayerTestAllTheThings()
        {
            Player p = new Player();
            char x = p.Glyph;
            p.CurrentHP = 50;
            for(int i = 0; i < 100; i++)
            {
                p.CurrentHP++;
            }

            Assert.IsTrue(p.MaxHP >= p.CurrentHP);

        }
    }
}
