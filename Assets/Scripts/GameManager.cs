using Helpers;
using Pathfinding;
using System;
using System.Collections.Generic;
using System.Linq;
using TechTree;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace AGP_Warcraft
{
    public sealed class GameManager : MonoSingleton<GameManager>
    {
        [HideInInspector] public AStar AStar;

        public Map Map;
        public GameObject GrassTile, RockTile;
        public Orc OrcPrefab;
        public Human HumanPrefab;
        public GameObject UnitsParent;
        public Mine MineToken;
        public int orcsQty, humansQty;

        [SerializeField] private TextMeshProUGUI GoldCounter, StoneCounter;

        public List<Creature> SelectedCreatures = new List<Creature>();
        public List<Upgrade> BoughtUpgrades = new List<Upgrade>();

        public UnityEvent<List<Creature>> SelectionChanged = new UnityEvent<List<Creature>>();

        public int GoldCollected = 0;
        public int StonesCollected = 0;
        public int Level;

        void Start()
        {
            AStar = new AStar(Map);
            Map.Init();

            for (int x = 0; x < orcsQty; x++)
            {
                AddOrc();
            }

            for (int x = 0; x < humansQty; x++)
            {
                AddHuman();
            }
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                MoveAttackOrSelect();
            }

            if (Input.GetMouseButtonDown(1))
            {
                AddOrDeselect();
            }

            if (Input.GetKeyDown(KeyCode.Space))
            {
                BuildMine();
            }

            GoldCounter.text = "Gold: " + GoldCollected;
            StoneCounter.text = "Stones: " + StonesCollected;
        }

        private void BuildMine()
        {
            if (SelectedCreatures.Count > 0 && GoldCollected >= GameEconomy.I.MineCost)
            {
                Instantiate(MineToken, SelectedCreatures[0].CurrentPosition.Point, Quaternion.identity, UnitsParent.transform);
                GoldCollected -= GameEconomy.I.MineCost;
            }
        }

        private void MoveAttackOrSelect()
        {
            var hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);

            if (hit.collider != null)
            {
                var node = Map.GetNodeByName(hit.collider.name);
                
                if (node != null && !node.isObstacle)
                {
                    SelectedCreatures.ForEach(sc => sc.GetPathToPoint(node));
                }

                var creature = hit.collider.GetComponent<Creature>();

                if (creature != null)
                {
                    if (creature is Orc)
                    {
                        SelectedCreatures = new List<Creature> { creature };
                        SelectionChanged.Invoke(SelectedCreatures);
                    }

                    if (creature is Human)
                    {
                        SelectedCreatures.ForEach(sc => sc.MoveAndAttack(creature));
                    }
                }
            }
        }

        private void AddOrDeselect()
        {
            var hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);

            if (hit.collider != null)
            {
                var creature = hit.collider.GetComponent<Creature>();

                if (creature != null && creature is Orc)
                {
                    SelectedCreatures.Add(creature);
                }
                else
                {
                    SelectedCreatures.Clear();
                }

                SelectionChanged.Invoke(SelectedCreatures);
            }
        }

        private void AddOrc()
        {
            var position = Map.Tiles.Where(t => t.Point.x < 3 && !t.isObstacle && !t.isOccupied).OrderBy(rnd => Guid.NewGuid()).First();
            var orc = Instantiate(OrcPrefab, position.Point, Quaternion.identity, UnitsParent.transform);
            orc.CurrentPosition = position;
            orc.CurrentPosition.isOccupied = true;

            orc.isRanged = UnityEngine.Random.Range(0, 2) == 1 ? true : false;
        }

        private void AddHuman()
        {
            var position = Map.Tiles.Where(t => t.Point.x > Map.horizontalSize - 3 && !t.isObstacle && !t.isOccupied).OrderBy(rnd => Guid.NewGuid()).First();
            var human = Instantiate(HumanPrefab, position.Point, Quaternion.identity, UnitsParent.transform);
            human.CurrentPosition = position;
            human.CurrentPosition.isOccupied = true;

            human.isRanged = UnityEngine.Random.Range(0, 2) == 1 ? true : false;
        }
    }
}
