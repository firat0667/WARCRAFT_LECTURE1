using UnityEngine;

namespace AGP_Warcraft
{
    public class Human : Creature
    {
        protected override void Start()
        {
            base.Start();
            GameManager.I.SelectionChanged.AddListener((sc) => CheckIfSelected(sc));
        }

        protected override void Update()
        {
            base.Update(); 
        }
    }
}
