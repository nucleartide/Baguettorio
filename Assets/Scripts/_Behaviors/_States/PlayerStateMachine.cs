using UnityEngine;

public class PlayerStateMachine : StateMachineBehaviour
{
    [SerializeField]
    [NotNull]
    private GameInputManager gameInput;

    [SerializeField]
    [NotNull]
    private ImmediateCollectableManager immediateCollectable;

    [SerializeField]
    [NotNull]
    private PlayerMiningState playerMiningState;

    [SerializeField]
    [NotNull]
    private PlayerChoppingState playerChoppingState;

    [SerializeField]
    [NotNull]
    private PlayerMovingState playerMovingState;

    [SerializeField]
    [NotNull]
    private PlayerScoopingState playerScoopingState;

    private void OnEnable()
    {
        gameInput.OnCollectStarted += GameInput_OnCollectStarted;
        gameInput.OnCollectCanceled += GameInput_OnCollectCanceled;
    }

    private void OnDisable()
    {
        gameInput.OnCollectStarted -= GameInput_OnCollectStarted;
        gameInput.OnCollectCanceled -= GameInput_OnCollectCanceled;
    }

    private void GameInput_OnCollectStarted(object sender, GameInputManager.GameInputArgs args)
    {
        var resource = immediateCollectable.ImmediateCollectable;
        if (resource == null)
            throw new System.Exception("TODO: Jason add in a 'null' sound here.");

        // Grab some data.
        var config = resource.ResourceConfiguration;
        var stateName = config.PlayerStateEnum.State.name;

        // Given a Resource, fetch the corresponding player state.
        PlayerCollectingState collectingState;
        if (stateName == playerMiningState.GetType().Name)
            collectingState = playerMiningState;
        else if (stateName == playerChoppingState.GetType().Name)
            collectingState = playerChoppingState;
        else if (stateName == playerScoopingState.GetType().Name)
            collectingState = playerScoopingState;
        else
            throw new System.Exception($"State name {stateName} does not have a corresponding PlayerCollectingState. Please inspect the source code to figure out what's going on.");

        // Perform state transition.
        collectingState.Initialize(resource);
        TransitionTo(collectingState);
    }

    private void GameInput_OnCollectCanceled(object sender, GameInputManager.GameInputArgs args)
    {
        TransitionTo(playerMovingState);
    }
}
