using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomMap : MonoBehaviour
{
    public struct CubeCoordinate
    {
        public int x;
        public int y;
        public int z;

        public CubeCoordinate(int x, int y, int z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        public CubeCoordinate CubePositionAdd(CubeCoordinate offset)
        {
            return new CubeCoordinate(x + offset.x, y + offset.y, z + offset.z);
        }
    }

    public struct OffsetCoordinate
    {
        public int col;
        public int row;

        public OffsetCoordinate(int col, int row)
        {
            this.col = col;
            this.row = row;
        }

        public OffsetCoordinate CubePositionAdd(OffsetCoordinate offset)
        {
            return new OffsetCoordinate(col + offset.col, row + offset.row);
        }
    }

    public GameObject landBlockPrefab;

    public int hexSize;
    public int mapSize;

    public int rectangleWidth;
    public int rectangleHeight;

    private static readonly List<CubeCoordinate> hexDirectionOffset = new List<CubeCoordinate> {
            new CubeCoordinate(-1, 0, 1), new CubeCoordinate(1, -1, 0), new CubeCoordinate(0, -1, 1),
            new CubeCoordinate(1, 0, -1), new CubeCoordinate(0, 1, -1), new CubeCoordinate(-1, 1, 0)
    };

    private List<Vector2> hexPositionOffset;

    private List<CubeCoordinate> cubePosList;
    private List<Vector2> worldPosList;

    private float widthDistance;
    private float heightDistance;

    // Use this for initialization
    void Start()
    {
        InitHexsDistance();
        InitHexPosOffset();

        cubePosList = new List<CubeCoordinate>();
        worldPosList = new List<Vector2>();

        //CreateHexagonalMap();
        //CreateRectangleMap();
        CreateRandomHexagonalMap();
        //CreateRandomRectangleMap();
    }

    private void InitHexsDistance()
    {
        widthDistance = hexSize * 0.75f * 0.01f;
        heightDistance = Mathf.Sqrt(3f) * hexSize * 0.5f * 0.01f;
    }

    private void InitHexPosOffset()
    {
        hexPositionOffset = new List<Vector2> {
            new Vector2(-widthDistance, -heightDistance*0.5f),new Vector2(widthDistance, -heightDistance*0.5f), new Vector2(0,-heightDistance),
            new Vector2(widthDistance, heightDistance*0.5f),new Vector2(0, heightDistance), new Vector2(-widthDistance,heightDistance*0.5f)
        };
    }

    public float GetTwoHexDistance(CubeCoordinate a, CubeCoordinate b)
    {
        return (Mathf.Abs(a.x - b.x) + Mathf.Abs(a.y - b.y) + Mathf.Abs(a.z - b.z)) / 2;
    }

    private void CreateHexagonalMap()
    {
        Queue<CubeCoordinate> cubePosQueue_BFS = new Queue<CubeCoordinate>();

        CubeCoordinate currentCubePos;
        CubeCoordinate nextCubePos;
        Vector2 currentWorldPos;
        Vector2 nextWorldPos;

        cubePosList.Add(new CubeCoordinate(0, 0, 0));
        worldPosList.Add(Vector2.zero);

        cubePosQueue_BFS.Enqueue(cubePosList[0]);

        Instantiate(landBlockPrefab, worldPosList[0], Quaternion.identity, transform);

        while (cubePosQueue_BFS.Count > 0)
        {
            currentCubePos = cubePosQueue_BFS.Dequeue();
            currentWorldPos = worldPosList[cubePosList.IndexOf(currentCubePos)];

            for (int j = 0; j < 3; j++)
            {
                nextCubePos = currentCubePos.CubePositionAdd(hexDirectionOffset[j]);

                if (!cubePosList.Contains(nextCubePos) &&
                    nextCubePos.x >= -mapSize &&
                    nextCubePos.x <= mapSize &&
                    nextCubePos.y >= -mapSize * 2 &&
                    nextCubePos.z <= mapSize * 2)
                {
                    nextWorldPos = currentWorldPos + hexPositionOffset[j];

                    cubePosList.Add(nextCubePos);
                    worldPosList.Add(nextWorldPos);

                    cubePosQueue_BFS.Enqueue(nextCubePos);

                    Instantiate(landBlockPrefab, nextWorldPos, Quaternion.identity, transform);
                }
            }
        }
    }

    private void CreateRectangleMap()
    {
        for (int i = 0; i < rectangleHeight * 2; i++)
        {
            for (int j = i % 2; j < rectangleWidth; j += 2)
            {
                cubePosList.Add(OffsetToCube_Oddq(i, j));

                Instantiate(landBlockPrefab, new Vector2(j * widthDistance, -heightDistance * 0.5f * i), Quaternion.identity, transform);
            }
        }
    }

    private CubeCoordinate OffsetToCube_Oddq(int col, int row)
    {
        int x = col;
        int z = row - (col - (col & 1)) / 2;

        int y = -x - z;

        return new CubeCoordinate(x, y, z);
    }

    private void CreateRandomHexagonalMap()
    {
        Queue<CubeCoordinate> cubePosQueue_BFS = new Queue<CubeCoordinate>();

        CubeCoordinate currentCubePos;
        CubeCoordinate nextCubePos;

        Vector2 nextWorldPos;

        int times = 1;
        int curentDirection = -1;

        cubePosQueue_BFS.Enqueue(new CubeCoordinate(0, 0, 0));
        cubePosList.Add(new CubeCoordinate(0, 0, 0));
        worldPosList.Add(Vector2.zero);

        Instantiate(landBlockPrefab, Vector2.zero, Quaternion.identity, transform);

        while (cubePosQueue_BFS.Count > 0)
        {
            times = Random.Range(1, 3);

            currentCubePos = cubePosQueue_BFS.Dequeue();

            for (int i = 0; i < times; i++)
            {
                if (times == 1)
                {
                    curentDirection = Random.Range(0, 2);
                }
                else
                {
                    curentDirection = i;
                }

                nextCubePos = currentCubePos.CubePositionAdd(hexDirectionOffset[curentDirection]);

                if (!cubePosList.Contains(nextCubePos) &&
                    nextCubePos.x >= -mapSize &&
                    nextCubePos.x <= mapSize &&
                    nextCubePos.y >= -mapSize * 2 &&
                    nextCubePos.z <= mapSize * 2)
                {
                    nextWorldPos = worldPosList[cubePosList.IndexOf(currentCubePos)] + hexPositionOffset[curentDirection];

                    cubePosQueue_BFS.Enqueue(nextCubePos);
                    cubePosList.Add(nextCubePos);
                    worldPosList.Add(nextWorldPos);

                    Instantiate(landBlockPrefab, nextWorldPos, Quaternion.identity, transform);
                }
            }
        }
    }

    private void CreateRandomRectangleMap()
    {
        Queue<OffsetCoordinate> offsetPosQueue_BFS = new Queue<OffsetCoordinate>();

        OffsetCoordinate currentOffsetPos;
        OffsetCoordinate nextOffsetPos;
        CubeCoordinate nextCubePos;

        Vector2 nextWorldPos;

        int[] direction = new int[2] { -1, 1 };

        int times = 1;
        int curentDirection = -1;

        for (int i = 0; i <= rectangleWidth; i += 2)
        {
            offsetPosQueue_BFS.Enqueue(new OffsetCoordinate(i, 0));
            cubePosList.Add(OffsetToCube_Oddq(i, 0));
            worldPosList.Add(new Vector2(i * widthDistance, 0));

            Instantiate(landBlockPrefab, new Vector2(i * widthDistance, 0), Quaternion.identity, transform);
        }

        while (offsetPosQueue_BFS.Count > 0)
        {
            times = Random.Range(1, 3);

            currentOffsetPos = offsetPosQueue_BFS.Dequeue();

            for (int i = 0; i < times; i++)
            {
                if (times == 1)
                {
                    curentDirection = direction[Random.Range(0, 2)];
                }
                else
                {
                    curentDirection = direction[i];
                }

                if (currentOffsetPos.col % 2 == 0)
                {
                    nextOffsetPos = currentOffsetPos.CubePositionAdd(new OffsetCoordinate(curentDirection, 0));
                }
                else
                {
                    nextOffsetPos = currentOffsetPos.CubePositionAdd(new OffsetCoordinate(curentDirection, 1));
                }

                nextCubePos = OffsetToCube_Oddq(nextOffsetPos.col, nextOffsetPos.row);

                if (!cubePosList.Contains(nextCubePos) &&
                    nextOffsetPos.col >= 0 &&
                    nextOffsetPos.col <= rectangleWidth &&
                    nextOffsetPos.row >= 0 &&
                    nextOffsetPos.row <= rectangleHeight)
                {
                    if (nextOffsetPos.col % 2 == 0)
                    {
                        nextWorldPos = new Vector2(nextOffsetPos.col * widthDistance, -heightDistance * nextOffsetPos.row);
                    }
                    else
                    {
                        nextWorldPos = new Vector2(nextOffsetPos.col * widthDistance, -heightDistance * 0.5f - heightDistance * nextOffsetPos.row);
                    }

                    offsetPosQueue_BFS.Enqueue(nextOffsetPos);
                    cubePosList.Add(nextCubePos);
                    worldPosList.Add(nextWorldPos);

                    Instantiate(landBlockPrefab, nextWorldPos, Quaternion.identity, transform);
                }
            }
        }
    }
}