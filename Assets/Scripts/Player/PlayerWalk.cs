using System;
using System.Collections;
using Assets.Scripts.Managers;
using Assets.Scripts.Utilities;
using UnityEngine;

namespace Assets.Scripts.Player
{
    /// <summary>
    /// Script responsible for moving player from waypoint to waypoint until path end.
    /// </summary>
    public class PlayerWalk : MonoBehaviour
    {
        #region Constants

        private const string DestinationBeaconTagName = "DestinationBeacon";
        private const float BackToEditorDelay = 1f;

        #endregion

        #region Editor Variables

        public GameObject directionIndicator;
        public float speed;
        public float coordinatesModifier = 1f;
        public string animatorWalkingParamName;
        public string animatorVelocityXParamName;
        public string animatorVelocityYParamName;

        #endregion

        #region Variables

        private DestinationManager _destManager;
        private bool _moving;
        private Vector2 _velocity;
        private Rigidbody2D _rigidbody;
        private Animator _animator;

        #endregion

        #region Unity callbacks

        void Start()
        {
            _destManager = new DestinationManager(gameObject, directionIndicator, coordinatesModifier);

            _rigidbody = GetComponent<Rigidbody2D>();
            if (_rigidbody == null)
                throw new NullReferenceException("rigidbody is not set up");

            _animator = GetComponent<Animator>();
            if(_animator == null)
                throw new NullReferenceException("animator is not set up");

            if (_destManager.NextWaypointAvailable)
            {
                SetUpNextWaypoint();
                _moving = true;
                SetUpAnimatorParams();
            }
            else
            {
                Debug.LogWarning("No waypoints on path.");
            }
        }
        
        void Update()
        {
            // keeping constant velocity
            _rigidbody.velocity = _velocity;
        }

        void LateUpdate()
        {
            if (!_moving)
                StartDelayedBackToEditorCoroutine();
        }

        void OnTriggerEnter2D(Collider2D collider)
        {
            if (!collider.CompareTag(DestinationBeaconTagName))
                return;

            if (_destManager.NextWaypointAvailable)
            {
                SetUpNextWaypoint();
                _moving = true;
                SetUpAnimatorParams();
            }
            else
            {
                _moving = false;
                _velocity = Vector2.zero;
                SetUpAnimatorParams();
                Debug.Log("Path ended.");
            }
        }

        #endregion

        #region Coroutines

        private IEnumerator DelayedBackToEditorCoroutine(float seconds)
        {
            yield return new WaitForSeconds(seconds);
            GameManager.Instance.LoadLevel(this, "Scene1_Editor", null);
        }

        #endregion

        #region Private Methods

        private void SetUpNextWaypoint()
        {
            _destManager.NextWaypoint();
            _velocity = _destManager.TrailingVector * speed;
        }

        private void SetUpAnimatorParams()
        {
            _animator.SetBool(animatorWalkingParamName, _moving);
            _animator.SetFloat(animatorVelocityXParamName, _velocity.x);
            _animator.SetFloat(animatorVelocityYParamName, _velocity.y);
        }

        private void StartDelayedBackToEditorCoroutine()
        {
            StartCoroutine(DelayedBackToEditorCoroutine(BackToEditorDelay));
        }
        
        #endregion
    }
}
