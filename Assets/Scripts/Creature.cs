using UnityEngine;
using Pathfinding;
using System.Collections.Generic;
using System.Linq;

namespace AGP_Warcraft
{
    public enum Team
    {
        Orcs,
        Humans
    }
    public class Creature : MonoBehaviour
    {
        public bool isRanged;

        public int RangedStopDistance = 3;
        public float AttackRange = 1.2f;
        public float AttackCooldown = 1f;

        private float _attackTimer = 0f;

        public Node CurrentPosition, Goal;
        public List<Node> CurrentPath = new List<Node>();

        public bool IsControlledByPlayer = false;
        public bool HasPlayerCommand = false;

        private Creature m_target;

        public Creature Target
        {
            get { return m_target; }
            set { m_target = value; }
        }

        [SerializeField] private SpriteRenderer Selected;

        internal float ActionTimer;
        internal bool IsSelected;

        private int recalculateCounter;
        private StateManager _state;

        private Team _team;
        public Team CreatureTeam
        {
            get { return _team; }
            set { _team = value; }
        }
        protected virtual void Start()
        {
            _state = new StateManager();
            _state.Initialize(this);
            _state.ChangeState<IdleState>();
        }

        protected virtual void Update()
        {
            _state.OnUpdate();
        }


        internal void CheckIfSelected(List<Creature> sc)
        {
            // if selected creatures contains this creature
            IsSelected = sc.Contains(this);
            // enable/disable selection sprite
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
        public Creature FindTarget()
        {
 
            var allCreatures = FindObjectsOfType<Creature>();


            var enemies = allCreatures
                .Where(c => c != this && c.CreatureTeam != this.CreatureTeam)
                .ToList();

            if (enemies.Count == 0)
                return null;

            return enemies
                .OrderBy(e => Vector2.Distance(transform.position, e.transform.position))
                .First();
        }


        public bool InAttackRange(Creature target)
        {
            if (target == null) return false;

            float dist = Vector2.Distance(transform.position, target.transform.position);
            return dist <= AttackRange;
        }
        public void CalculatePathTo(Creature target)
        {
            if (target == null) return;

            Goal = target.CurrentPosition;

            CurrentPath = GameManager.I.AStar.GetShortestPath(CurrentPosition, Goal);
            CurrentPath.Remove(CurrentPosition);

        if (isRanged)
            {
                int stopDist = RangedStopDistance;

                CurrentPath.RemoveRange(
                    Mathf.Max(CurrentPath.Count - stopDist, 0),
                    Mathf.Min(stopDist, CurrentPath.Count)
                );
            }
        }
        public void MoveAlongPath()
        {
            if (CurrentPath == null || CurrentPath.Count == 0)
                return;

            MoveToNextPathNode(); 
        }
        public void PerformAttack()
        {
            if (Target == null) return;
            Debug.Log(name + " HIT " + Target.name);
        }
        public bool CanAttack()
        {
            _attackTimer += Time.deltaTime;

            if (_attackTimer >= AttackCooldown)
            {
                _attackTimer = 0;
                return true;
            }

            return false;
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
            m_target = target;
            Goal = target.CurrentPosition;

            CurrentPath = GameManager.I.AStar.GetShortestPath(CurrentPosition, Goal);
            CurrentPath.Remove(CurrentPosition);

            if (isRanged)
            {
                int stopDist = RangedStopDistance;

                CurrentPath.RemoveRange(
                    Mathf.Max(CurrentPath.Count - stopDist, 0),
                    Mathf.Min(stopDist, CurrentPath.Count)
                );
            }

        }
    }
}
