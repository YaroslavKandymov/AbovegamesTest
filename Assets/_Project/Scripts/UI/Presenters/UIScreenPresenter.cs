using System;
using Cysharp.Threading.Tasks;
using Project.UI.Views;
using R3;

namespace Project.UI.Presenters
{
    public abstract class UIScreenPresenter<T> : IDisposable where T : UIScreenView
    {
        protected readonly CompositeDisposable Disposables;
        protected T View { get; }

        protected UIScreenPresenter(UIHub hub)
        {
            View = hub.GetWindow<T>();
            View.Close();

            Disposables = new ();
        }

        void IDisposable.Dispose()
        {
            Disposables?.Dispose();
        }

        public abstract void Initialize();

        public virtual async UniTask Enable()
        {
            View.Open();

            await UniTask.CompletedTask;
        }

        public virtual async UniTask Disable()
        {
            View.Close();
            
            await UniTask.CompletedTask;
        }
    }
}