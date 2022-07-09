using System;
using System.Diagnostics;
using System.Drawing;

namespace ThoughtWorks
{
   static class Compass
   {
      public const int NoOfDirections = 4;

      /// <summary>
      /// This method determines the left of the current compass point
      /// but does not change the direction yet. To determine and also
      /// change the direction, use RotateLeft.
      /// </summary>
      public static CompassPoint GetLeftOf(CompassPoint cpt)
      {
         int c = (int)cpt - 1;
         return (CompassPoint)(c < 0 ? Compass.NoOfDirections + c : c);
      }

      /// <summary>
      /// This method determines the right of the current compass point
      /// but does not change the direction yet. To determine and also
      /// change the direction, use RotateRight.
      /// </summary>
      public static CompassPoint GetRightOf(CompassPoint cpt)
      {
         return (CompassPoint)(((int)cpt + 1) % Compass.NoOfDirections);
      }

      public static CompassPoint RotateLeft(ref CompassPoint cpt)
      {
         cpt = GetLeftOf(cpt);
         return cpt;
      }

      public static CompassPoint RotateRight(ref CompassPoint cpt)
      {
         cpt = GetRightOf(cpt);
         return cpt;
      }

      /// <summary>
      /// Get the next (x, y) position in the specified direction.
      /// This method does not check for plateau boundary violations.
      /// Those checks are made in the core logic.
      /// </summary>
      public static Point GetNextPosition(int x, int y, CompassPoint cpt)
      {
         switch (cpt)
         {
            case CompassPoint.North:
               ++y;
               break;

            case CompassPoint.East:
               ++x;
               break;

            case CompassPoint.South:
               --y;
               break;

            case CompassPoint.West:
               --x;
               break;
         }

         return new Point(x, y);
      }
   }

   /// <summary>
   /// A nifty little class for writing diagnostic log messages to stderr.
   /// </summary>
   class Logger
   {
      /// <summary>
      /// Writes messages to the console (and to the stderr device)
      /// </summary>
      public static void WriteLine(string fmtSpec, params object[] args)
      {
         string text = string.Format(fmtSpec ?? string.Empty, args);
         Console.WriteLine(text);
         WriteTrace(text);
      }

      /// <summary>
      /// Writes messages to the stderr device
      /// </summary>
      public static void TraceLine(string fmtSpec, params object[] args)
      {
         string text = string.Format(fmtSpec ?? string.Empty, args);
         WriteTrace(text);
      }

      public static void TraceLine(string text)
      {
         WriteTrace(text ?? string.Empty);
      }

      public static void Trace(string fmtSpec, params object[] args)
      {
         string text = string.Format(fmtSpec ?? string.Empty, args);
         WriteTrace(text);
      }

      public static void Trace(string text)
      {
         WriteTrace(text ?? string.Empty);
      }

      private static void WriteTrace(string text)
      {
         System.Diagnostics.Trace.WriteLine(text);
      }
   }

   static class Extensions
   {
      public static int ToInt32(this string text)
      {
         return Convert.ToInt32(text);
      }

      public static bool IsInteger(this string text, ref int num)
      {
         return Int32.TryParse(text, out num);
      }

      public static bool IsCompassPoint(this string text, ref CompassPoint cpt)
      {
         switch (text)
         {
            case "N": cpt = CompassPoint.North; return true;
            case "E": cpt = CompassPoint.East; return true;
            case "W": cpt = CompassPoint.West; return true;
            case "S": cpt = CompassPoint.South; return true;
         }

         return false;
      }
   }
}