using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Drawing;

namespace ThoughtWorks
{
   public class Plateau : IPlateau
   {
      internal const string XLimitInvalidErrorText = "X limit cannot be <= 0.";
      internal const string YLimitInvalidErrorText = "Y Limit cannot be <= 0.";
      internal const string CellOccupiedErrorText = "An other rover is already at the specified cell.";
      internal const string CellOccupiedFmtSpec = "Rover '{0}' is already present at the specified cell.";
      internal const string NullCellSpecifiedErrorText = "Specified cell object is null.";
      internal const string CellOutsidePlateauFmtSpec = "The specified cell position ({0}, {1}) does not lie in the plateau boundary.";

      private LRectangle _bounds = LRectangle.Empty;
      private Cell[,] _cells = null;
      private List<IRover> _rovers = new List<IRover>();

      /// <summary>
      /// As per the problem statement, the lower-left coordinates 
      /// are assumed to be (0, 0). The upper-right co-ordinates
      /// are expected as width\height of the plateau (W: UX+1, H: UY+1).
      /// </summary>
      public Plateau(Size sz)
      {
         RoverAppException.ThrowIfTrue(sz.Width <= 0, Plateau.XLimitInvalidErrorText);
         RoverAppException.ThrowIfTrue(sz.Height <= 0, Plateau.YLimitInvalidErrorText);
         //Throw.IfTrue(sz.Width <= 0, Plateau.XLimitInvalidErrorText);
         //Throw.IfTrue(sz.Height <= 0, Plateau.YLimitInvalidErrorText);

         _bounds = new LRectangle(0, 0, sz.Width, sz.Height);
         _cells = new Cell[sz.Width, sz.Height];
      }

      #region IPlateau Members

      public LRectangle Bounds
      {
         get
         {
            return _bounds;
         }
      }

      public ICell GetCell(int x, int y)
      {
         RoverAppException.ThrowIfFalse(_bounds.IsPointInRect(x, y), Plateau.CellOutsidePlateauFmtSpec, x, y);

         Cell cellObj = _cells[x, y];
         if (cellObj == null)
         {
            cellObj = new Cell(x, y, this);
            _cells[x, y] = cellObj;
         }

         return cellObj;
      }

      public int NoOfRovers()
      {
         return _rovers.Count;
      }

      public IRover GetRoverAt(ICell iCell)
      {
         RoverAppException.ThrowIfNull(iCell, Plateau.NullCellSpecifiedErrorText);
         IRover iRvr = this._rovers.Find(r => r.Cell == iCell);
         return iRvr;
      }

      public IRover PlaceRover(int x, int y, CompassPoint direction)
      {
         RoverAppException.ThrowIfFalse(_bounds.IsPointInRect(x, y), Plateau.CellOutsidePlateauFmtSpec, x, y);

         IRover otherRover = null;
         if (GetCell(x, y).IsOccupied(out otherRover))
         {
            RoverAppException.Throw(Plateau.CellOccupiedFmtSpec, otherRover.Name);
         }

         IRover iRover = new Rover(GetCell(x, y), direction);
         _rovers.Add(iRover);

         return iRover;
      }

      #endregion
   }
}