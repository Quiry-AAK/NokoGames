using Scripts.UI;
using UnityEngine;

namespace Scripts.Player
{
    public class CoinManager : MonoBehaviour
    {
        private CoinPanel coinPanel;
        private int currentCoinAmount;

        private void Awake()
        {
            coinPanel = FindFirstObjectByType<CoinPanel>();
            coinPanel.UpdateTxt(currentCoinAmount);
        }

        public void AddCoin(int addCoinAmount)
        {
            currentCoinAmount += addCoinAmount;
            coinPanel.UpdateTxt(currentCoinAmount);
        }

        public void RemoveCoin(int removeCoinAmount)
        {
            currentCoinAmount -= removeCoinAmount;
            coinPanel.UpdateTxt(currentCoinAmount);
        }

        public int GetCurrentCoinAmount()
        {
            return currentCoinAmount;
        }
    }
}