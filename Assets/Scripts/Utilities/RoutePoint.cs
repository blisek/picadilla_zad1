using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Utilities
{
    public struct RoutePoint
    {
        public RoutePoint(int id, float x = 0f, float y = 0f) : this()
        {
            Id = id;
            X = x;
            Y = y;
        }

        public int Id { get; private set; }
        public float X { get; set; }
        public float Y { get; set; }


        public bool InRange(Vector2 point, float xDeviation = 0f, float yDeviation = 0f)
        {
            bool inXAxisAllowedDeviation = Mathf.Abs(X - point.x) <= xDeviation;
            bool inYAxisAllowedDeviation = Mathf.Abs(Y - point.y) <= yDeviation;
            return inXAxisAllowedDeviation && inYAxisAllowedDeviation;
        }

        public static explicit operator Vector3(RoutePoint routePoint)
        {
            return new Vector3(routePoint.X, routePoint.Y);
        }
    }

    public class RoutePointComparer : IComparer<RoutePoint>
    {
        public int Compare(RoutePoint x, RoutePoint y)
        {
            return x.Id - y.Id;
        }
    }
}
