using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spin : MonoBehaviour
{


    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0, 0, 1300 * Time.deltaTime);
    }
}
