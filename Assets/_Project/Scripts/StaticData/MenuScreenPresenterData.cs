using DG.Tweening;
using Project.Enums;
using Project.UI.Views;
using UnityEngine;

namespace Project.StaticData
{
    [CreateAssetMenu(fileName = "new MenuScreenPresenterData", menuName = "StaticData/MenuScreenPresenterData")]
    public class MenuScreenPresenterData : ScriptableObject
    {
        public GalleryItemView GalleryItemView => _galleryItemView;
        public Color ButtonsColor => _buttonsColor;
        public TabBarType StartType => _startType;
        public float AutoTimer => _autoTimer;
        public int PremiumNumbers => _premiumNumbers;
        public float BannersOffset => _bannersOffset;
        public float BannersMoveTime => _bannersMoveTime;
        public int PhoneColumns => _phoneColumns;
        public int TabletColumns => _tabletColumns;
        public Vector2 PhoneSize => _phoneSize;
        public Vector2 TabletSize => _tabletSize;
        public float PopupScaleTime => _popupScaleTime;
        public AnimationCurve PopupScaleEase => _popupScaleEase;
        public Ease BannersEase => _bannersEase;

        [SerializeField] private GalleryItemView _galleryItemView;
        [SerializeField] private Color _buttonsColor;
        [SerializeField] private TabBarType _startType;
        [SerializeField] private float _autoTimer;
        [SerializeField] private int _premiumNumbers;
        [SerializeField] private float _bannersOffset;
        [SerializeField] private float _bannersMoveTime;
        [SerializeField] private int _phoneColumns = 2;
        [SerializeField] private int _tabletColumns = 3;
        [SerializeField] private Vector2 _phoneSize;
        [SerializeField] private Vector2 _tabletSize;
        [SerializeField] private float _popupScaleTime;
        [SerializeField] private AnimationCurve _popupScaleEase;
        [SerializeField] private Ease _bannersEase;
    }
}