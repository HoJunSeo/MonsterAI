using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class _SpearSkill : MonoBehaviour
{
    private void Update()
    {
        Invoke("SpearUp", 2);
    }


    void SpearUp()
    {
        transform.position = new Vector3(transform.position.x, 0, transform.position.z);
    }
}
