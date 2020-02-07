using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    float speed = 4;

    void Update()
    {
        if (Input.mouseScrollDelta.y != 0)
            Camera.main.transform.Translate(0, 0, Input.mouseScrollDelta.y * 4);
        MoveCamera();
    }

    void MoveCamera()
    {
        float mouseX = Input.mousePosition.x;
        float mouseY = Input.mousePosition.y;
        float camY = Camera.main.transform.position.y;       

        if (mouseX < 0 || Input.GetKey("a"))
            Camera.main.transform.Translate(-speed, 0, 0);
        else if (mouseX >= Screen.width - 5 || Input.GetKey("d"))
            Camera.main.transform.Translate(speed, 0, 0);

        if (mouseY < 0 || Input.GetKey("s"))
            Camera.main.transform.Translate(0, 0, -speed);
        else if (mouseY >= Screen.height || Input.GetKey("w"))
            Camera.main.transform.Translate(0, 0, speed);

        RevertChangeInY(camY);
    }

    void RevertChangeInY(float camY)
    {
        float camX = Camera.main.transform.position.x;
        float camZ = Camera.main.transform.position.z;
        Camera.main.transform.position = new Vector3(camX, camY, camZ);
    }
}
