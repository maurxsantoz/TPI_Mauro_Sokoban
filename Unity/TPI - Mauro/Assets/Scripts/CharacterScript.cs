using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterScript : MonoBehaviour
{
    Animator animator;
    float runningTime = 0.25f;
    float runningTimer = 0;
    bool isRunning = false;
    float verticalInput = 0;
    float horizontalInput = 0;
    [SerializeField]
    List<GameObject> colliders = new List<GameObject>();
    Collider northCollision, southCollision, westCollision, eastCollision;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        Mouvement();
    }
    void Mouvement()
    {
        if (isRunning)
        {
            transform.position = new Vector3(transform.position.x + verticalInput * Time.deltaTime / runningTime, transform.position.y, transform.position.z + horizontalInput * Time.deltaTime / runningTime);
            if (runningTimer>runningTime*0.85)
            {
                SetColliders(true);
            }

            if (runningTime < runningTimer)
            {
                transform.position = new Vector3(Mathf.Round(transform.position.x-0.5f)+0.5f, transform.position.y, Mathf.Round(transform.position.z / 0.5f) * 0.5f);
                isRunning = false;
                SetColliders(true);
            }
        }
        else
        {
            float verticalAxis = Input.GetAxis("Vertical"), horizontalaxis = Input.GetAxis("Horizontal");
            if (Mathf.Abs(verticalAxis) > Mathf.Abs(horizontalaxis) & Mathf.Abs(verticalAxis) > 0.2)
            {
                if (verticalAxis > 0)
                {
                    transform.eulerAngles = new Vector3(transform.eulerAngles.x, 90, transform.eulerAngles.z);
                    if (northCollision == null)
                    {
                        verticalInput = 1;
                        horizontalInput = 0;
                        validMove();
                    }
                    else if (northCollision.gameObject.layer == 7)
                    {
                        if (northCollision.gameObject.GetComponent<BoxScript>().Push("North"))
                        {
                            verticalInput = 1;
                            horizontalInput = 0;
                            validMove();
                        }
                    }

                }
                else
                {
                    transform.eulerAngles = new Vector3(transform.eulerAngles.x, 270, transform.eulerAngles.z);
                    if (southCollision == null)
                    {
                        verticalInput = -1;
                        horizontalInput = 0;
                        validMove();
                    }
                    else if (southCollision.gameObject.layer == 7)
                    {
                        if (southCollision.gameObject.GetComponent<BoxScript>().Push("South"))
                        {
                            verticalInput = -1;
                            horizontalInput = 0;
                            validMove();
                        }
                    }
                }
            }
            if (Mathf.Abs(verticalAxis) < Mathf.Abs(horizontalaxis) & Mathf.Abs(horizontalaxis) > 0.2)
            {
                if (horizontalaxis > 0)
                {
                    transform.eulerAngles = new Vector3(transform.eulerAngles.x, 180, transform.eulerAngles.z);
                    if (eastCollision == null)
                    {
                        horizontalInput = -1;
                        verticalInput = 0;
                        validMove();
                    }
                    else if (eastCollision.gameObject.layer == 7)
                    {
                        if (eastCollision.gameObject.GetComponent<BoxScript>().Push("East"))
                        {
                            horizontalInput = -1;
                            verticalInput = 0;
                            validMove();
                        }
                    }
                }
                else
                {
                    transform.eulerAngles = new Vector3(transform.eulerAngles.x, 0, transform.eulerAngles.z);
                    if (westCollision == null)
                    {
                        horizontalInput = 1;
                        verticalInput = 0;
                        validMove();
                    }
                    else if (westCollision.gameObject.layer == 7)
                    {
                        if (westCollision.gameObject.GetComponent<BoxScript>().Push("West"))
                        {
                            horizontalInput = 1;
                            verticalInput = 0;
                            validMove();
                        }
                    }
                }
                verticalInput = 0;
            }
        }
        runningTimer += Time.deltaTime;
    }
    void validMove()
    {
        SetColliders(false);
        animator.SetTrigger("IsRunning");
        isRunning = true;
        runningTimer = 0;
    }
    void SetColliders(bool status)
    {
        foreach(GameObject collider in colliders)
        {
            collider.SetActive(status);
        }
        if (!status)
        {
            northCollision = null;
            southCollision = null;
            westCollision = null;
            eastCollision = null;
        }
    }
    public void AddCollision(string side, Collider item)
    {

        float rotation = 0;
        switch (side)
        {
            case "Left":
                rotation = 270;
                break;
            case "Right":
                rotation = 90;
                break;
            case "Front":
                rotation = 0;
                break;
            case "Back":
                rotation = 180;
                break;

        }
        rotation = Mathf.RoundToInt((rotation + transform.localRotation.eulerAngles.y) / 90) * 90;
        switch (rotation%360)
        {
            case 270:
                southCollision = item;
                break;
            case 90:
                northCollision = item;
                break;
            case 0:
                westCollision = item;
                break;
            case 180:
                eastCollision = item;
                break;
        }

    }
}