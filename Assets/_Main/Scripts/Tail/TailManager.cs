using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

namespace Scripts.Tail
{
    public class TailManager : MonoBehaviour
    {
        [SerializeField] private Transform tailHead;
        [SerializeField] private float spacing = 1f;
        [SerializeField] private float followSpeed = 5f;

        [Header("Gizmos")]
        [SerializeField] private Color gizmosColor = Color.green;
        [SerializeField] private Mesh previewItemMesh;
        [SerializeField] private Quaternion previewItemMeshRot;
        [SerializeField] private bool drawGizmos = true;

        private List<Transform> tailNode;
        private List<Vector3> velocities;


        private void Awake()
        {
            tailNode = new List<Transform>();
            velocities = new List<Vector3>();

            for (int i = 0; i < transform.childCount; i++)
            {
                tailNode.Add(transform.GetChild(i));
                velocities.Add(Vector3.zero);
            }

            foreach (var node in tailNode)
            {
                node.SetParent(null);
            }
        }

        private void FixedUpdate()
        {
            UpdateTail();
        }

        public int GetCapacity()
        {
            return tailNode.Count;
        }

        public void UpdateTail()
        {
            for (int i = 0; i < tailNode.Count; i++)
            {
                var targetTransform = i == 0 ? tailHead : tailNode[i - 1];

                var targetPos = targetTransform.position;
                targetPos.y += spacing;

                var velocity = velocities[i];

                tailNode[i].position = Vector3.SmoothDamp(
                    tailNode[i].position,
                    targetPos,
                    ref velocity,
                    1f / followSpeed
                );

                velocities[i] = velocity;

                tailNode[i].rotation = tailHead.rotation;
            }
        }

        public Transform GetTailNodeTransform(int index)
        {
            return tailNode[index];
        }
        
#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            if (previewItemMesh != null && drawGizmos && transform.childCount > 0)
            {
                Gizmos.color = gizmosColor;

                for (int i = 0; i < transform.childCount; i++)
                {
                    Vector3 pos = transform.position;
                    pos.y += i * spacing;
                    Gizmos.DrawMesh(previewItemMesh, pos, transform.rotation);
                }
            }
        }

        private void OnValidate()
        {
            if (Application.isPlaying) return;

            for (int i = 0; i < transform.childCount; i++)
            {
                var child = transform.GetChild(i);
                child.localPosition = new Vector3(0, i * spacing, 0);
            }
        }
#endif

    }
}