using ThoughtWorks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Drawing;

namespace ThoughtWorks.UnitTests
{
   [TestClass]   
   public class PlateauTest
   {
      public PlateauTest()
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

      internal static IPlateau CreatePlateauAndVerify(int width, int height)
      {
         IPlateau ptu = new Plateau(new Size(width, height));
         PlateauTest.VerifyBounds(ptu, new LRectangle(0, 0, width, height));
         return ptu;
      }

      private static void VerifyBounds(IPlateau ptu, LRectangle expectedBounds)
      {
         Assert.IsTrue(ptu != null, "Plateau object specified is null.");
         Assert.IsTrue(ptu.Bounds == expectedBounds, "Plateau bounds incorrect {0}. Expected: {1}", ptu.Bounds, expectedBounds);
         Assert.IsTrue(ptu.Bounds.GetHashCode() == expectedBounds.GetHashCode(), "HashCode check failure!");
      }

      [TestMethod]      
      [ExpectedException(typeof(RoverAppException), Plateau.XLimitInvalidErrorText)]
      public void UpperXLessThanZero()
      {
         IPlateau p = CreatePlateauAndVerify(-1, 6);
      }

      [TestMethod]      
      [ExpectedException(typeof(RoverAppException), Plateau.YLimitInvalidErrorText)]
      public void UpperYLessThanZero()
      {
         IPlateau p = CreatePlateauAndVerify(6, -1);
      }

      [TestMethod]      
      [ExpectedException(typeof(RoverAppException), Plateau.XLimitInvalidErrorText)]
      public void UpperXZero()
      {
         IPlateau p = CreatePlateauAndVerify(0, 6);
      }

      [TestMethod]
      [ExpectedException(typeof(RoverAppException), Plateau.YLimitInvalidErrorText)]
      public void UpperYZero()
      {
         IPlateau p = CreatePlateauAndVerify(6, 0);
      }

      [TestMethod]
      [Description("Create Plateau with (5, 5) as upper right co-ordinates and access a few cells.")]
      public void PlateauCreationAndCellAccess()
      {
         IPlateau p = CreatePlateauAndVerify(6, 6);

         ICell iCell = p.GetCell(0, 0);
         iCell = p.GetCell(3, 3);
         iCell = p.GetCell(5, 5);
      }

      [TestMethod]
      [Description("Create plateau and access a cell outside the plateau")]
      [ExpectedException(typeof(RoverAppException))]
      public void InvalidCellAccess()
      {
         IPlateau p = CreatePlateauAndVerify(6, 6);
         ICell iCell = p.GetCell(6, 6);
      }

      [TestMethod]
      [Description("Cannot place a rover outside the plateau")]
      [ExpectedException(typeof(RoverAppException))]
      public void CantPlaceRoverOutsidePlateau()
      {
         IPlateau p = CreatePlateauAndVerify(6, 6);
         p.PlaceRover(6, 6, CompassPoint.East);
      }

      [TestMethod]
      [Description("Get number of rovers")]
      public void GetNoOfRovers()
      {
         int width = 3;
         int height = 3;
         IPlateau ptu = CreatePlateauAndVerify(width, height);
         int index = 0;

         for (int x = 0; x < width; ++x)
         {
            for (int y = 0; y < height; ++y)
            {
               ptu.PlaceRover(x, y, CompassPoint.North);

               ++index;
               int numRovers = ptu.NoOfRovers();

               Assert.IsTrue(numRovers == index, string.Format("Incorrect rover count: {0}. Expected: {1}.", numRovers, index));
            }
         }
      }

      [TestMethod]
      [Description("GetRoverAt negative test")]
      public void GetRoverAtFailureTest()
      {
         IPlateau ptu = CreatePlateauAndVerify(3, 4);
         ICell iCell = ptu.GetCell(0, 0);
         IRover rvr = ptu.GetRoverAt(iCell);
         Assert.IsNull(rvr, "Expected null Rover object at cell {0}", iCell);
      }

      [TestMethod]
      [Description("Get rover test")]
      public void GetRoverAtTest()
      {
         int width = 3;
         int height = 3;
         IPlateau ptu = CreatePlateauAndVerify(width, height);
         int index = 0;

         for (int x = 0; x < width; ++x)
         {
            for (int y = 0; y < height; ++y)
            {
               ptu.PlaceRover(x, y, CompassPoint.North);

               ICell iCell = ptu.GetCell(x, y);
               IRover rvr = ptu.GetRoverAt(iCell);
               Assert.IsNotNull(rvr, "Expected non-null Rover object at cell {0}", iCell);

               ++index;
               int numRovers = ptu.NoOfRovers();

               Assert.IsTrue(numRovers == index, string.Format("Incorrect rover count: {0}. Expected: {1}.", numRovers, index));
            }
         }
      }

      [TestMethod]
      [Description("Get rover test")]
      [ExpectedException(typeof(RoverAppException))]
      public void RoverAlreadyPresentTest()
      {
         IPlateau ptu = CreatePlateauAndVerify(3, 4);
         IRover rvr = ptu.PlaceRover(1, 2, CompassPoint.West);
         rvr = ptu.PlaceRover(1, 2, CompassPoint.North);
      }
   }
}