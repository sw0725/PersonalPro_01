using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crosshairs : MonoBehaviour
{
    RectTransform crosshairs;

    private void Awake()
    {
        crosshairs = GetComponent<RectTransform>();
    }

    private void Start()
    {
        Player player = GameManager.Instance.Player;
        player.OnShoot += RayCasting;

        GameManager.Instance.GameEnding += GameEnd;
        GameManager.Instance.GameStarting += GameStart;
    }

    void GameStart() 
    {
        Cursor.visible = false;
    }

    void GameEnd() 
    {
        Cursor.visible = true;
    }

    private void Update()
    {
        MousePointPosition();
    }

    void MousePointPosition() 
    {
        Vector2 mousePos = Input.mousePosition;
        crosshairs.position = mousePos;
    }

    void RayCasting() 
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(crosshairs.position);
        if (Physics.Raycast(ray, out hit)) 
        {
            EnemyBace enemy = hit.transform.GetComponent<EnemyBace>();
            if(enemy != null) 
            {
                enemy.OnDemage();
                HitEffect a = Factory.Instance.GetHitEffect(hit.point);
            }
        }
    }

    
}
