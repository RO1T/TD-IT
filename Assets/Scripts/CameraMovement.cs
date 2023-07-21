using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [SerializeField]
    private float cameraSpeed = 20;

    private float xMax;
    private float yMin;

    public void SetLimits(Vector3 maxTile)
    {
        Vector3 wp = Camera.main.ViewportToWorldPoint(new Vector3(1, 0));
        xMax = maxTile.x - wp.x;
        yMin = maxTile.y - wp.y;
    }
    private void GetInput()
    {
        if (Input.GetKey(KeyCode.W))
        {
            transform.Translate(cameraSpeed * Time.deltaTime * Vector3.up);
        }
        if (Input.GetKey(KeyCode.A))
        {
            transform.Translate(cameraSpeed * Time.deltaTime * Vector3.left);
        }
        if (Input.GetKey(KeyCode.S))
        {
            transform.Translate(cameraSpeed * Time.deltaTime * Vector3.down);
        }
        if (Input.GetKey(KeyCode.D))
        {
            transform.Translate(cameraSpeed * Time.deltaTime * Vector3.right);
        }

        transform.position = new Vector3(Mathf.Clamp(transform.position.x, 0, xMax), Mathf.Clamp(transform.position.y, yMin, 0), -10);
    }
    public void Update()
    {
        GetInput();
    }
}
