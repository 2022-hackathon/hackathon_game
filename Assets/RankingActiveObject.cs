using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RankingActiveObject : ActiveObject
{
    public GameObject rankingPanel;

    public override void Active()
    {
        base.Active();
        IngameManager.Instance.rank();
        rankingPanel.SetActive(true);
    }
}
