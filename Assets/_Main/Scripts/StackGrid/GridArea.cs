using UnityEngine;

namespace Scripts.StackGrid
{
    public class GridArea : MonoBehaviour
    {
        [Header("Area")]
        public Vector3 areaSize;

        [Header("Grid")]
        public int gridX;
        public int gridZ = 5;
        public int maxLayers = 3;

        [Header("Gizmo")]
        public Color areaColor;
        public Color gridColor;
        public Color filledSlotColor;
        public bool showGrid;
        public bool showSlots;
        public Mesh previewMesh;
        public Quaternion previewMeshRotation;

#if UNITY_EDITOR

        private void OnDrawGizmos()
        {
            if (showGrid)
            {
                DrawGridLines();

                Gizmos.color = areaColor;
                Gizmos.DrawCube(transform.position + Vector3.up * areaSize.y / 2f, areaSize);

                Gizmos.color = Color.yellow;
                DrawWireCube(transform.position + Vector3.up * areaSize.y / 2f, areaSize);
            }

            if (showSlots)
            {
                DrawItemSlots();
            }
        }

#endif

        private void DrawWireCube(Vector3 center, Vector3 size)
        {
            Vector3 halfSize = size / 2f;

            Vector3 p1 = center + new Vector3(-halfSize.x, -halfSize.y, -halfSize.z);
            Vector3 p2 = center + new Vector3(halfSize.x, -halfSize.y, -halfSize.z);
            Vector3 p3 = center + new Vector3(halfSize.x, -halfSize.y, halfSize.z);
            Vector3 p4 = center + new Vector3(-halfSize.x, -halfSize.y, halfSize.z);

            Vector3 p5 = center + new Vector3(-halfSize.x, halfSize.y, -halfSize.z);
            Vector3 p6 = center + new Vector3(halfSize.x, halfSize.y, -halfSize.z);
            Vector3 p7 = center + new Vector3(halfSize.x, halfSize.y, halfSize.z);
            Vector3 p8 = center + new Vector3(-halfSize.x, halfSize.y, halfSize.z);

            Gizmos.DrawLine(p1, p2);
            Gizmos.DrawLine(p2, p3);
            Gizmos.DrawLine(p3, p4);
            Gizmos.DrawLine(p4, p1);

            Gizmos.DrawLine(p5, p6);
            Gizmos.DrawLine(p6, p7);
            Gizmos.DrawLine(p7, p8);
            Gizmos.DrawLine(p8, p5);

            Gizmos.DrawLine(p1, p5);
            Gizmos.DrawLine(p2, p6);
            Gizmos.DrawLine(p3, p7);
            Gizmos.DrawLine(p4, p8);
        }

        private void DrawGridLines()
        {
            Gizmos.color = gridColor;

            Vector3 cubeCenter = transform.position + Vector3.up * areaSize.y / 2f;
            float cellWidth = areaSize.x / gridX;
            float cellDepth = areaSize.z / gridZ;
            float cellHeight = areaSize.y / maxLayers;

            for (int layer = 0; layer <= maxLayers; layer++)
            {
                float yPos = cubeCenter.y - areaSize.y / 2f + cellHeight * layer;

                for (int z = 0; z <= gridZ; z++)
                {
                    Vector3 start = cubeCenter + new Vector3(-areaSize.x / 2f, yPos - cubeCenter.y, -areaSize.z / 2f + z * cellDepth);
                    Vector3 end = start + new Vector3(areaSize.x, 0, 0);
                    Gizmos.DrawLine(start, end);
                }

                for (int x = 0; x <= gridX; x++)
                {
                    Vector3 start = cubeCenter + new Vector3(-areaSize.x / 2f + x * cellWidth, yPos - cubeCenter.y, -areaSize.z / 2f);
                    Vector3 end = start + new Vector3(0, 0, areaSize.z);
                    Gizmos.DrawLine(start, end);
                }
            }
        }

        private void DrawItemSlots()
        {
            Vector3 cubeCenter = transform.position + Vector3.up * areaSize.y / 2f;
            float cellWidth = areaSize.x / gridX;
            float cellDepth = areaSize.z / gridZ;
            float cellHeight = areaSize.y / maxLayers;

            Vector3 slotSize = new Vector3(cellWidth * 0.8f, cellHeight * 0.8f, cellDepth * 0.8f);

            for (int layer = 0; layer < maxLayers; layer++)
            {
                float yPos = cubeCenter.y - areaSize.y / 2f + cellHeight * layer + cellHeight / 2f;

                for (int x = 0; x < gridX; x++)
                {
                    for (int z = 0; z < gridZ; z++)
                    {
                        Vector3 slotPos = cubeCenter + new Vector3(
                            -areaSize.x / 2f + x * cellWidth + cellWidth / 2f,
                            yPos - cubeCenter.y,
                            -areaSize.z / 2f + z * cellDepth + cellDepth / 2f
                        );

                        Gizmos.color = filledSlotColor;

                        if (previewMesh == null)
                        {
                            Gizmos.DrawCube(slotPos, slotSize);
                            Gizmos.DrawWireCube(slotPos, slotSize);
                        }

                        else
                        {
                            Gizmos.DrawMesh(previewMesh, slotPos, previewMeshRotation);
                        }

                    }
                }
            }
        }

        public Vector3 GetSlotPosition(int index)
        {
            int slotsPerLayer = gridX * gridZ;
            int layer = index / slotsPerLayer;
            int indexInLayer = index % slotsPerLayer;

            int x = indexInLayer % gridX;
            int z = indexInLayer / gridX;

            Vector3 cubeCenter = transform.position + Vector3.up * areaSize.y / 2f;
            float cellWidth = areaSize.x / gridX;
            float cellDepth = areaSize.z / gridZ;
            float cellHeight = areaSize.y / maxLayers;

            float yPos = cubeCenter.y - areaSize.y / 2f + cellHeight * layer + cellHeight / 2f;

            Vector3 slotPos = cubeCenter + new Vector3(
                -areaSize.x / 2f + x * cellWidth + cellWidth / 2f,
                yPos - cubeCenter.y,
                -areaSize.z / 2f + z * cellDepth + cellDepth / 2f
            );

            return slotPos;
        }

        public int GetTotalCapacity()
        {
            return gridX * gridZ * maxLayers;
        }
    }
}