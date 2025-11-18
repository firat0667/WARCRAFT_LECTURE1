using AGP_Warcraft;
using UnityEngine;

public class Mine : MonoBehaviour
{
    internal float ActivationTimer;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        ActivationTimer += Time.deltaTime;

        if (ActivationTimer > GameEconomy.I.MineActivationFrequency)
        {
            ActivationTimer = 0;
            GameManager.I.StonesCollected += GameEconomy.I.MineYieldedQuantity;
        }
    }
}