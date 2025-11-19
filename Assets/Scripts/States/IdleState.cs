using AGP_Warcraft;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : IState
{
   private Creature _creature;
   private StateManager _state;

   private float targetSearchTimer;
    public IdleState(Creature creature, StateManager stateManager)
    {
         _creature = creature;
        _state = stateManager;
    }
    public void EnterState()
    {
        Debug.Log($"{_creature.name} entered Idle State.");
        // Additional logic for entering idle state
    }
    public void UpdateState()
    {
        targetSearchTimer += Time.deltaTime;

        if (targetSearchTimer >= 0.5f)
        {
            targetSearchTimer = 0f;
            _creature.Target = _creature.FindTarget();
        }

        if (_creature.Target == null)
            return;

        if (_creature.InAttackRange(_creature.Target))
        {
            _state.ChangeState<AttackState>();
            return;
        }

        _state.ChangeState<MoveState>();
    }
    public void ExitState()
    {
        Debug.Log($"{_creature.name} exited Idle State.");
        // Additional logic for exiting idle state
    }
}
