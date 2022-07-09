using System;
using System.Drawing;
using System.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ThoughtWorks.UnitTests
{
   [TestClass]
   public class CellTest
   {
      private const int Width = 6;
      private const int Height = 5;

      public CellTest()
      {
      }

      /// <summary>
      ///Gets or sets the test context which provides
      ///information about and functionality for the current test run.
      ///</summary>
      private TestContext testContextInstance { get; set; }

      #region Additional test attributes

      // 
      //You can use the following additional attributes as you write your tests:
      //
      //Use ClassInitialize to run code before running the first test in the class
      //[ClassInitialize()]
      //public static void MyClassInitialize(TestContext testContext)
      //{
      //}
      //
      //Use ClassCleanup to run code after all tests in a class have run
      //[ClassCleanup()]
      //public static void MyClassCleanup()
      //{
      //}
      //
      //Use TestInitialize to run code before running each test
      //[TestInitialize()]
      //public void MyTestInitialize()
      //{
      //}
      //
      //Use TestCleanup to run code after each test has run
      //[TestCleanup()]
      //public void MyTestCleanup()
      //{
      //}
      //

      #endregion

      [TestMethod][Description("Cell creation and access")]
      public void CellCreation()
      {
         IPlateau ptu = PlateauTest.CreatePlateauAndVerify(CellTest.Width, CellTest.Height);

         for (int w = 0; w < ptu.Bounds.Width; ++w)
         {
            for (int h = 0; h < ptu.Bounds.Height; ++h)
            {
               ICell iCell = ptu.GetCell(w, h);
               Assert.IsTrue(iCell.X == w, "X position invalid {0}. Expected: {1}.", iCell.X, w);
               Assert.IsTrue(iCell.Y == h, "Y position invalid {0}. Expected: {1}.", iCell.Y, h);
               Assert.IsNotNull(iCell.Plateau, "Plateau is null for {0}", iCell);
               Assert.IsFalse(iCell.IsOccupied(), "Cell {0} expected to be unoccupied", iCell);
            }
         }
      }

      [TestMethod][Description("Throw if Rover accessed on an unoccupied cell")]
      [ExpectedException(typeof(RoverAppException))]
      public void ThrowIfUnoccupuied()
      {
         IPlateau ptu = PlateauTest.CreatePlateauAndVerify(CellTest.Width, CellTest.Height);
         ICell iCell = ptu.GetCell(0, 0);
         IRover rObj = iCell.Rover;
      }

      [TestMethod][Description("Cell Occupied Test")]
      public void CellOccupiedTest()
      {
         //Debug.Assert(false, "Break into debugger");
         IPlateau ptu = PlateauTest.CreatePlateauAndVerify(CellTest.Width, CellTest.Height);

         IRover rvr = ptu.PlaceRover(ptu.Bounds.LowerX, ptu.Bounds.LowerY, CompassPoint.North);
         ICell iCell = ptu.GetCell(ptu.Bounds.LowerX, ptu.Bounds.LowerY);
         Assert.IsTrue(iCell.IsOccupied(), "Cell {0} expected to be occupied by a rover.", iCell);

         IRover rObj = null;
         Assert.IsTrue(iCell.IsOccupied(out rObj), "Cell {0} expected to be occupied by a rover.");
         Assert.IsTrue(rObj != null, "Expected non-null rover object at cell {0}", iCell);

         IRover rObj2 = iCell.Rover;
         Assert.IsNotNull(rObj2, "Expected non-null rover object at cell {0}", iCell);
      }
   }
}