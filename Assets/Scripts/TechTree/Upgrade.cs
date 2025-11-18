using AGP_Warcraft;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace TechTree
{
    public class Upgrade : MonoBehaviour
    {
        public string Name;
        public int Cost;
        public Image Icon;
        public bool IsBought;

        public int LevelNeeded;
        public List<Upgrade> Requirements;

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public bool IsAvailable()
        {
            return (!IsBought || GameManager.I.GoldCollected >= Cost) && Requirements.All(r => r.IsBought) && GameManager.I.Level >= LevelNeeded;
        }
    }
}
