using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMiniGameController : MonoBehaviour
{
    private float t;
    private Vector3 startPosition;
    private Vector3 target;
    private readonly float timeToReachTarget = 5f;
    // Start is called before the first frame update
    void Start()
    {
        startPosition = transform.position;
        target = new Vector3(-13f, transform.position.y, transform.position.z);
    }

    void Update()
    {
        if (transform.position != target && DistanceController.isFirstHalfCompleted)
        {
            t += Time.deltaTime / (timeToReachTarget/2.2f);
            transform.position = Vector3.Lerp(startPosition, target, t);
        }
    }
}
