using System;
using System.Diagnostics;

namespace ThoughtWorks
{
   class Cell : ICell
   {
      internal const string RoverNotFoundFmtSpec = "No rover found at the specified cell {0}";
      private IPlateau _plateau = null;

      internal Cell(int x, int y, Plateau p)
      {
         Debug.Assert(x >= 0 && y >= 0);
         Debug.Assert(p != null);

         this.X = x;
         this.Y = y;
         this._plateau = p;
      }

      #region ICell Members

      public int X { get; private set; }
      public int Y { get; private set; }

      public IPlateau Plateau
      {
         get
         {
            Debug.Assert(_plateau != null);
            return _plateau;
         }
      }

      public bool IsOccupied()
      {
         IRover iRvr = null;
         return IsOccupied(out iRvr);
      }

      public bool IsOccupied(out IRover iRvr)
      {
         iRvr = this._plateau.GetRoverAt(this);
         return (iRvr != null ? true : false);
      }

      /// <summary>
      /// This property must be used in conjunction with IsOccupied
      /// since an exception will be thrown if a rover is not
      /// stationed in this cell.
      /// </summary>
      public IRover Rover
      {
         get
         {
            IRover iRvr = null;
            RoverAppException.ThrowIfFalse(IsOccupied(out iRvr), RoverNotFoundFmtSpec, this);
            return iRvr;
         }
      }

      #endregion

      public override string ToString()
      {
         return string.Format("({0}, {1})", this.X, this.Y);
      }
   }
}