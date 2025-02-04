using Aoiti.Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ginko.CoreSystem
{
    public class Pathfinding : CoreComponent
    {
        [Header("Navigator options")]
        [SerializeField] float gridSize = 0.5f; //increase patience or gridSize for larger maps

        Pathfinder<Vector2> pathfinder; //the pathfinder object that stores the methods and patience
        [Tooltip("The layers that the navigator can not pass through.")]
        [SerializeField] LayerMask obstacles;

        [Tooltip("Deactivate to make the navigator move along the grid only, except at the end when it reaches to the target point. This shortens the path but costs extra Physics2D.LineCast")]
        [SerializeField] bool searchShortcut = false;

        [Tooltip("Deactivate to make the navigator to stop at the nearest point on the grid.")]
        [SerializeField]
        public bool snapToGrid = false;
        [SerializeField] private PathFindingType pathfindingType;
        [SerializeField] private float alignDistance;

        [SerializeField] bool drawDebugLines;

        private List<Vector2> path;
        private List<Vector2> pathLeftToGo = new List<Vector2>();

        private Movement Movement
        {
            get => movement ??= Core.GetCoreComponent<Movement>();
        }
        private Movement movement;

        private enum PathFindingType
        {
            Target,
            Target_Align,
        }

        private void Start()
        {
            pathfinder = new Pathfinder<Vector2>(GetDistance, GetNeighbourNodes, 1000); //increase patience or gridSize for larger maps
        }
        
        private List<Vector2> GetPathList(Vector2 target)
        {
            Vector2 closestNode = GetClosestNode(Movement.RB.transform.position);
            if (pathfinder.GenerateAstarPath(closestNode, GetClosestNode(target), out path)) //Generate path between two points on grid that are close to the transform position and the assigned target.
            {
                if (searchShortcut && path.Count > 0)
                    pathLeftToGo = ShortenPath(path);
                else
                {
                    pathLeftToGo = new List<Vector2>(path);
                    if (!snapToGrid) pathLeftToGo.Add(target);
                }
            }
            if (drawDebugLines)
            {
                for (int i = 0; i < pathLeftToGo.Count - 1; i++) //visualize your path in the sceneview
                {
                    Debug.DrawLine(pathLeftToGo[i], pathLeftToGo[i + 1]);
                }
            }

            return pathLeftToGo;
        }

        public List<Vector2> GetMoveCommand(Vector2 relativePos)
        {
            if (pathfindingType == PathFindingType.Target)
            {
                return GetPathList(relativePos);
            }

            Vector3 basePosition = transform.position;
            if ((transform.position - (Vector3)relativePos).sqrMagnitude > alignDistance * alignDistance)
            {
                basePosition = relativePos + ((Vector2)transform.position - relativePos).normalized * alignDistance;
            }

            Vector2 pendingPosA = new Vector2(basePosition.x, relativePos.y);
            Vector2 pendingPosB = new Vector2(relativePos.x, basePosition.y);

            List<Vector2> pendingListA = GetPathList(pendingPosA);
            List<Vector2> pendingListB = GetPathList(pendingPosB);

            return CalculateSqrPathDistance(pendingListA) > CalculateSqrPathDistance(pendingListB)
                ? pendingListB
                : pendingListA;
        }

        private float CalculateSqrPathDistance(List<Vector2> path)
        {
            float output = 0f;

            for(int i = 0;i < path.Count - 1;i++)
            {
                output += GetDistance(path[i], path[i + 1]);
            }

            return output;
        }

        /// <summary>
        /// Finds closest point on the grid
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        Vector2 GetClosestNode(Vector2 target)
        {
            return new Vector2(Mathf.Round(target.x / gridSize) * gridSize, Mathf.Round(target.y / gridSize) * gridSize);
        }

        /// <summary>
        /// A distance approximation. 
        /// </summary>
        /// <param name="A"></param>
        /// <param name="B"></param>
        /// <returns></returns>
        float GetDistance(Vector2 A, Vector2 B)
        {
            return (A - B).sqrMagnitude; //Uses square magnitude to lessen the CPU time.
        }
        /// <summary>
        /// Finds possible conenctions and the distances to those connections on the grid.
        /// </summary>
        /// <param name="pos"></param>
        /// <returns></returns>
        Dictionary<Vector2, float> GetNeighbourNodes(Vector2 pos)
        {
            Dictionary<Vector2, float> neighbours = new Dictionary<Vector2, float>();
            for (int i = -1; i < 2; i++)
            {
                for (int j = -1; j < 2; j++)
                {
                    if (i == 0 && j == 0) continue;

                    Vector2 dir = new Vector2(i, j) * gridSize;
                    if (!Physics2D.Linecast(pos, pos + dir, obstacles))
                    {
                        neighbours.Add(GetClosestNode(pos + dir), dir.magnitude);
                    }
                }

            }
            return neighbours;
        }

        List<Vector2> ShortenPath(List<Vector2> path)
        {
            List<Vector2> newPath = new List<Vector2>();

            for (int i = 0; i < path.Count; i++)
            {
                newPath.Add(path[i]);
                for (int j = path.Count - 1; j > i; j--)
                {
                    if (!Physics2D.Linecast(path[i], path[j], obstacles))
                    {

                        i = j;
                        break;
                    }
                }
                newPath.Add(path[i]);
            }
            newPath.Add(path[path.Count - 1]);
            return newPath;
        }
    }
}