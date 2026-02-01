using System;
using Project.UI.Presenters;
using R3;
using UnityEngine;

namespace Project.Services
{
    public class SoundService : IDisposable
    {
        private readonly CompositeDisposable _disposables = new();
        private readonly SoundCollection _soundCollection;
        private readonly MenuScreenPresenter _menuScreenPresenter;

        public SoundService(SoundCollection soundCollection, MenuScreenPresenter menuScreenPresenter)
        {
            _soundCollection = soundCollection;
            _menuScreenPresenter = menuScreenPresenter;
        }

        void IDisposable.Dispose()
        {
            _disposables?.Dispose();
        }

        public void Initialize()
        {
            Observable
                .EveryUpdate()
                .Where(_ => Input.GetMouseButtonDown(0))
                .Subscribe(_ => PlayClickSound())
                .AddTo(_disposables);
            
            _menuScreenPresenter.PopupOpened
                .Subscribe(_ => PlayPopupSound())
                .AddTo(_disposables);
            
            _menuScreenPresenter.PremiumOpened
                .Subscribe(_ => PlayPremiumSound())
                .AddTo(_disposables);
        }

        private void PlayClickSound()
        {
            _soundCollection.ClickSound.Play();
        }

        private void PlayPopupSound()
        {
            _soundCollection.OpenImageSound.Play();
        }
        
        private void PlayPremiumSound()
        {
            _soundCollection.PremiumSound.Play();
        }
    }
}