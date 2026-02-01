using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using Project.StaticData;
using R3;
using UnityEngine.Networking;

namespace Project.Services
{
    public class ImageDownloaderService : IDisposable
    {
        public int TotalImagesCount => _config.EndIndex - _config.StartIndex + 1;

        private readonly Dictionary<int, UniTaskCompletionSource<ImageDownloadResult>> _pendingRequests = new();
        private readonly ImageDownloaderData _config;
        private readonly CancellationTokenSource _cancellationTokenSource;
        private readonly Dictionary<int, ImageDownloadResult> _cachedResults = new();

        private readonly Subject<ImageDownloadResult> _onImageDownloaded = new();
        private readonly Subject<DownloadProgress> _onDownloadProgress = new();
        private readonly ReactiveProperty<bool> _isInitialized = new(false);


        public ImageDownloaderService(ImageDownloaderData config)
        {
            _config = config;
            _cancellationTokenSource = new CancellationTokenSource();
        }

        void IDisposable.Dispose()
        {
            _cancellationTokenSource.Cancel();
            _cancellationTokenSource?.Dispose();

            _onImageDownloaded.OnCompleted();
            _onImageDownloaded.Dispose();

            _onDownloadProgress.OnCompleted();
            _onDownloadProgress.Dispose();

            ClearCache();
            _isInitialized.Dispose();
        }

        public async UniTask<ImageDownloadResult> LoadImageAsync(int index)
        {
            index++;
            
            if (!IsValidIndex(index))
            {
                return ImageDownloadResult.Failure(index, string.Empty, $"Invalid index: {index}");
            }

            if (_cachedResults.TryGetValue(index, out var cachedResult))
            {
                return cachedResult;
            }

            return await TryLoadImageAsync(index, false);
        }

        private async UniTask<ImageDownloadResult> TryLoadImageAsync(int index, bool isPreload)
        {
            if (_pendingRequests.TryGetValue(index, out var request))
            {
                return await request.Task;
            }

            var url = $"{_config.BaseUrl}{index}.jpg";
            var completionSource = new UniTaskCompletionSource<ImageDownloadResult>();
            _pendingRequests[index] = completionSource;

            try
            {
                var result = await DownloadSingleImageAsync(index, url, _cancellationTokenSource.Token);

                if (result.IsSuccess)
                {
                    _cachedResults[index] = result;

                    if (!isPreload)
                    {
                        _onImageDownloaded.OnNext(result);
                        UpdateProgress();
                    }
                }

                completionSource.TrySetResult(result);
                return result;
            }
            finally
            {
                _pendingRequests.Remove(index);
            }
        }

        private async UniTask<ImageDownloadResult> DownloadSingleImageAsync(int index, string url, CancellationToken ct)
        {
            using var request = UnityWebRequestTexture.GetTexture(url);

            var (isCanceled, _) = await request.SendWebRequest()
                .WithCancellation(ct)
                .SuppressCancellationThrow();

            if (isCanceled)
            {
                return ImageDownloadResult.Failure(index, url, "Cancelled");
            }

            if (request.result != UnityWebRequest.Result.Success)
            {
                return ImageDownloadResult.Failure(index, url, request.error);
            }

            var texture = DownloadHandlerTexture.GetContent(request);
            return ImageDownloadResult.Success(index, url, texture);
        }

        private void CancelAllDownloads()
        {
            _cancellationTokenSource.Cancel();
            _pendingRequests.Clear();
        }

        private void ClearCache()
        {
            CancelAllDownloads();

            foreach (var result in _cachedResults.Values)
            {
                if (result.IsSuccess && result.Texture != null)
                {
                    UnityEngine.Object.Destroy(result.Texture);
                }
            }

            _cachedResults.Clear();
            _onDownloadProgress.OnNext(new DownloadProgress(0, TotalImagesCount));
        }

        private void UpdateProgress()
        {
            var progress = new DownloadProgress(_cachedResults.Count, TotalImagesCount);
            _onDownloadProgress.OnNext(progress);
        }

        private bool IsValidIndex(int index)
        {
            return index >= _config.StartIndex && index <= _config.EndIndex;
        }
    }
}