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
    WaterInteraction
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
    HAUT,
    BAS,
    GAUCHE,
    DROITE,
    RIEN
}

[Serializable]
public struct DialogueID
{
    public int lineId;
    public int columnId;
}