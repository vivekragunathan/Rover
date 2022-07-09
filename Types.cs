using System;
using System.Collections.Generic;
using System.Drawing;
using System.Diagnostics;

namespace ThoughtWorks
{
   public enum CompassPoint : byte
   {
      North = 0,
      East = 1,
      South = 2,
      West = 3
   }

   public interface IPlateau
   {
      LRectangle Bounds { get; }
      ICell GetCell(int x, int y);
      int NoOfRovers();
      IRover PlaceRover(int x, int y, CompassPoint cpt);
      IRover GetRoverAt(ICell iCell);
   }
   
   public interface IRover
   {
      string Name { get; }
      ICell Cell { get; }
      ICell Move();
      CompassPoint RotateLeft();
      CompassPoint RotateRight();
      CompassPoint Direction { get; }
   }

   public interface ICell
   {
      int X { get; }
      int Y { get; }
      bool IsOccupied();
      bool IsOccupied(out IRover rover);
      IRover Rover { get; }
      IPlateau Plateau { get; }
   }

   public class RoverAppException : Exception
   {
      public RoverAppException(string msgText)
         : base(msgText ?? string.Empty)
      {
      }

      public static void Throw(string fmtSpec, params object[] args)
      {
         RoverAppException.ThrowIfTrue(true, fmtSpec, args);
      }

      public static void ThrowIfFalse(bool condition, string fmtSpec, params object[] args)
      {
         string errorText = string.Format(fmtSpec ?? string.Empty, args);
         RoverAppException.ThrowIfTrue(!condition, errorText);
      }

      public static void ThrowIfNull<T>(T obj, string fmtSpec, params object[] args) where T : class
      {
         string errorText = string.Format(fmtSpec ?? string.Empty, args);
         RoverAppException.ThrowIfTrue(obj == null, errorText);
      }

      public static void ThrowIfTrue(bool condition, string fmtSpec, params object[] args)
      {
         if (condition)
         {
            string errorText = string.Format(fmtSpec ?? string.Empty, args);
            throw new RoverAppException(errorText);
         }
      }
   }

   /// <summary>
   /// A rectangle class that takes the lower-left co-ordinates. The
   /// rectangle in FCL takes upper-left (x, y) co-ordinates.
   /// </summary>
   public struct LRectangle
   {
      public static readonly LRectangle Empty;

      private int _lowerX;
      private int _lowerY;
      private int _width;
      private int _height;

      public LRectangle(Point lowerCoords, Size sz)
         : this(lowerCoords.X,
            lowerCoords.Y,
            sz.Width,
            sz.Height)
      {
         LowerX = lowerCoords.X;
      }

      public LRectangle(int lowerX, int lowerY, int width, int height)
      {
         _lowerX = lowerX;
         _lowerY = lowerY;
         _width = width;
         _height = height;
      }

      public bool IsPointInRect(Point pt)
      {
         return IsPointInRect(pt.X, pt.Y);
      }

      public bool IsPointInRect(int x, int y)
      {
         bool xOK = (x >= LowerX && x <= (LowerX + Width - 1));
         bool yOK = (y >= LowerY && y <= (LowerY + Height - 1));
         return xOK && yOK;
      }

      public int LowerX
      {
         get
         {
            return _lowerX;
         }
         set
         {
            _lowerX = value;
         }
      }

      public int LowerY
      {
         get
         {
            return _lowerY;
         }
         set
         {
            _lowerY = value;
         }
      }

      public int Width
      {
         get
         {
            return _width;
         }
         set
         {
            _width = value;
         }
      }

      public int Height
      {
         get
         {
            return _height;
         }
         set
         {
            _height = value;
         }
      }

      public int UpperRightX
      {
         get
         {
            return LowerX + Width - 1;
         }
      }

      public int UpperRightY
      {
         get
         {
            return LowerY + Height - 1;
         }
      }

      public override string ToString()
      {
         return string.Format("({0},{1})({2},{3})", LowerX, LowerY, UpperRightX, UpperRightY);
      }

      public override bool Equals(object obj)
      {
         if ((obj == null) || (obj.GetType() != GetType()))
         {
            return false;
         }

         LRectangle that = (LRectangle)obj;
         return this._lowerX == that._lowerX
            && this._lowerY == that._lowerY
            && this._width == that._width
            && this._height == that._height;
      }

      public override int GetHashCode()
      {
         return this._lowerX ^ this._lowerY ^ this._width ^ this._height;
      }

      public static bool operator ==(LRectangle left, LRectangle right)
      {
         return left.Equals(right);
      }

      public static bool operator !=(LRectangle left, LRectangle right)
      {
         return !left.Equals(right);
      }
   }
}