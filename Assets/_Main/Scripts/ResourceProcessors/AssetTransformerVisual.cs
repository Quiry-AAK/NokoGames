using DG.Tweening;
using UnityEngine;

namespace Scripts.ResourceProcessors
{
    public class AssetTransformerVisual : ResourceProcessorVisual
    {
        [SerializeField] private Animator spawnerAnimator;
        [SerializeField] private Transform spawnedObjectVisual;
        [SerializeField] private Vector3 spawnedObjectVisualEndPos;
        [SerializeField] private Vector3 endPos;
        [SerializeField] private ParticleSystem[] particles;

        [Header("Gizmos")]
        [SerializeField] private Color gizmoColor;
        [SerializeField] private float gizmoSize;

        private Vector3 initialPos;

        public Transform SpawnedObjectVisual => spawnedObjectVisual;
        public Vector3 EndPos => endPos;
        public Vector3 InitialPos => initialPos;

        private Tween moveTween;

        private void Awake()
        {
            initialPos = spawnedObjectVisual.transform.position;
        }

        public override void StartVisualizingSpawning(float spawnTime)
        {
            moveTween?.Kill();
            spawnedObjectVisual.transform.position = initialPos;
            spawnedObjectVisual.gameObject.SetActive(true);
            moveTween = spawnedObjectVisual.transform.DOLocalMoveZ(spawnedObjectVisualEndPos.z, spawnTime).SetEase(Ease.Linear);
            spawnerAnimator.SetBool("Processing", true);

            foreach (var particle in particles)
            {
                if (particle.isPlaying)
                    continue;
                particle.Play();
            }
        }

        public override void StopVisualizingSpawning()
        {
            moveTween?.Kill();
            spawnedObjectVisual.gameObject.SetActive(false);
            spawnerAnimator.SetBool("Processing", false);

            foreach (var particle in particles)
            {
                if (particle.isPlaying)
                    continue;
                particle.Stop();
            }
        }
#if UNITY_EDITOR
        void OnDrawGizmos()
        {
            Gizmos.color = gizmoColor;
            Gizmos.DrawWireSphere(transform.TransformPoint(endPos), gizmoSize);
            Gizmos.DrawWireSphere(transform.TransformPoint(spawnedObjectVisualEndPos), gizmoSize);
        }
#endif
    }
}