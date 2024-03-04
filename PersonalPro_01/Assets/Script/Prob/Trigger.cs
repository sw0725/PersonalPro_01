using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trigger : MonoBehaviour
{
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player")) 
        {
            Player player = GameManager.Instance.Player;
            player.GoToStandMode();
        }
    }
}
