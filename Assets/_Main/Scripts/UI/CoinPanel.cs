using TMPro;
using UnityEngine;

namespace Scripts.UI
{
    public class CoinPanel : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI coinTxt;

        public void UpdateTxt(int coinAmount)
        {
            coinTxt.text = coinAmount.ToString();
        }
    }
}