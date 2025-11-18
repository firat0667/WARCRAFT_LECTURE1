using System.Collections.Generic;
using System.Linq;
using AGP_Warcraft;
using UnityEngine;

namespace Pathfinding
{
    public class AStar
    {
        public Map Map { get; set; }
        public int NodeVisits { get; private set; }
        public double ShortestPathLength { get; set; }
        public double ShortestPathCost { get; private set; }

        public AStar(Map map)
        {
            Map = map;
        }

        public List<Node> GetShortestPath(Node start, Node goal)
        {
            Search(start, goal);
            
            var shortestPath = new List<Node>();
            shortestPath.Add(goal);
            
            BuildShortestPath(shortestPath, goal);
            
            shortestPath.Reverse();

            foreach (var node in GameManager.I.Map.Tiles)
            {
                node.Reset();
            }
            
            return shortestPath;
        }

        private void BuildShortestPath(List<Node> list, Node node)
        {
            if (node.NearestToStart == null)
                return;
            list.Add(node.NearestToStart);
            ShortestPathLength += node.Connections.Single(x => x.ConnectedNode == node.NearestToStart).Length;
            ShortestPathCost += node.Connections.Single(x => x.ConnectedNode == node.NearestToStart).Cost;
            BuildShortestPath(list, node.NearestToStart);
        }

        private void Search(Node start, Node goal)
        {
            NodeVisits = 0;
            start.MinCostToStart = 0;
            
            var prioQueue = new List<Node>();
            prioQueue.Add(start);

            do
            {
                prioQueue = prioQueue.OrderBy(x => x.MinCostToStart + Vector2.Distance(x.Point, goal.Point)).ToList();
                var node = prioQueue.First();
                prioQueue.Remove(node);

                NodeVisits++;

                foreach (var cnn in node.Connections.OrderBy(x => x.Cost))
                {
                    var childNode = cnn.ConnectedNode;
                    
                    if (childNode.Visited)
                        continue;
                    
                    if (childNode.MinCostToStart == null || node.MinCostToStart + cnn.Cost < childNode.MinCostToStart)
                    {
                        childNode.MinCostToStart = node.MinCostToStart + cnn.Cost;
                        childNode.NearestToStart = node;
                        
                        if (!prioQueue.Contains(childNode))
                            prioQueue.Add(childNode);
                    }
                }

                node.Visited = true;

                if (node == goal)
                    return;

            }
            while (prioQueue.Any());
        }
    }
}
