using System;
using System.Collections.Generic;
using System.Numerics;
using VG.Collections;

namespace VG
{
    public class Node
    {
        public readonly bool Walkable;
        public readonly float Weight;
        public float Cost;
        public float DistanceToTarget;
        public Node Parent;
        public Vector2 Position;

        public Node(Vector2 pos, bool walkable, float weight = 1)
        {
            Parent = null;
            Position = pos;
            DistanceToTarget = -1;
            Cost = 1;
            Weight = weight;
            Walkable = walkable;
        }

        public float F =>
            Math.Abs(DistanceToTarget + 1) > 0.01f && Math.Abs(Cost + 1) > 0.01f
                ? DistanceToTarget + Cost
                : -1;
    }

    public class AStar
    {
        private readonly Node[,] _grid;


        public AStar(Node[,] grid)
        {
            _grid = grid;
        }

        private int GridCols => _grid.GetLength(0);
        private int GridRows => _grid.GetLength(1);

        public Stack<Node> FindPath(Vector2 startCoords, Vector2 endCoords)
        {
            var start = new Node(
                new Vector2((int)startCoords.X, (int)startCoords.Y), true);
            var end = new Node(new Vector2((int)endCoords.X, (int)endCoords.Y),
                true);

            var path = new Stack<Node>();
            var openList = new PriorityQueue<Node>(true);
            var closedList = new List<Node>();
            var current = start;

            // add start node to Open List
            openList.Enqueue(start, start.F);

            while (openList.Count != 0 && !closedList.Exists(x => x.Position == end.Position))
            {
                current = openList.Dequeue();
                closedList.Add(current);
                var neighbours = GetNeighbourNodes(current);

                foreach (var n in neighbours)
                    if (!closedList.Contains(n) && n.Walkable)
                        if (!openList.Contains(n))
                        {
                            n.Parent = current;
                            n.DistanceToTarget = Math.Abs(n.Position.X - end.Position.X) +
                                                 Math.Abs(n.Position.Y - end.Position.Y);
                            n.Cost = n.Weight + n.Parent.Cost;
                            openList.Enqueue(n, n.F);
                        }
            }

            // construct path, if end was not closed return null
            if (!closedList.Exists(x => x.Position == end.Position)) return null;

            // if all good, return path
            var temp = closedList[closedList.IndexOf(current)];
            if (temp == null) return null;
            do
            {
                path.Push(temp);
                temp = temp.Parent;
            } while (temp != start && temp != null);

            return path;
        }

        private List<Node> GetNeighbourNodes(Node n)
        {
            var temp = new List<Node>();

            var row = (int)n.Position.Y;
            var col = (int)n.Position.X;

            if (row + 1 < GridRows) temp.Add(_grid[col, row + 1]);
            if (row - 1 >= 0) temp.Add(_grid[col, row - 1]);
            if (col - 1 >= 0) temp.Add(_grid[col - 1, row]);
            if (col + 1 < GridCols) temp.Add(_grid[col + 1, row]);

            return temp;
        }
    }
}