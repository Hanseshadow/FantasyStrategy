using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LerpOverTime : MonoBehaviour
{
    public float LerpLength = 1f;

    float LerpTime = 0f;

    Vector3 LocationStart;
    Vector3 CurrentLocation;
    Vector3 TargetLocation;
    bool IsLerping = false;

    GameManager Game;

    private void Start()
    {
        Game = FindObjectOfType<GameManager>();
    }

    void Update()
    {
        if(!IsLerping || Game == null || Game.IsGamePaused)
            return;

        LerpTime += Time.deltaTime;

        gameObject.transform.position = Vector3.Lerp(LocationStart, TargetLocation, LerpTime / LerpLength);

        if(LerpTime / LerpLength >= LerpLength)
        {
            LerpCompleted();
        }
    }

    public void StartLerp()
    {
        IsLerping = true;
        LerpTime = 0f;
    }

    public void LerpCompleted()
    {
        IsLerping = false;
    }

    public void SetLocation()
    {
        SetLocation(transform.position);
    }

    public void SetLocation(Vector3 location)
    {
        LocationStart = location;
    }

    public void SetTarget(GameObject target)
    {
        SetTarget(gameObject.transform.position);
    }

    public void SetTarget(Vector3 target)
    {
        TargetLocation = target;
    }

    public void SetHalfTarget()
    {
        // TODO: Test this
        TargetLocation = ((TargetLocation - LocationStart) * 0.5f) + LocationStart;
    }
}
