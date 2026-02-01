using DG.Tweening;
using UnityEngine;

namespace Project.StaticData
{
    [CreateAssetMenu(fileName = "new PremiumScreenPresenterData", menuName = "StaticData/PremiumScreenPresenterData")]
    public class PremiumScreenPresenterData : ScriptableObject
    {
        public float Overshoot => _overshoot;
        public float OvershootDuration => _overshootDuration;
        public float OpenDuration => _openDuration;
        public float CloseDuration => _closeDuration;
        public Ease OvershootEase => _overshootEase;
        public Ease OpenEase => _openEase;
        public Ease CloseEase => _closeEase;

        [SerializeField] private float _overshoot = 1.1f;
        [SerializeField] private float _overshootDuration = 0.1f;
        [SerializeField] private float _openDuration = 0.25f;
        [SerializeField] private float _closeDuration = 0.3f;
        [SerializeField] private Ease _overshootEase;
        [SerializeField] private Ease _openEase;
        [SerializeField] private Ease _closeEase;
    }
}