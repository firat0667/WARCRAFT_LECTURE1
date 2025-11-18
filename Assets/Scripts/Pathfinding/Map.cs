using AGP_Warcraft;
using Pathfinding;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Map : MonoBehaviour
{
    public int horizontalSize, verticalSize, obstaclePercent, goldPercent;
    public float processingInterval;

    public List<Node> Tiles => Nodes.Cast<Node>().ToList();

    [SerializeField] private GameObject WorldParent, ResourcesParent, GoldPrefab;

    private Node[,] Nodes;

    public List<GameObject> Resources = new List<GameObject>();

    public void Init()
    {
        Nodes = new Node[horizontalSize, verticalSize];

        for (int x = 0; x < horizontalSize; x++)
        {
            for (int y = 0; y < verticalSize; y++)
            {
                var isBlocked = Random.Range(0, 100) < obstaclePercent ? true : false;

                var tile = Instantiate(isBlocked ? GameManager.I.RockTile : GameManager.I.GrassTile, new Vector2(x, y), Quaternion.identity, WorldParent.transform);
                
                tile.name = x + "_" + y;

                Nodes[x, y] = new Node();
                Nodes[x, y].Name = tile.name;
                Nodes[x, y].isObstacle = isBlocked;
                Nodes[x, y].Point = new Vector2(x, y);
                Nodes[x, y].Sprite = tile;

                if (!Nodes[x, y].isObstacle && Random.Range(0, 100) < goldPercent)
                {
                    var gold = Instantiate(GoldPrefab, Nodes[x, y].Point, Quaternion.identity, ResourcesParent.transform);
                    gold.GetComponent<Resource>().CurrentPosition = Nodes[x, y];
                    gold.GetComponent<Resource>().Type = Resource.ResourceType.Gold;
                    Resources.Add(gold);
                }
            }
        }

        foreach (var node in Nodes)
        {
            var conn = Tiles.Where(n => n != node && Vector2.Distance(n.Point, node.Point) < 1.5).ToList();

            foreach (var cn in conn)
            {
                node.Connections.Add(new Edge
                {
                    ConnectedNode = cn,
                    Length = Vector2.Distance(cn.Point, node.Point),
                    Cost = node.isObstacle ? Mathf.Infinity : Vector2.Distance(cn.Point, node.Point)
                });
            }
        }
    }

    internal Node GetNodeByName(string name)
    {
       return Tiles.Where(t=> t.Name == name).FirstOrDefault();
    }
}