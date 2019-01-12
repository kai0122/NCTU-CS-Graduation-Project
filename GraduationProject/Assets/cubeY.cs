using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cubeY : MonoBehaviour {
    public Camera camera_eye;
    public AvatarController m_avatarController;

    float maxJumpHeight = 3.0f;
    float groundHeight;
    Vector3 groundPos;
    float jumpSpeed = 7.0f;
    float fallSpeed = 7.0f;
    public bool inputJump = false;
    public bool grounded = true;

    void Start()
    {
        groundPos = transform.position;
        groundHeight = transform.position.y;
        maxJumpHeight = transform.position.y + maxJumpHeight;
    }

    void Update()
    {   
        gameObject.transform.position = new Vector3(camera_eye.transform.position.x, gameObject.transform.position.y, camera_eye.transform.position.z);
        if (gameObject.transform.position.y < 0) gameObject.transform.position = new Vector3(gameObject.transform.position.x, 0, gameObject.transform.position.z);

        if (Input.GetKey(KeyCode.Space) || (m_avatarController.ifLeftHandGrip && m_avatarController.ifRightHandGrip))
        {
            gameObject.transform.position = new Vector3(camera_eye.transform.position.x, 20f, camera_eye.transform.position.z);
        }
        transform.Translate(Vector3.down * fallSpeed * Time.smoothDeltaTime);
    }

    IEnumerator Jump()
    {
        while (true)
        {
            if (transform.position.y >= maxJumpHeight)
                inputJump = false;
            if (inputJump)
                transform.Translate(Vector3.up * jumpSpeed * Time.smoothDeltaTime);
            else if (!inputJump)
            {
                
                if (transform.position.y < groundPos.y)
                {
                    transform.position = groundPos;
                    StopAllCoroutines();
                }
            }

            yield return new WaitForEndOfFrame();
        }
    }
}
