using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Project.Enums;
using Project.Services;
using Project.StaticData;
using Project.UI.Views;
using R3;
using UnityEngine;
using UnlimitedScrollUI;
using DeviceType = Project.Enums.DeviceType;

namespace Project.UI.Presenters
{
    public class MenuScreenPresenter : UIScreenPresenter<MenuScreenView>
    {
        public Observable<Unit> PopupOpened => _popupOpened;
        public Observable<Unit> PremiumOpened => _premiumOpened;

        private readonly PremiumScreenPresenter _premiumScreenPresenter;
        private readonly MenuScreenPresenterData _data;
        private readonly ImageDownloaderService _imageDownloaderService;
        private readonly DeviceChecker _deviceChecker;

        private TabBarButtonView _currentTabBarButtonView;
        private CancellationTokenSource _cancellationTokenSource;
        private List<int> _filteredIndices = new();
        private int _currentBannerIndex;
        private bool _isMoving;

        private readonly Subject<Unit> _popupOpened = new();
        private readonly Subject<Unit> _premiumOpened = new();

        public MenuScreenPresenter(
            UIHub hub,
            PremiumScreenPresenter premiumScreenPresenter,
            MenuScreenPresenterData data,
            ImageDownloaderService imageDownloaderService,
            DeviceChecker deviceChecker) : base(hub)
        {
            _premiumScreenPresenter = premiumScreenPresenter;
            _data = data;
            _imageDownloaderService = imageDownloaderService;
            _deviceChecker = deviceChecker;
        }

        public override void Initialize()
        {
            View.PopupImageView.transform.localScale = Vector3.zero;

            foreach (var button in View.Views)
            {
                button.ButtonClicked
                    .Subscribe(OnButtonClicked)
                    .AddTo(Disposables);
            }

            View.PopupCloseButtonClicked
                .Subscribe(OnPopupCloseButtonClicked)
                .AddTo(Disposables);

            View.SwipeDetector.SwipeRight
                .Subscribe(OnSwipeRight)
                .AddTo(Disposables);

            View.SwipeDetector.SwipeLeft
                .Subscribe(OnSwipeLeft)
                .AddTo(Disposables);

            var device = _deviceChecker.CheckDevice();
         
            View.UpdateGridLayout(
                device == DeviceType.Phone ? _data.PhoneColumns : _data.TabletColumns,
                device == DeviceType.Phone ? _data.PhoneSize : _data.TabletSize);

            ActivateCurrentTabBarButtonView(_data.StartType);
            View.UnlimitedScroller.Generate(_data.GalleryItemView.gameObject, _imageDownloaderService.TotalImagesCount,
                OnGenerate);
        }

        public override async UniTask Enable()
        {
            await base.Enable();

            StartAutomaticMoveBanners();
        }

        public override async UniTask Disable()
        {
            await base.Disable();

            CancelMove();
        }

        private void CancelMove()
        {
            _cancellationTokenSource?.Cancel();
        }

        private void OnButtonClicked(TabBarType tabBarType)
        {
            if (_currentTabBarButtonView.Type == tabBarType)
                return;

            _currentTabBarButtonView.SetActiveLine(false);
            _currentTabBarButtonView.SetTextColor(Color.black);

            ActivateCurrentTabBarButtonView(tabBarType);
        }

        private void ActivateCurrentTabBarButtonView(TabBarType tabBarType)
        {
            _currentTabBarButtonView = View.GetView(tabBarType);
            _currentTabBarButtonView.SetActiveLine(true);
            _currentTabBarButtonView.SetTextColor(_data.ButtonsColor);
            ApplyFilter(tabBarType);
        }

        private void ApplyFilter(TabBarType filter)
        {
            _filteredIndices = GetFilteredIndices(filter);

            RegenerateScroller();
        }

        private void RegenerateScroller()
        {
            View.UnlimitedScroller.Clear();

            View.UnlimitedScroller.Generate(
                _data.GalleryItemView.gameObject,
                _filteredIndices.Count,
                OnGenerate
            );
        }

        private void StartAutomaticMoveBanners()
        {
            _cancellationTokenSource = new();

            Observable.Interval(TimeSpan.FromSeconds(_data.AutoTimer))
                .Subscribe(_ => MoveBannerLeft().Forget())
                .AddTo(_cancellationTokenSource.Token);
        }

        private async UniTask MoveBannersRight()
        {
            _isMoving = true;

            View.Banners[_currentBannerIndex]
                .DOAnchorPosX(_data.BannersOffset, _data.BannersMoveTime)
                .SetEase(_data.BannersEase);

            View.SetBannersPointsInactive();

            _currentBannerIndex--;

            if (_currentBannerIndex < 0)
            {
                _currentBannerIndex = View.Banners.Count - 1;
            }

            View.Banners[_currentBannerIndex].anchoredPosition = new Vector2(
                -_data.BannersOffset,
                View.Banners[_currentBannerIndex].anchoredPosition.y);

            await View.Banners[_currentBannerIndex]
                .DOAnchorPosX(0, _data.BannersMoveTime)
                .SetEase(_data.BannersEase);

            View.SetBannersPointsActive(_currentBannerIndex);

            _isMoving = false;
        }

        private async UniTask MoveBannerLeft()
        {
            _isMoving = true;

            View.Banners[_currentBannerIndex]
                .DOAnchorPosX(-_data.BannersOffset, _data.BannersMoveTime)
                .SetEase(_data.BannersEase);

            View.SetBannersPointsInactive();

            _currentBannerIndex++;

            if (_currentBannerIndex >= View.Banners.Count)
            {
                _currentBannerIndex = 0;
            }

            View.Banners[_currentBannerIndex].anchoredPosition = new Vector2(
                _data.BannersOffset,
                View.Banners[_currentBannerIndex].anchoredPosition.y);

            await View.Banners[_currentBannerIndex]
                .DOAnchorPosX(0, _data.BannersMoveTime)
                .SetEase(_data.BannersEase);

            View.SetBannersPointsActive(_currentBannerIndex);

            _isMoving = false;
        }

        private void OnGenerate(int index, ICell cell)
        {
            if (cell is GalleryItemView view)
            {
                int realImageIndex = _filteredIndices[index];
                InitializeImage(realImageIndex, view).Forget();

                view.SetActivePremium((index + 1) % _data.PremiumNumbers == 0);
                SubscribeToClick(view, view.IsPremium);
            }
        }

        private async UniTaskVoid InitializeImage(int index, GalleryItemView view)
        {
            var result = await _imageDownloaderService.LoadImageAsync(index);

            if (result.IsSuccess)
            {
                var sprite = TextureToSprite(result.Texture);

                view.Initialize(sprite);
            }
        }

        private Sprite TextureToSprite(Texture2D texture)
        {
            if (texture == null) return null;

            return Sprite.Create(
                texture,
                new Rect(0, 0, texture.width, texture.height),
                new Vector2(0.5f, 0.5f),
                100f
            );
        }

        private List<int> GetFilteredIndices(TabBarType filter)
        {
            var indices = new List<int>();
            int totalImages = _imageDownloaderService.TotalImagesCount + 1;

            for (int i = 0; i < totalImages; i++)
            {
                bool shouldInclude = false;

                switch (filter)
                {
                    case TabBarType.All:
                        shouldInclude = true;
                        break;

                    case TabBarType.Even:
                        shouldInclude = (i + 1) % 2 == 0;
                        break;

                    case TabBarType.Odd:
                        shouldInclude = (i + 1) % 2 == 1;
                        break;
                }

                if (shouldInclude)
                {
                    indices.Add(i);
                }
            }

            return indices;
        }

        private void SubscribeToClick(GalleryItemView view, bool isPremium)
        {
            view.OnButtonClicked
                .Subscribe(_ => HandleImageClick(view, isPremium))
                .AddTo(Disposables);
        }

        private void HandleImageClick(GalleryItemView view, bool isPremium)
        {
            SetSpriteInPopup(view).Forget();

            if (isPremium)
            {
                OpenPremium().Forget();

                _premiumOpened?.OnNext(Unit.Default);
            }
            else
            {
                OpenPopup();

                _popupOpened?.OnNext(Unit.Default);
            }
        }

        private async UniTaskVoid SetSpriteInPopup(GalleryItemView view)
        {
            var sprite = await view.GetSpriteAsync();

            View.PopupImageView.SetImage(sprite);
        }

        private void OpenPopup()
        {
            View.PopupImageView.transform.DOScale(1, _data.PopupScaleTime).SetEase(_data.PopupScaleEase);
        }

        private void OnPopupCloseButtonClicked(Unit _)
        {
            View.PopupImageView.transform.DOScale(0, _data.PopupScaleTime).SetEase(_data.PopupScaleEase);
        }

        private async UniTaskVoid OpenPremium()
        {
            await _premiumScreenPresenter.Enable();

            View.PopupImageView.transform.localScale = Vector3.one;
        }

        private void OnSwipeRight(Unit _)
        {
            MoveBanners(MoveBannersRight).Forget();
        }

        private void OnSwipeLeft(Unit _)
        {
            MoveBanners(MoveBannerLeft).Forget();
        }

        private async UniTaskVoid MoveBanners(Func<UniTask> action)
        {
            if (_isMoving)
                return;

            CancelMove();

            await action();

            StartAutomaticMoveBanners();
        }
    }
}