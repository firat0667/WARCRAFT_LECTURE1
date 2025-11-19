using AGP_Warcraft;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateManager
{
    private Dictionary<System.Type, IState> _states = new();
    private IState _currentState;

    public IState CurrentState => _currentState;
    public void Initialize(Creature creature)
    {
        _states = new Dictionary<System.Type, IState>
        {
            { typeof(IdleState), new IdleState(creature, this) },
            { typeof(MoveState), new MoveState(creature, this) },
            { typeof(AttackState), new AttackState(creature, this) },
        };
    }
    public void ChangeState<T>() where T : IState
    {
        _currentState?.ExitState();
        _currentState = _states[typeof(T)];
        _currentState.EnterState();
    }

    public void OnUpdate()
    {
        _currentState?.UpdateState();
    }
}
