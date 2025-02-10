using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface SecondBossState
{
    void EnterState(SecondBoss boss);
    void UpdateState(SecondBoss boss);
    void ExitState(SecondBoss boss);
}

