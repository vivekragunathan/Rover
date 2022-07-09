using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Drawing;

namespace ThoughtWorks
{
   public class ExternalInput
   {
      private const string XLIM = "XLIM";
      private const string YLIM = "YLIM";
      private const string XPOS = "XPOS";
      private const string YPOS = "YPOS";
      private const string DIR = "DIR";
      private const string CMD = "CMD";
      private const string RVR = "RVR";

      private const char RotateLeft = 'L';
      private const char RotateRight = 'R';
      private const char Move = 'M';

      private static int lineIndex = 0;

      public static void ExecuteFile(Stream str)
      {
         RoverAppException.ThrowIfNull(str, "The specified Stream object reference is null.");

         using (StreamReader sr = new StreamReader(str))
         {
            string xyText = ReadLine(sr);
            MatchCollection mcx = Regex.Matches(xyText, Pattern.PlateauUpperXY);
            RoverAppException.ThrowIfTrue(mcx.Count == 0, "File Corrupted. Invalid plateau co-ordinates.");

            Match m = mcx[0];

            int upperX = m.Groups[ExternalInput.XLIM].Value.ToInt32();
            int upperY = m.Groups[ExternalInput.YLIM].Value.ToInt32();

            IPlateau ptu = new Plateau(new Size(upperX + 1, upperY + 1));


            while (!sr.EndOfStream)
            {
               try
               {
                  // Each rover has two lines of input. The first line gives the rover's 
                  // position, and the second line is a series of instructions telling 
                  // the rover how to explore the plateau.
                  string posText = ReadLine(sr);
                  string cmdText = ReadLine(sr);

                  ExternalInput.ExecuteCommand(ptu, posText, cmdText);
               }
               catch (RoverAppException ex)
               {
                  Logger.WriteLine("Error processing rover information at line index ({0}, {1}). {2}{3}",
                     ExternalInput.CurrentLineIndex() - 1,
                     ExternalInput.CurrentLineIndex(),
                     ex.Message,
                     Environment.NewLine);
               }
            }
         }
      }

      public static void ExecuteCommand(IPlateau ptu, string roverPositionText, string roverCmdText)
      {
         RoverAppException.ThrowIfNull(ptu, "Specified Plateau object reference is null.");

         Logger.WriteLine("Input: {0} ({1})", roverPositionText ?? "***EMPTY***", roverCmdText ?? "***EMPTY***");

         RoverInputData riData = ExternalInput.ParseRoverInput(roverPositionText, roverCmdText);

         IRover rvr = ptu.PlaceRover(riData.X, riData.Y, riData.Direction);
         ExecuteRoverCommands(rvr, riData.Command);

         Logger.WriteLine("Output: {0} {1} {2} {3}", rvr.Cell.X, rvr.Cell.Y, rvr.Direction, Environment.NewLine);
      }

      private static RoverInputData ParseRoverInput(string posText, string cmdText)
      {
         RoverAppException.ThrowIfTrue(string.IsNullOrEmpty(posText), "Position information is empty or zero-length.");

         MatchCollection mcx = Regex.Matches(posText, Pattern.Position);
         RoverAppException.ThrowIfTrue(mcx.Count == 0, "Position information is not in valid format. Format: X Y D");

         Match m = mcx[0];

         int x = 0;
         int y = 0;
         CompassPoint cpt = CompassPoint.North;

         RoverAppException.ThrowIfFalse(m.Groups[ExternalInput.XPOS].Value.IsInteger(ref x),
            "X position specified is not in valid format. Format: Positive Number (<= Plateau Upper X).");

         RoverAppException.ThrowIfFalse(m.Groups[ExternalInput.YPOS].Value.IsInteger(ref y),
            "Y position specified is not in valid format. Format: Positive Number (<= Plateau Upper Y).");

         RoverAppException.ThrowIfFalse(m.Groups[ExternalInput.DIR].Value.IsCompassPoint(ref cpt),
            "Direction specified is not valid. Format: N or E or W or S.");

         RoverAppException.ThrowIfFalse(Regex.IsMatch(cmdText, Pattern.Command),
            "Rover command is not in valid format. Format: [L][M][R].");

         return new RoverInputData(x, y, cpt, cmdText);
      }

      private static void ExecuteRoverCommands(IRover rvr, string command)
      {
         Debug.Assert(rvr != null);
         Debug.Assert(!string.IsNullOrEmpty(command));

         foreach (char ch in command)
         {
            switch (ch)
            {
               case ExternalInput.RotateLeft:
                  rvr.RotateLeft();
                  break;

               case ExternalInput.RotateRight:
                  rvr.RotateRight();
                  break;

               case ExternalInput.Move:
                  rvr.Move();
                  break;

               default:
                  Debug.Assert(false);
                  break;
            }
         }
      }

      private static int IncrementLineIndex()
      {
         return ++lineIndex;
      }

      private static string ReadLine(StreamReader sr)
      {
         string text = sr.ReadLine();
         IncrementLineIndex();
         return text;
      }

      private static int CurrentLineIndex()
      {
         return lineIndex;
      }

      #region Nested Types

      struct RoverInputData
      {
         public int X;
         public int Y;
         public CompassPoint Direction;
         public string Command;

         public RoverInputData(int x, int y, CompassPoint dir, string cmdText)
         {
            Debug.Assert(!string.IsNullOrEmpty(cmdText));

            X = x;
            Y = y;
            Direction = dir;
            Command = cmdText;
         }
      }

      static class Pattern
      {
         public const string PlateauUpperXY = @"^(?<XLIM>[0-9]+)\s(?<YLIM>[0-9]+)\s*$";
         public const string Position = @"^(?<XPOS>\w+)\s(?<YPOS>\w+)\s(?<DIR>\w+)\s*$";
         public const string Command = @"^\s*(?<CMD>[LMR]+)\s*$";
      }

      #endregion
   }
}