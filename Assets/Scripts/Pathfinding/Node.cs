using System.Collections.Generic;
using UnityEngine;
using System;

namespace Pathfinding
{
    public class Node
    {
        public bool isObstacle, isOccupied;

        public GameObject Sprite;

        public string Name { get; set; }
        public Vector2 Point { get; set; }
        public List<Edge> Connections { get; set; } = new List<Edge>();

        public double? MinCostToStart { get; set; }
        public Node NearestToStart { get; set; }
        public bool Visited { get; set; }


        public override string ToString()
        {
            return Name;
        }

        internal void Reset()
        {
            MinCostToStart = null;
            NearestToStart = null;
            Visited = false;
        }
    }

    public class Edge
    {
        private double cost;

        public double Length { get; set; }
        public Node ConnectedNode { get; set; }

        public double Cost
        {
            get { if (ConnectedNode.isOccupied || ConnectedNode.isObstacle) return 10000; return cost; }
            set { cost = value; }
        }

        public override string ToString()
        {
            return "-> " + ConnectedNode.ToString();
        }
    }
}
