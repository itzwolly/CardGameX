﻿using System;
using UnityEngine;

public class DealDamageToOpponent : Action {
    public override void OnEnter() {
        Debug.Log("Dealing damage to x");
    }

    public override void OnExit() {
        Debug.Log("Calling OnExit in: " + this);
    }

    public override void OnStay() {
        Debug.Log("Calling OnStay in: " + this);
    }
}
