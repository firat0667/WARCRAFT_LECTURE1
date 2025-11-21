using AGP_Warcraft;

public class MoveState : IState
{
    private Creature _creature;
    private StateManager _state;

    public MoveState(Creature creature, StateManager state)
    {
        _creature = creature;
        _state = state;
    }

    public void EnterState()
    {
        // PLAYER PATH ALREADY SET
        // AI PATH alýnacaksa sadece AI modunda yapýlýr:
        if (!_creature.IsControlledByPlayer && _creature.Target != null)
            _creature.CalculatePathTo(_creature.Target);
    }

    public void UpdateState()
    {
        _creature.ProcessActions();

        // PLAYER MOVE FINISHED
        if (_creature.IsControlledByPlayer)
        {
            if (_creature.HasPlayerCommand && _creature.CurrentPath.Count == 0)
            {
                _creature.HasPlayerCommand = false;
                _state.ChangeState<IdleState>();
                return;
            }

            return;
        }

        // AI
        if (_creature.InAttackRange(_creature.Target))
        {
            _state.ChangeState<AttackState>();
            return;
        }

        if (_creature.CurrentPath.Count == 0)
        {
            _creature.CalculatePathTo(_creature.Target);
        }
    }

    public void ExitState() { }
}
