
using DG.Tweening;
using UnityEngine;

namespace Scripts.Buy
{
    public class HireAI : MonoBehaviour
    {
        [SerializeField] private Transform buyArea;
        [SerializeField] private GameObject aiPrefab;
        [SerializeField] private float animationDuration;
        [SerializeField] private GameObject spawnParticle;

        public void SpawnAI()
        {
            spawnParticle.gameObject.SetActive(true);
            buyArea.DOScale(0f, animationDuration).SetEase(Ease.InBack).OnComplete(() =>
            {
                Instantiate(aiPrefab, transform.position, transform.rotation);
                Destroy(gameObject, 1f);
            });
        }
    }
}