using System.Linq;
using UnityEngine;

namespace AGP_Warcraft
{
    public class Orc : Creature
    {
        void Start()
        {
            GameManager.I.SelectionChanged.AddListener((sc) => CheckIfSelected(sc));
        }

        void Update()
        {
            ProcessActions();

            var goldFound = GameManager.I.Map.Resources.SingleOrDefault(gr => CurrentPosition.Point == gr.GetComponent<Resource>().CurrentPosition.Point);

            if (goldFound != null)
            {
                GameManager.I.Map.Resources.Remove(goldFound.gameObject);
                Destroy(goldFound.gameObject);
                GameManager.I.GoldCollected++;
            }
        }
    }
}
