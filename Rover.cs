using System;
using System.Diagnostics;
using System.Drawing;

namespace ThoughtWorks
{
   class Rover : IRover
   {
      internal const string CantMoveOutsidePlateauErrorText = "Attempt to move rover outside the plateau bounds.";
      private ICell _location = null;
      private CompassPoint _moveDirection = CompassPoint.North;

      internal Rover(ICell initLocation, CompassPoint cpt)
      {
         this.Name = "Rvr" + initLocation.Plateau.NoOfRovers().ToString();
         this._location = initLocation;
         this._moveDirection = cpt;

         Logger.TraceLine("Rover '{0}' created. Location: {1}. Direction: {2}", Name, initLocation, cpt);
      }

      #region IRover Members

      public string Name { get; private set; }

      public ICell Cell
      {
         get
         {
            return _location;
         }
         internal set
         {
            Debug.Assert(value != null);
            _location = value;
         }
      }
      
      public ICell Move()
      {
         Logger.TraceLine("Rover[{0}].Move - Cell: {1}. Direction: {2}.", Name, Cell, Direction);

         Point pt = Compass.GetNextPosition(_location.X, _location.Y, _moveDirection);
         RoverAppException.ThrowIfFalse(_location.Plateau.Bounds.IsPointInRect(pt), Rover.CantMoveOutsidePlateauErrorText);

         this.Cell = VerifyCellNotOccupied(pt.X,pt.Y);

         Logger.TraceLine("Rover[{0}].Moved to cell {1}.", Name, Cell);

         return _location;
      }

      public CompassPoint RotateLeft()
      {
         CompassPoint cpt = Compass.RotateLeft(ref _moveDirection);
         Logger.TraceLine("Rover[{0}].RotateLeft - Direction: {1}", Name, cpt);
         return cpt;
      }

      public CompassPoint RotateRight()
      {
         CompassPoint cpt = Compass.RotateRight(ref _moveDirection);
         Logger.TraceLine("Rover[{0}].RotateRight - Direction: {1}", Name, cpt);
         return cpt;
      }

      public CompassPoint Direction
      {
         get
         {
            return _moveDirection;
         }
      }

      #endregion

      public override string ToString()
      {
         return string.Format("[{0}, {1}, {2}]", this.Name, this.Cell, this.Direction);
      }

      private ICell VerifyCellNotOccupied(int x, int y)
      {
         ICell newCell = _location.Plateau.GetCell(x, y);
         IRover rvr = null;

         if (newCell.IsOccupied(out rvr))
         {
            Debug.Assert(rvr != null);
            RoverAppException.Throw("The destined location is already occupied by a Rover ({0})", rvr.Name);
         }

         return newCell;
      }
   }
}