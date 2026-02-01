using UnityEngine;

namespace Project.Services
{
    public readonly struct ImageDownloadResult
    {
        public readonly bool IsSuccess;
        public readonly Texture2D Texture;
        public readonly int Index;
        public readonly string Error;
        public readonly string Url;
    
        public static ImageDownloadResult Success(int index, string url, Texture2D texture) =>
            new ImageDownloadResult(true, index, url, texture, null);
    
        public static ImageDownloadResult Failure(int index, string url, string error) =>
            new ImageDownloadResult(false, index, url, null, error);
    
        private ImageDownloadResult(bool isSuccess, int index, string url, Texture2D texture, string error)
        {
            IsSuccess = isSuccess;
            Index = index;
            Url = url;
            Texture = texture;
            Error = error;
        }
    }
}