using AGP_Warcraft;
using TMPro;
using UnityEngine;

public class GameEconomySim : MonoBehaviour
{
    private float GoldSpawnTimer, MineYieldTimer, EndOfDayTimer;
    private int Mines;
    private int StonesSold;

    public float GoldTotalValue => GameManager.I.GoldCollected * GameEconomy.I.GoldValue;
    public float StonesTotalValue => GameManager.I.StonesCollected * GameEconomy.I.StoneValue;

    [SerializeField] private TextMeshProUGUI StonesSoldCounter, MinesCounter, TimeCounter, StonesValueText, StonesDemandText;

    // Start is called before the first frame update
    void Start()
    {
        AdjustStonesPrice();
    }

    // Update is called once per frame
    void Update()
    {
        GoldSpawnTimer += Time.deltaTime;
        MineYieldTimer += Time.deltaTime;
        EndOfDayTimer += Time.deltaTime;

        TimeCounter.text = "Time " + Mathf.FloorToInt(EndOfDayTimer) + ":00";

        CollectGold();
        BuildMine();
        CollectStone();

        if (EndOfDayTimer >= 24)
        {

            SellStones();
            PayForMaintenance();
            AdjustStonesPrice(); //Advanced feature

            EndOfDayTimer = 0f;
        }

        StonesSoldCounter.text = "Stones Sold: " + StonesSold;
        MinesCounter.text = "Mines: " + Mines;
    }

    private void PayForMaintenance()
    {
        GameManager.I.GoldCollected = GameManager.I.GoldCollected - Mines * GameEconomy.I.MineMaintenance;

        if (GameManager.I.GoldCollected < 0)
        {
            var minesToSell = Mathf.Abs(GameManager.I.GoldCollected / GameEconomy.I.MineMaintenance);
            Mines -= minesToSell;
            GameManager.I.GoldCollected = 0;
        }
    }

    private void CollectGold()
    {
        if (GoldSpawnTimer > GameEconomy.I.GoldSpawnFrequency)
        {
            GoldSpawnTimer = 0;
            GameManager.I.GoldCollected++;
        }
    }

    private void BuildMine()
    {
        if (GameManager.I.GoldCollected >= GameEconomy.I.MineCost)
        {
            GameManager.I.GoldCollected -= GameEconomy.I.MineCost;
            Mines++;
        }
    }

    private void CollectStone()
    {
        if (MineYieldTimer > GameEconomy.I.MineActivationFrequency)
        {
            MineYieldTimer = 0;
            GameManager.I.StonesCollected += GameEconomy.I.MineYieldedQuantity * Mines;
        }
    }

    private void SellStones()
    {
        if (GameManager.I.StonesCollected > GameEconomy.I.StonesToStore)
        {
            var StonesToSell = Mathf.Clamp(GameManager.I.StonesCollected - GameEconomy.I.StonesToStore, 0, GameEconomy.I.StonesDemand);
            
            GameManager.I.StonesCollected -= StonesToSell;
            StonesSold += StonesToSell;

            GameManager.I.GoldCollected += Mathf.CeilToInt(GameManager.I.StonesCollected * GameEconomy.I.StoneValue / GameEconomy.I.GoldValue);
        }
    }

    private void AdjustStonesPrice()
    {
        GameEconomy.I.StoneValue = Mathf.Clamp(GameEconomy.I.StoneValue + GameEconomy.I.StonesDemand / 10f - 1f, 1f, 10f); //10f & 1f arbitrary coefficents because monopoly

        GameEconomy.I.TotalStonesNeeded += Random.Range(0, 20); //20f arbitrary limit
        GameEconomy.I.StonesDemand = GameEconomy.I.TotalStonesNeeded - StonesSold;

        StonesDemandText.text = "Stone Demand: " + GameEconomy.I.StonesDemand;
        StonesValueText.text = "Stone Value: " + GameEconomy.I.StoneValue.ToString("N2");
    }
}
