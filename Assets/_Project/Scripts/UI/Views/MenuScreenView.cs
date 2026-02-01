using System.Collections.Generic;
using System.Linq;
using Project.Common;
using Project.Enums;
using R3;
using UnityEngine;
using UnityEngine.UI;
using UnlimitedScrollUI;

namespace Project.UI.Views
{
    public class MenuScreenView : UIScreenView
    {
        public IReadOnlyCollection<TabBarButtonView> Views => _tabBarButtonViews;
        public IUnlimitedScroller UnlimitedScroller => _unlimitedScrollUI;
        public Observable<Unit> PopupCloseButtonClicked => _popupCloseButton.onClick.AsObservable();
        public PopupImageView PopupImageView => _popupImageView;
        public IReadOnlyList<RectTransform > Banners => _banners;
        public SwipeDetector SwipeDetector => _swipeDetector;

        [SerializeField] private GridUnlimitedScroller _unlimitedScrollUI;
        [SerializeField] private Sprite _activePoint;
        [SerializeField] private Sprite _inactivePoint;
        [SerializeField] private Button _popupCloseButton;
        [SerializeField] private PopupImageView _popupImageView;
        [SerializeField] private SwipeDetector _swipeDetector;
        [SerializeField] private TabBarButtonView[] _tabBarButtonViews;
        [SerializeField] private Image[] _bannerPoints;
        [SerializeField] private RectTransform [] _banners;

        public void UpdateGridLayout(int count, Vector2 size)
        {
            _unlimitedScrollUI.constraintCount = count;
            _unlimitedScrollUI.cellPerRow = count;
            _unlimitedScrollUI.cellSize = size;
        }
        
        public TabBarButtonView GetView(TabBarType tabBarType)
        {
            return _tabBarButtonViews.FirstOrDefault(t => t.Type == tabBarType);
        }
        
        public void SetBannersPointsInactive()
        {
            foreach (var image in _bannerPoints)
            {
                image.sprite = _inactivePoint;
            }
        }

        public void SetBannersPointsActive(int activeIndex)
        {
            for (var index = 0; index < _bannerPoints.Length; index++)
            {
                _bannerPoints[index].sprite = index == activeIndex ? _activePoint : _inactivePoint;
            }
        }
    }
}