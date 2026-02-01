using Project.UI.Views;
using UnityEngine;

namespace Project.UI
{
    public class UIHub : MonoBehaviour
    {
        [SerializeField] private UIScreenView[] _windows;
        
        public T GetWindow<T>() where T : UIScreenView
        {
            for (int i = 0, count = _windows.Length; i < count; i++)
            {
                if (_windows[i] is T result)
                {
                    return result;
                }
            }

            return null;
        }
    }
}