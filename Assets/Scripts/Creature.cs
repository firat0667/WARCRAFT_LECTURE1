using UnityEngine;
using Pathfinding;
using System.Collections.Generic;
using System.Linq;

namespace AGP_Warcraft
{
    public class Creature : MonoBehaviour
    {
        public bool isRanged;

        public Node CurrentPosition, Goal;
        public List<Node> CurrentPath = new List<Node>();

        private Creature Target;

        [SerializeField] private SpriteRenderer Selected;

        internal float ActionTimer;
        internal bool IsSelected;

        private int recalculateCounter;

        internal void CheckIfSelected(List<Creature> sc)
        {
            IsSelected = sc.Contains(this);
            Selected.enabled = IsSelected;
        }

        internal void ProcessActions()
        {
            ActionTimer += Time.deltaTime;

            if (ActionTimer > 0.2f)
            {
                ActionTimer = 0;
                if (Goal != null && Goal != CurrentPosition)
                {
                    MoveToNextPathNode();
                }
                else
                {
                    CurrentPath.Clear();
                }
            }
        }

        internal void MoveToNextPathNode()
        {
            if (CurrentPath.Count == 0) return;

            if (CurrentPath.First().isOccupied)
            {
                recalculateCounter++;
                if (recalculateCounter > 1)
                {
                    recalculateCounter = 0;
                    GetPathToPoint(Goal);
                }

                return;
            }

            CurrentPosition.isOccupied = false;
            CurrentPosition = CurrentPath.First();
            CurrentPosition.isOccupied = true;
            transform.position = CurrentPosition.Point;
            CurrentPath.Remove(CurrentPosition);
        }

        public void GetPathToPoint(Node goal)
        {
            Goal = goal;
            CurrentPath = GameManager.I.AStar.GetShortestPath(CurrentPosition, Goal);
            CurrentPath.Remove(CurrentPosition);
        }

        internal void MoveAndAttack(Creature target)
        {
            Target = target;
            Goal = Target.CurrentPosition;

            CurrentPath = GameManager.I.AStar.GetShortestPath(CurrentPosition, Goal);
            CurrentPath.Remove(CurrentPosition);

            if (isRanged)
            {
                CurrentPath.RemoveRange(Mathf.Max(CurrentPath.Count - 3, 0), Mathf.Min(3, CurrentPath.Count));
            }
        }
    }
}
