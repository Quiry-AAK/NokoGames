
using Scripts.ObjectPools;
using UnityEngine;

namespace Scripts.Building
{
    public class BuildingBlock : MonoBehaviour
    {
        [SerializeField] private PoolType itemType;
        [SerializeField] private MeshRenderer meshRenderer;
        [SerializeField] private Material disabledMat, enabledMat;

        public PoolType ItemType => itemType;

        public void EnableMe(bool enable)
        {
            meshRenderer.material = enable ? enabledMat : disabledMat;
        }
    }
}