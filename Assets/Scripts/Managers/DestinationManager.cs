using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts.Utilities;
using UnityEngine;

namespace Assets.Scripts.Managers
{
    /// <summary>
    /// Class used as an iterator to waypoints stored in GameManager. Additionally
    /// provides helper methods for vectors calculation.
    /// </summary>
    public class DestinationManager
    {
        private readonly IList<RoutePoint> _routePoints;
        private readonly GameObject _navigatedObject;
        private readonly GameObject _directionIndicator;
        private readonly float _coordinatesModifier;

        private int _currentIndex = -1;

        public bool NextWaypointAvailable
        {
            get { return _currentIndex + 1 < _routePoints.Count; }
        }

        public Vector2 TrailingVector { get; private set; }

        public DestinationManager(GameObject navigatedGameObject,  GameObject directionIndicator = null, float coordinatesModifier = 1f)
        {
            if(navigatedGameObject == null)
                throw new ArgumentNullException("navigatedGameObject");

            _navigatedObject = navigatedGameObject;
            _directionIndicator = directionIndicator;
            _coordinatesModifier = coordinatesModifier;
            _routePoints = GameManager.Instance.RoutePoints;
        }

        public void NextWaypoint()
        {
            ++_currentIndex;
            Vector2 nextPoint = (Vector3)_routePoints[_currentIndex];
            nextPoint *= _coordinatesModifier;

            if(_directionIndicator != null)
                MoveDirectionIndicator(nextPoint);

            TrailingVector = (nextPoint - (Vector2) _navigatedObject.transform.localPosition).normalized;
        }

        private void MoveDirectionIndicator(Vector2 newCoords)
        {
            _directionIndicator.transform.localPosition = newCoords;
        }
    }
}
