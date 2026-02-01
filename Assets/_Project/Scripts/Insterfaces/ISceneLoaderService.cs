using Cysharp.Threading.Tasks;
using Project.Enums;

namespace Project.Insterfaces
{
    public interface ISceneLoaderService
    {
        public UniTask LoadScene(CurrentScene scene);
    }
}