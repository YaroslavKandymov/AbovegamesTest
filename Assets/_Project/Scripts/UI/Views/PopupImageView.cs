using UnityEngine;
using UnityEngine.UI;

namespace Project.UI.Views
{
    public class PopupImageView : MonoBehaviour
    {
        [SerializeField] private Image _image;

        public void SetImage(Sprite sprite)
        {
            _image.sprite = sprite;
        }
    }
}