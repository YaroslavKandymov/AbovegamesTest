using Cysharp.Threading.Tasks;
using R3;
using UnityEngine;
using UnityEngine.UI;
using UnlimitedScrollUI;

namespace Project.UI.Views
{
    public class GalleryItemView : MonoBehaviour, ICell
    {
        public Observable<GalleryItemView> OnButtonClicked => _button.onClick.AsObservable().Select(_ => this);

        public bool IsPremium { get; private set; }

        [SerializeField] private Image _image;
        [SerializeField] private Button _button;
        [SerializeField] private GameObject _premiumBadge;
        
        private UniTaskCompletionSource<Sprite> _loadCompletionSource;

        public void Initialize(Sprite sprite)
        {
            _image.sprite = sprite;
            _loadCompletionSource?.TrySetResult(sprite);
        }
        
        public async UniTask<Sprite> GetSpriteAsync()
        {
            if (_image.sprite != null)
                return _image.sprite;
            
            _loadCompletionSource ??= new UniTaskCompletionSource<Sprite>();
        
            return await _loadCompletionSource.Task;
        }

        public void SetActivePremium(bool active)
        {
            _premiumBadge.SetActive(active);
            
            IsPremium = active;
        }

        public void OnBecomeVisible(ScrollerPanelSide side)
        {
        }

        public void OnBecomeInvisible(ScrollerPanelSide side)
        {
        }
    }
}