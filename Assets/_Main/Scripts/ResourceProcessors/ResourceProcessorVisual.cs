using UnityEngine;

namespace Scripts.ResourceProcessors
{
    public abstract class ResourceProcessorVisual : MonoBehaviour
    {
        public abstract void StartVisualizingSpawning(float spawnTime);
        public abstract void StopVisualizingSpawning();
    }
}