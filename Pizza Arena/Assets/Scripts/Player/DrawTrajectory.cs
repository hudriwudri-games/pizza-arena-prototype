using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawTrajectory : MonoBehaviour
{
    [SerializeField]
    private LineRenderer lineRenderer;

    [SerializeField]
    [Range(3, 30)]
    private int lineSegmentCount = 20;

    private List<Vector3> linePoints = new List<Vector3>();

    float radianAngle;

    public void DrawLineTrajectory(Vector3 forceVector, Vector3 startingPoint, float projectileMass = 1)
    {
        Vector3 velocity = (forceVector / projectileMass) * Time.fixedDeltaTime;

        float FlightDuration = (2 * velocity.y) / Physics.gravity.y;

        float stepTime = FlightDuration / lineSegmentCount;

        linePoints.Clear();

        for (int i = 0; i < lineSegmentCount; i++)
        {
            float stepTimePassed = stepTime * i;

            Vector3 MovementVector = new Vector3(
                velocity.x * stepTimePassed,
                velocity.y * stepTimePassed - 0.5f * Physics.gravity.y * stepTimePassed * stepTimePassed,
                velocity.z * stepTimePassed
            );

            linePoints.Add(-MovementVector + startingPoint);
        }

        lineRenderer.positionCount = linePoints.Count;
        lineRenderer.SetPositions(linePoints.ToArray());
    }

    public void HideLine()
    {
        lineRenderer.positionCount = 0;
    }
}
