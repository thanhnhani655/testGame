using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffDayController : MonoBehaviour
{
    public BuffDay activeBuff;
    [SerializeField]
    private InvadeController invadeController;

    public void SetActiveBuff(BuffDay buff)
    {
        activeBuff = buff;
        Debug.Log("Buff Day Choose: " + buff.buffID);
        switch (buff.buffID)
        {
            case "BUDA_001":
                invadeController.SetInvadeRank(InvaderRank.Tier1);
                break;
            case "BUDA_002":
                invadeController.SetInvadeRank(InvaderRank.Tier2);
                break;
            case "BUDA_003":
                invadeController.SetInvadeRank(InvaderRank.Tier3);
                break;
            case "BUDA_004":
                invadeController.SetInvadeRank(InvaderRank.Tier4);
                break;
        }

    }
}
