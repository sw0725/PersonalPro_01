using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public interface IInteracable 
{
    void Use();

    void OnTriggerStay(Collider other);
}
