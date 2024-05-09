using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tetromino : MonoBehaviour
{
    float fall = 0;
    public float fallSpeed = 1;

    public bool allowRotation = true;
    public bool limitRotation = false;

    public int individualScore = 100;

    public float individualScoreTime;

    public AudioClip moveSound;
    public AudioClip rotateSound;
    public AudioClip landSound;

    private AudioSource audioSource;

    private float continuousVerticalSpeed = 0.05f;
    private float continuousHorizontalSpeed = 0.1f;
    private float buttonDownWaitMax = 0.2f;

    private float verticalTimer = 0;
    private float horizontalTimer = 0;
    private float buttonDownWaitTimerVertical = 0;
    private float buttonDownWaitTimerHorizontal = 0;

    private bool moveImmediateHorizontal = false;
    private bool moveImmediateVertical = false;



    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }
    void Update()
    {
        CheckUserInput();
        UpdateIndividualScore();
    }
    void  UpdateIndividualScore()
    {
        if (individualScoreTime < 1)
        {
            individualScoreTime += Time.deltaTime;
        }
        else
        {
            individualScoreTime = 0;
            individualScore = Mathf.Max(individualScore - 10, 0);
        }
    }
    void CheckUserInput()
    {
        if (Input.GetKeyUp(KeyCode.LeftArrow) || Input.GetKeyUp(KeyCode.RightArrow))
        {
            moveImmediateHorizontal = false;
            horizontalTimer = 0;
            buttonDownWaitTimerHorizontal = 0;
        }

        if (Input.GetKeyUp(KeyCode.DownArrow))
        {
            moveImmediateVertical = false;
            verticalTimer = 0;
            buttonDownWaitTimerVertical = 0;
        }

        if (Input.GetKey(KeyCode.RightArrow))
        {
            MoveRight();
        }

        if (Input.GetKey(KeyCode.LeftArrow))
        {
            MoveLeft();
        }

        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            Rotate();
        }

        if (Input.GetKey(KeyCode.DownArrow) || Time.time - fall >= fallSpeed)
        {
            MoveDown();
        }

        if (Time.time - fall >= fallSpeed)
        {
            MoveDown();
        }

        if (Input.GetKeyUp(KeyCode.Space))
        {
            SlamDown();
        }
    }

    public void SlamDown()
    {
        while (CheckIsValidPosition())
        {
            transform.position += new Vector3(0, -1, 0);
        }

        if (!CheckIsValidPosition())
        {
            transform.position += new Vector3(0, 1, 0);
            FindObjectOfType<Game>().UpdateGrid(this);
        }

        FindObjectOfType<Game>().DeleteRow();

        if (FindObjectOfType<Game>().CheckIsAboveGrid(this))
        {
            FindObjectOfType<Game>().GameOver();
        }
        PlayLandSound();
        FindObjectOfType<Game>().SpawnNextTetromino();

        FindObjectOfType<GameManager>().currentScore += individualScore;

        enabled = false;

        tag = "Untagged";
    }

    void MoveRight()
    {
        if (moveImmediateHorizontal)
        {
            if (buttonDownWaitTimerHorizontal < buttonDownWaitMax)
            {
                buttonDownWaitTimerHorizontal += Time.deltaTime;
                return;
            }

            if (horizontalTimer < continuousHorizontalSpeed)
            {
                horizontalTimer += Time.deltaTime;
                return;
            }
        }

        if (!moveImmediateHorizontal)
            moveImmediateHorizontal = true;

        horizontalTimer = 0;

        transform.position += new Vector3(1, 0, 0);

        if (CheckIsValidPosition())
        {
            FindObjectOfType<Game>().UpdateGrid(this);
            PlayMoveSound();
        }
        else
        {
            transform.position += new Vector3(-1, 0, 0);
        }
    }

    void MoveLeft()
    {
        if (moveImmediateHorizontal)
        {
            if (buttonDownWaitTimerHorizontal < buttonDownWaitMax)
            {
                buttonDownWaitTimerHorizontal += Time.deltaTime;
                return;
            }

            if (horizontalTimer < continuousHorizontalSpeed)
            {
                horizontalTimer += Time.deltaTime;
                return;
            }
        }

        if (!moveImmediateHorizontal)
            moveImmediateHorizontal = true;

        horizontalTimer = 0;

        transform.position += new Vector3(-1, 0, 0);

        if (CheckIsValidPosition())
        {
            FindObjectOfType<Game>().UpdateGrid(this);
            PlayMoveSound();
        }
        else
        {
            transform.position += new Vector3(1, 0, 0);
        }
    }

    void MoveDown()
    {
        if (moveImmediateVertical)
        {
            if (buttonDownWaitTimerVertical < buttonDownWaitMax)
            {
                buttonDownWaitTimerVertical += Time.deltaTime;
                return;
            }

            if (verticalTimer < continuousVerticalSpeed)
            {
                verticalTimer += Time.deltaTime;
                return;
            }
        }

        if (!moveImmediateVertical)
            moveImmediateVertical = true;

        verticalTimer = 0;

        transform.position += new Vector3(0, -1, 0);

        if (CheckIsValidPosition())
        {
            FindObjectOfType<Game>().UpdateGrid(this);

            if (Input.GetKey(KeyCode.DownArrow))
            {
                PlayMoveSound();
            }
        }
        else
        {
            transform.position += new Vector3(0, 1, 0);

            FindObjectOfType<Game>().DeleteRow();

            if (FindObjectOfType<Game>().CheckIsAboveGrid(this))
            {
                FindObjectOfType<Game>().GameOver();
            }
            PlayLandSound();
            FindObjectOfType<Game>().SpawnNextTetromino();

            FindObjectOfType<GameManager>().currentScore += individualScore;

            enabled = false;

            tag = "Untagged";
        }

        fall = Time.time;
    }

    void Rotate()
    {
        if (allowRotation)
        {
            if (limitRotation)
            {
                if (transform.rotation.eulerAngles.z >= 90)
                {
                    transform.Rotate(0, 0, -90);
                }
                else
                {
                    transform.Rotate(0, 0, 90);
                }
            }
            else
            {
                transform.Rotate(0, 0, 90);
            }
            if (CheckIsValidPosition())
            {
                FindObjectOfType<Game>().UpdateGrid(this);

                PlayRotateSound();
            }
            else
            {
                if (limitRotation)
                {
                    if (transform.rotation.eulerAngles.z >= 90)
                    {
                        transform.Rotate(0, 0, -90);
                    }
                    else
                    {
                        transform.Rotate(0, 0, 90);
                    }
                }
                else
                {
                    transform.Rotate(0, 0, -90);
                }

            }

        }
    }

    void PlayMoveSound()
    {
        if (moveSound)
        {
            audioSource.PlayOneShot(moveSound);
        }
    }

    void PlayRotateSound()
    {
        if (rotateSound)
        {
            audioSource.PlayOneShot(rotateSound);
        }
    }

    void PlayLandSound()
    {
        if (landSound)
        {
            audioSource.PlayOneShot(landSound);
        }
    }


    bool CheckIsValidPosition()
    {
        foreach (Transform mino in transform)
        {
            Vector2 pos = FindObjectOfType<Game>().Round(mino.position);
            if (FindObjectOfType<Game>().CheckIsInsideGrid(pos) == false)
            {
                return false;
            }
            if (FindObjectOfType<Game>().GetTransformFromGridPosition(pos) != null && FindObjectOfType<Game>().GetTransformFromGridPosition(pos).parent != transform)
            {
                return false;
            }
        }
        return true;
    }
}
