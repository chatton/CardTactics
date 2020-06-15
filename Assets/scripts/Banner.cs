using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Banner : MonoBehaviour
{

    private void LookAtMainCamera() {
        transform.LookAt(Camera.main.transform);
        transform.rotation = Camera.main.transform.rotation;
    }

    private void Awake()
    {
        LookAtMainCamera();
    }

    private void LateUpdate()
    {
        LookAtMainCamera();
    }
    void Update()
    {
        LookAtMainCamera();
    }
}
