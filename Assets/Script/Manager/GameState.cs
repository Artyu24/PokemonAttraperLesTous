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
    Idle,
    InMovement,
    InFight,
    Teleportation,
    WaterInteraction
}
public enum CombatState
{
    Init,
    CallButton,
    PlayerChoose,
    EnemyChoose,
    PlayerAttack,
    EnemyAttack,
    PlayerDeath,
    EnemyDeath,
    Victory,
    End
}

public enum PotentialDirection
{
    HAUT,
    BAS,
    GAUCHE,
    DROITE,
    RIEN
}
