using UnityEngine;

namespace Project.Services
{
    public class SoundCollection : MonoBehaviour
    {
        public AudioSource ClickSound => _clickSound;
        public AudioSource OpenImageSound => _openImageSound;
        public AudioSource PremiumSound => _premiumSound;

        [SerializeField] private AudioSource _clickSound;
        [SerializeField] private AudioSource _openImageSound;
        [SerializeField] private AudioSource _premiumSound;
    }
}