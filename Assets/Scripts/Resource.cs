using Pathfinding;
using UnityEngine;

public class Resource : MonoBehaviour
{
    public Node CurrentPosition = new Node();
    public ResourceType Type;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public enum ResourceType
    {
        Gold,
        Wood,
        Stone
    }
}