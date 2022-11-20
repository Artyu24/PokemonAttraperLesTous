using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameState
{
    Menu,
    Adventure,
    Fight,
    Paused,
    End
}

public enum PlayerState
{
    PlayerStartMove,
    PlayerInMovement,
    PlayerEnterTP,
    PlayerLeaveTP,
    PlayerWaterInteraction,
    PlayerInFight
}
public enum CombatState
{
    Init,
    PlayerChoose,
    EnemyChoose,
    PlayerAttack,
    EnemyAttack,
    PlayerDeath,
    EnemyDeath,
    PlayerVictory,
    EnemyVictory,
    End
}
