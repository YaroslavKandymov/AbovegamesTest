using UnityEngine;

namespace Project.UI.Views
{
    public class SplashScreenView : UIScreenView
    {
        public float ExistTime => _existTime;

        [SerializeField] private float _existTime;
    }
}