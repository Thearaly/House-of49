
using System;
using System.Collections;
using UnityEngine;
using TMPro;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(BoxCollider))]
public class DiceRoller : MonoBehaviour
{
    [Header("Game Manager")]
    public RPSBoardManager boardManager;

    [Header("UI")]
    public TMP_Text diceResultText;

    [Header("Dice Faces")]
    public Transform face1;
    public Transform face2;
    public Transform face3;
    public Transform face4;
    public Transform face5;
    public Transform face6;

    [Header("Rolling Settings")]
    public float upwardForce = 3f;
    public float rollingForce = 5f;
    public float torqueForce = 8f;
    public float stopSpeed = 0.08f;
    public float stopTime = 0.5f;
    public float maxRollTime = 8f;

    private Rigidbody rb;
    private bool isRolling;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    void OnMouseDown()
{
    Debug.Log("DICE CLICKED!");

    if (boardManager != null)
    {
        boardManager.RollDice();
    }
    else
    {
        Debug.LogWarning("Please assign RPSBoardManager!");
    }
}

    public void Roll(Action<int> onComplete)
    {
        if (isRolling)
            return;

        if (rb.isKinematic)
        {
            Debug.LogWarning("Dice Rigidbody must not be Kinematic!");
            onComplete?.Invoke(0);
            return;
        }

        Transform[] faces =
        {
            face1, face2, face3,
            face4, face5, face6
        };

        foreach (Transform face in faces)
        {
            if (face == null)
            {
                Debug.LogError("Assign all six dice face markers!");
                onComplete?.Invoke(0);
                return;
            }
        }

        StartCoroutine(RollDice(onComplete));
    }

    IEnumerator RollDice(Action<int> onComplete)
    {
        isRolling = true;

        if (diceResultText != null)
            diceResultText.text = "Rolling...";

        rb.WakeUp();

        Vector3 randomDirection = new Vector3(
            UnityEngine.Random.Range(-1f, 1f),
            0f,
            UnityEngine.Random.Range(-1f, 1f)
        ).normalized;

        rb.AddForce(
            Vector3.up * upwardForce,
            ForceMode.Impulse
        );

        rb.AddForce(
            randomDirection * rollingForce,
            ForceMode.Impulse
        );

        rb.AddTorque(
            UnityEngine.Random.insideUnitSphere * torqueForce,
            ForceMode.Impulse
        );

        float elapsedTime = 0f;
        float stableTime = 0f;

        while (elapsedTime < maxRollTime)
        {
            yield return new WaitForFixedUpdate();

            elapsedTime += Time.fixedDeltaTime;

            bool stable =
                rb.linearVelocity.magnitude < stopSpeed &&
                rb.angularVelocity.magnitude < stopSpeed;

            if (stable)
            {
                stableTime += Time.fixedDeltaTime;

                if (stableTime >= stopTime)
                    break;
            }
            else
            {
                stableTime = 0f;
            }
        }

        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        rb.Sleep();

        int result = GetTopFace();

        if (diceResultText != null)
            diceResultText.text = "Dice: " + result;

        Debug.Log("Dice rolled: " + result);

        isRolling = false;

        onComplete?.Invoke(result);
    }

    int GetTopFace()
    {
        Transform[] faces =
        {
            face1, face2, face3,
            face4, face5, face6
        };

        int result = 1;
        float highestDot = -1f;

        for (int i = 0; i < faces.Length; i++)
        {
            float dot = Vector3.Dot(
                faces[i].forward,
                Vector3.up
            );

            if (dot > highestDot)
            {
                highestDot = dot;
                result = i + 1;
            }
        }

        return result;
    }

    
}
