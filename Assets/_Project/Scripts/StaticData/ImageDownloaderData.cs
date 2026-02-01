using UnityEngine;

namespace Project.StaticData
{
    [CreateAssetMenu(fileName = "new ImageDownloaderData", menuName = "StaticData/ImageDownloaderData")]
    public class ImageDownloaderData : ScriptableObject
    {
        public string BaseUrl => _baseUrl;
        public int StartIndex => _startIndex;
        public int EndIndex => _endIndex;
        public float DelayBetweenDownloads => _delayBetweenDownloads;

        [SerializeField] private string _baseUrl = "http://data.ikppbb.com/test-task-unity-data/pics/";
        [SerializeField] private int _startIndex = 1;
        [SerializeField] private int _endIndex = 66;
        [SerializeField] private float _delayBetweenDownloads = 0.1f;
    }
}