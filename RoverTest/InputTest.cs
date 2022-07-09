using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using System.Text;

namespace ThoughtWorks.UnitTests
{
   [TestClass]
   public class InputTests
   {
      #region Member Data

      private const string PositiveInputText = "2 2\r\n" +
         "2 2 E\r\nRMRMMLM\r\n" +
         "1 2 N\r\nMLMMLM\r\n" +
         "4 2 W\r\nMLMLMLMMRR\r\n" +
         "0 2 S\r\nLMMRMM\r\n";

      private const string NegativeInputText = "2 2\r\n" +
         "2 0 S\r\nLMMRMMRMRR\r\n" +
         "6 5 S\r\nLMMRMMRMRR\r\n" +
         "5 3 A\r\nLMMRMMRMRR\r\n" +
         "5 5 S\r\nMMMRMMMM\r\n";

      /// <summary>
      ///Gets or sets the test context which provides
      ///information about and functionality for the current test run.
      ///</summary>
      private TestContext testContextInstance { get; set; }

      #endregion

      #region Ctor(s)

      public InputTests()
      {
      }

      #endregion

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

      [TestMethod]
      [Description("ExecuteCommand +ve Test")]
      public void ExecCmdTest()
      {
         IPlateau ptu = PlateauTest.CreatePlateauAndVerify(6, 5);

         ExternalInput.ExecuteCommand(ptu, "3 4 W", "LMMMR");

         ICell iCell = ptu.GetCell(3, 1);

         IRover rvr = null;
         bool bOcc = iCell.IsOccupied(out rvr);
         Assert.IsTrue(bOcc, "Expected cell {0} to be occupied.", iCell);
         Assert.IsNotNull(rvr, "Expected non-null rover object at cell {0}", iCell);
         Assert.IsTrue(rvr.Direction == CompassPoint.West, "Expected rover at cell {0} to be directed {1}", iCell, CompassPoint.West);
      }

      [TestMethod]
      [ExpectedException(typeof(RoverAppException))]
      public void ValidRoverCommandText()
      {
         IPlateau ptu = PlateauTest.CreatePlateauAndVerify(3, 3);
         ExternalInput.ExecuteCommand(ptu, "2 3 W", "MMLMMM");
      }

      [TestMethod]
      [ExpectedException(typeof(RoverAppException))]
      public void OutOfPlateauTest()
      {
         IPlateau ptu = PlateauTest.CreatePlateauAndVerify(6, 5);
         ExternalInput.ExecuteCommand(ptu, "8 4 W", "LMMMR");
      }

      [TestMethod]
      [ExpectedException(typeof(RoverAppException))]
      public void InvalidCoordinatesTest()
      {
         IPlateau ptu = PlateauTest.CreatePlateauAndVerify(6, 5);
         ExternalInput.ExecuteCommand(ptu, "X Y W", "MMMR");
      }
      
      [TestMethod]
      [ExpectedException(typeof(RoverAppException))]
      public void InvalidDirectionTest()
      {
         IPlateau ptu = PlateauTest.CreatePlateauAndVerify(6, 5);
         ExternalInput.ExecuteCommand(ptu, "3 2 Q", "MMMR");
      }

      [TestMethod]
      [ExpectedException(typeof(RoverAppException))]
      public void InvalidRoverCommandTest()
      {
         IPlateau ptu = PlateauTest.CreatePlateauAndVerify(6, 5);
         ExternalInput.ExecuteCommand(ptu, "3 2 S", "AMMMR");
      }

      [TestMethod]
      [ExpectedException(typeof(RoverAppException))]
      public void RoverAlreadyPresentTest()
      {
         IPlateau ptu = PlateauTest.CreatePlateauAndVerify(6, 5);
         ExternalInput.ExecuteCommand(ptu, "3 3 S", "MMMR");
         ExternalInput.ExecuteCommand(ptu, "3 0 W", "MMMR");
      }

      [TestMethod]
      [Description("ExecuteFile +ve Test")]
      public void ExecuteFilePositiveTest()
      {
         MemoryStream ms = new MemoryStream(Encoding.Default.GetBytes(InputTests.PositiveInputText));
         ExternalInput.ExecuteFile(ms);
      }

      [TestMethod]
      [Description("ExecuteFile -ve Test")]
      public void ExecuteFileNegativeTest()
      {
         MemoryStream ms = new MemoryStream(Encoding.Default.GetBytes(InputTests.NegativeInputText));
         ExternalInput.ExecuteFile(ms);
      }
   }
}