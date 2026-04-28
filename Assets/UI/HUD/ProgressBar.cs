using UnityEngine;

namespace FoxShooter.UI.HUD
{
    public class ProgressBar : MonoBehaviour
    {
        [SerializeField] private RectTransform rect;

        public void SetPercentage(float percentage)
        {
            var scale = rect.localScale;
            scale.x = percentage;
            rect.localScale = scale;
        }
    }
}