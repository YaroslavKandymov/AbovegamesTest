using Project.Extensions;
using UnityEngine;

namespace Project.UI.Views
{
    public class UIScreenView : MonoBehaviour
    {
        [SerializeField] private CanvasGroup _canvasGroup;
        
        public void Open()
        {
            _canvasGroup.Open();
        }

        public void Close()
        {
            _canvasGroup.Close();
        }
    }
}