using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crouch : MonoBehaviour, IInteracable
{
    bool canSit = false;
    bool isSit = false;

    Player player;

    private void Start()
    {
        player = GameManager.Instance.Player;
    }

    public void OnTriggerStay(Collider other)
    {
        canSit = true;
        //Àü±¤ÆÇ
    }

    private void OnTriggerExit(Collider other)
    {
        canSit = false;
    }

    public void Use()
    {
        if (canSit) 
        {
            if (!isSit)
            {
                player.GotoSitMode();
            }
            else 
            {
                player.GoToStandMode();
            }
            isSit = !isSit;
        }
    }
}
