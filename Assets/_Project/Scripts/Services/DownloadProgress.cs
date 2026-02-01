namespace Project.Services
{
    public readonly struct DownloadProgress
    {
        public int LoadedCount { get; }
        public int TotalCount { get; }

        public DownloadProgress(int loaded, int total)
        {
            LoadedCount = loaded;
            TotalCount = total;
        }
    }
}