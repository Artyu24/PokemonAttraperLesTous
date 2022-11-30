using System;
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
    Interaction,
    Inventory,
    ForcedMove,
    Map,
    WaterInteraction
}

public enum PNJState
{
    Idle,
    InMovement,
    Interaction
}

public enum CombatState
{
    Init,
    CallButton,
    PlayerChoose,
    UsePotion,
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
    UP,
    DOWN,
    LEFT,
    RIGHT,
    IDLE,
    SEE_HAUT,
    SEE_BAS,
    SEE_GAUCHE,
    SEE_DROITE,
    NOTHING
}

public enum DialogueState
{
    INTERACTION,
    INTERACTION_ACTION,
    DRAW,
    DRAW_ACTION
}

[Serializable]
public struct DialogueID
{
    public int lineId;
    public int columnId;
}