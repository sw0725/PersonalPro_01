using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemUseChecker : MonoBehaviour
{
    public Action<IInteracable> onItemUse;

    private void OnTriggerEnter(Collider other)
    {
        Transform target = other.transform;
        IInteracable interacable = null;

        do
        {
            interacable = target.GetComponent<IInteracable>();
            target = target.parent;
        } while (interacable == null && target != null);        //���ͷ��� ã�ų� �θ� ������ ����

        if (interacable != null)
        {
            onItemUse?.Invoke(interacable);
        }
    }
}
