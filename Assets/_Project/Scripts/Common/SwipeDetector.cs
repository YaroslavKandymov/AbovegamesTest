using R3;
using UnityEngine;

namespace Project.Common
{
    public class SwipeDetector : MonoBehaviour
    {
        public Observable<Unit> SwipeLeft => _swipeLeftSubject;
        public Observable<Unit> SwipeRight => _swipeRightSubject;
        
        [SerializeField] private RectTransform _swipeArea;
        [SerializeField] private float _swipeThreshold = 50f;
        
        private readonly Subject<Unit> _swipeLeftSubject = new();
        private readonly Subject<Unit> _swipeRightSubject = new();

        private Vector2 _touchStartPosition;
        private Vector2 _touchEndPosition;
        private bool _isSwiping;

        private void Start()
        {
            SetupSwipeDetection();
        }

        private void SetupSwipeDetection()
        {
            Observable
                .EveryUpdate()
                .Subscribe(_ => ProcessSwipes())
                .AddTo(this);
        }

        private void ProcessSwipes()
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (IsPointInSwipeArea(Input.mousePosition))
                {
                    _touchStartPosition = Input.mousePosition;
                    _isSwiping = true;
                }
            }
            else if (Input.GetMouseButtonUp(0) && _isSwiping)
            {
                if (IsPointInSwipeArea(Input.mousePosition))
                {
                    _touchEndPosition = Input.mousePosition;
                    ProcessSwipe();
                }

                _isSwiping = false;
            }
        }

        private void ProcessSwipe()
        {
            Vector2 swipeDelta = _touchEndPosition - _touchStartPosition;
            
            if (swipeDelta.magnitude < _swipeThreshold)
                return;
            
            if (Mathf.Abs(swipeDelta.x) > Mathf.Abs(swipeDelta.y))
            {
                if (swipeDelta.x > 0)
                {
                    _swipeRightSubject.OnNext(Unit.Default);
                }
                else
                {
                    _swipeLeftSubject.OnNext(Unit.Default);
                }
            }
        }

        private bool IsPointInSwipeArea(Vector2 screenPoint)
        {
            if (_swipeArea == null)
                return true;

            return RectTransformUtility.RectangleContainsScreenPoint(
                _swipeArea,
                screenPoint,
                null
            );
        }
    }
}