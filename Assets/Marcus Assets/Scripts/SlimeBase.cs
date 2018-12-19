﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeBase : MonoBehaviour {

    public int m_MaxHealth; // max health
    public int m_MaxAttack; // max attack

    private int m_Health; // health
    private int m_Attack; // attack

    public Animator anim;

    public GameObject m_DeathParticles;

    enum States
    {
        Wait,
        Walk,
        Attack,
        Defend,
    }

    private States currState;
    private States nextState;

    [HideInInspector]
    public bool toDespawn;

    // Use this for initialization
    void Start() {
        toDespawn = false;

        m_Health = m_MaxHealth - 1;
        m_Attack = m_MaxAttack;
    }

    public void DeductHealth(int toDeduct)
    {
        m_Health -= toDeduct;
    }

    public int GetHealth()
    {
        return m_Health;
    }
}
