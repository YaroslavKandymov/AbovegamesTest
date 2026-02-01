using UnityEngine;

namespace Project.Extensions
{
    public static class CanvasGroupExtensions
    {
        public static void Open(this CanvasGroup canvasGroup)
        {
            canvasGroup.alpha = 1;
            canvasGroup.interactable = true;
            canvasGroup.blocksRaycasts = true;
        }
    
        public static void Close(this CanvasGroup canvasGroup)
        {
            canvasGroup.alpha = 0;
            canvasGroup.interactable = false;
            canvasGroup.blocksRaycasts = false;
        }
        
        public static void Set(this CanvasGroup canvasGroup, float alpha)
        {
            canvasGroup.alpha = alpha;
        }
    }
}