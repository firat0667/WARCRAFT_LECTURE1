using AGP_Warcraft;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveState : IState
{
  private Creature _creature;
  private StateManager _state;
    public MoveState(Creature creature, StateManager stateManager)
    {
         _creature = creature;
         _state = stateManager;
    }
    public void EnterState()
    {

        if (_creature.Target != null)
        {
            _creature.CalculatePathTo(_creature.Target);
        }
        Debug.Log($"{_creature.name} entered Move State.");
        // Additional logic for entering move state
    }
    public void UpdateState()
    {
        // Logic for updating move state
        // For example, check if the creature has reached its destination to transition to IdleState

        if (_creature.Target == null)
        {
            _state.ChangeState<IdleState>();
            return;
        }
        if (_creature.InAttackRange(_creature.Target))
        {
            _state.ChangeState<AttackState>();
            return;
        }
        _creature.MoveAlongPath();

        if (_creature.CurrentPath.Count == 0)
        {
          _creature.CalculatePathTo(_creature.Target);
        }
    }
    public void ExitState()
    {
        Debug.Log($"{_creature.name} exited Move State.");
        // Additional logic for exiting move state
    }
}
