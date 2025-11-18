namespace AGP_Warcraft
{
    public class Human : Creature
    {
        void Start()
        {
            GameManager.I.SelectionChanged.AddListener((sc) => CheckIfSelected(sc));
        }

        void Update()
        {
            ProcessActions();
        }
    }
}
