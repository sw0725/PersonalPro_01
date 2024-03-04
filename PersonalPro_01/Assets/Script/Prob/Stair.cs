using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stair : MonoBehaviour, IInteracable
{
    Transform goPoint;
    Player player;

    private void Awake()
    {
        goPoint = transform.GetChild(0).GetChild(0);
    }

    private void Start()
    {
        player = GameManager.Instance.Player;
    }

    public void OnTriggerStay(Collider other)
    {
        //Àü±¤ÆÇ = F to Use
    }

    public void Use()
    {
        player.transform.position = goPoint.position;
    }

}
