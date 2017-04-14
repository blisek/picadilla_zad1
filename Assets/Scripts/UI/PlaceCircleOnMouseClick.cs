using System;
using System.Collections.Generic;
using Assets.Scripts.Managers;
using Assets.Scripts.Utilities;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Assets.Scripts.UI
{
    public class PlaceCircleOnMouseClick : MonoBehaviour, IPointerDownHandler
    {
        #region Constants

        private const int PointsListInitialSize = 32;

        #endregion

        #region Variables

        public Canvas canvas;
        public GameObject pointObjectTemplate;
        public string clonedObjectNamePrefix;

        private int uniqueCounter = 0;

        /// <summary>
        /// Game window rect. Has convenient method used for checking if mouse pointer is in game window boundries.
        /// </summary>
        private Rect _windowRect;

        #endregion

        #region Unity callbacks
        

        void Start()
        {
            var routePoints = GameManager.Instance.RoutePoints;
            if (routePoints.Count > 0)
            {
                foreach (var routePoint in routePoints)
                {
                    var clonedObject = MakeNewClone(routePoint.Id);
                    clonedObject.transform.localPosition = Vector3.zero;
                    clonedObject.transform.SetParent(canvas.transform);
                    var rectTransform = clonedObject.GetComponent<RectTransform>();
                    rectTransform.position = (Vector3)routePoint;
                }
            }
            
            _windowRect = new Rect(0f, 0f, Screen.width, Screen.height);
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            var currentMousePosition = eventData.position;
            if (!_windowRect.Contains(currentMousePosition, allowInverse: false))
                return;

            var newId = uniqueCounter++;
            var clonedObject = MakeNewClone(newId);
            clonedObject.transform.localPosition = Vector3.zero;
            clonedObject.transform.SetParent(canvas.transform);
            var rectTransform = clonedObject.GetComponent<RectTransform>();
            var shiftedMousePosition = currentMousePosition -
                                       new Vector2(rectTransform.rect.width, 0) / 2f;
            rectTransform.position = shiftedMousePosition;
            GameManager.Instance.RoutePoints.Add(new RoutePoint(newId, shiftedMousePosition.x, shiftedMousePosition.y));
        }

        #endregion

        #region Private methods

        private GameObject MakeNewClone(int id)
        {
            var circle = Instantiate(pointObjectTemplate);
            circle.name = string.Format("{0}_{1}", clonedObjectNamePrefix, id);
            return circle;
        }

        #endregion
    }
}
