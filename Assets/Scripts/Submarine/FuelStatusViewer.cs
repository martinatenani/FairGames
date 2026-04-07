using TMPro;
using UnityEngine;

namespace Submarine
{
    public class FuelStatusViewer : MonoBehaviour
    {
        [SerializeField] TMP_Text statusText;
        [SerializeField] char barSymbol = '|';
        [SerializeField] string template = "[*]";
        [SerializeField] int maxBars = 10;

        private void Start()
        {
            if (!statusText) statusText = GetComponentInChildren<TMP_Text>();
        }

        public void SetFuel(float value)
        {
            if (value < 0)
            {
                statusText.text = null;
                return;
            }
        
            float clampedValue = Mathf.Clamp(value, 0f, 1f);
            int howManyBars = (int)(maxBars * clampedValue);
        
            string barsSymbols = new string(barSymbol, howManyBars);
            statusText.text = $"{template.Replace("*", barsSymbols)}";
        }
    }
}
