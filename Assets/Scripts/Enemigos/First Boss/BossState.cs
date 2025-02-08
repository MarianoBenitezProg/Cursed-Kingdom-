using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface BossState
{
      void EnterState(FirstBoss boss);
      void UpdateState(FirstBoss boss);
      void ExitState(FirstBoss boss);
}

