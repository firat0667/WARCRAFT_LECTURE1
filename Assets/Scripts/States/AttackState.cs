using AGP_Warcraft;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : IState
{
    private Creature _creature;
    private StateManager _state;
    public AttackState(Creature creature, StateManager stateManager)
    {
         _creature = creature;
         _state = stateManager;
    }
    public void EnterState()
    {
        Debug.Log($"{_creature.name} entered Attack State.");
        // Additional logic for entering attack state
    }
    public void UpdateState()
    {
        if (_creature.Target == null)
        {
            _state.ChangeState<IdleState>();
            return;
        }
        if (!_creature.InAttackRange(_creature.Target))
        {
            _state.ChangeState<MoveState>();
            return;
        }
        if (_creature.CanAttack())
        {
           _creature.PerformAttack();
        }
    }
    public void ExitState()
    {
        Debug.Log($"{_creature.name} exited Attack State.");
        // Additional logic for exiting attack state
    }
}
