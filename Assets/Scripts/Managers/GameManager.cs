using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Assets.Scripts.Utilities;
using UnityEngine;
using UnityEngine.SceneManagement;
using Object = UnityEngine.Object;

namespace Assets.Scripts.Managers
{
    public class GameManager : Singleton<GameManager>
    {
        #region Delegates

        public delegate bool OnSceneNearlyLoaded(AsyncOperation loadOperation);

        #endregion

        #region Constants

        private const int ProgressOver90ProcentCounterMinValue = 10;
        // zabieg optymalizacyjny
        private const int RoutePointsListInitialSize = 32;

        #endregion

        #region Variables

        private readonly List<RoutePoint> _routePoints;
        private readonly IComparer<RoutePoint> _routePointComparer;

        private int _uniqueCounter;

        #endregion

        #region Properties

        public IList<RoutePoint> RoutePoints
        {
            get { return _routePoints; }
        }

        #endregion

        #region Constructors

        public GameManager()
        {
            _routePoints = new List<RoutePoint>(RoutePointsListInitialSize);
            _routePointComparer = new RoutePointComparer();

            _uniqueCounter = 0;
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Generates unique id. Ids uniqueness is guaranted if method is called less or equal than int.MaxValue (2147483647).
        /// </summary>
        /// <returns>New id.</returns>
        public int GenerateUniqueId()
        {
            return _uniqueCounter++;
        }

        public Coroutine LoadLevel(MonoBehaviour caller, string levelName, Action<AsyncOperation> callback)
        {
            var asyncOp = SceneManager.LoadSceneAsync(levelName, LoadSceneMode.Single);
            asyncOp.allowSceneActivation = (callback == null);
            return caller.StartCoroutine(AsyncSceneLoaderHelper.SupervisedLevelLoadCoroutine(asyncOp, callback, ProgressOver90ProcentCounterMinValue));
        }

        /// <summary>
        /// Remove specified point from created root. Sorting isn't required because
        /// variable _uniqueCounter generates successive numbers in natural order, so
        /// they are ordered out of the box.
        /// </summary>
        /// <param name="id">Candidate for removal id. If id don't exist, nothing happens.</param>
        public void RemoveRoutePoint(int id)
        {
            var index = _routePoints.BinarySearch(new RoutePoint(id), _routePointComparer);
            if (index >= 0)
                _routePoints.RemoveAt(index);
        }

        #endregion
    }
}
