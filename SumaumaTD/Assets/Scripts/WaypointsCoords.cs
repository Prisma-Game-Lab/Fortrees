using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts
{
    public class WaypointsCoords
    {
        public List<List<Coords>> Coordinates;
        private int _lastColumn =-1;
        private int _columnCount = 0;

        public WaypointsCoords()
        {
            Coordinates= new List<List<Coords>>();
        }

        public void AddCoordinateToColumn(float x, float z, int column, bool direction)
        {
            if (column != _lastColumn)
            {
                Coordinates.Add(new List<Coords> {new Coords(x, z, direction)});
                _lastColumn = column;
                _columnCount++;
            }
            else
            {
                Coordinates[_columnCount -1].Add(new Coords(x, z, direction));
            }
        }

        public struct Coords
        {
            public float X;
            public float Z;
            public bool Right;

            public Coords(float x, float z, bool right)
            {
                X = x;
                Z = z;
                Right = right;
            }
        }

    }
}
