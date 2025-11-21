using System.Linq;
using UnityEngine;

namespace AGP_Warcraft
{
    public class Orc : Creature
    {
        protected override void Start()
        {
            base.Start();
            GameManager.I.SelectionChanged.AddListener((sc) => CheckIfSelected(sc));
        }

        protected override void Update()
        {
            base.Update(); // FSM

            var goldFound = GameManager.I.Map.Resources
                .SingleOrDefault(gr => CurrentPosition.Point ==
                                       gr.GetComponent<Resource>().CurrentPosition.Point);

            if (goldFound != null)
            {
                GameManager.I.Map.Resources.Remove(goldFound.gameObject);
                Destroy(goldFound.gameObject);
                GameManager.I.GoldCollected++;
            }
        }
    }
}
