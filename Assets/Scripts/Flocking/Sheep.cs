using UnityEngine;

public class Sheep : MonoBehaviour
{
    public float flightZoneRadius;

    // Algorithm from paper: https://www.diva-portal.org/smash/get/diva2:675990/FULLTEXT01.pdf
    private float predatorMultiplier(float _distToPredator)
    {
        return ((1 / Mathf.PI) * Mathf.Atan((flightZoneRadius - _distToPredator) / 20)) + 0.5f;
    }

    private float noPreadtorMultiplier()
    {
        return 1; // TO DO: ADD IMPLEMETATION
    }

    private float combinedMultiplier()
    {
        return noPreadtorMultiplier() * (1f + predatorMultiplier(1)); // TO DO: REPLACE PLACEOLDER 1
    }
    private float cMultiplier()
    {
        return 0;
    }
    private float aMultiplier()
    {
        return 0;
    }
    private float sMultiplier()
    {
        return 0;
    }
    private float eMultiplier()
    {
        return 0;
    }

    private Vector3 CohesionVector()
    {
        return Vector3.zero;
    }
    private Vector3 AlignmentVector()
    {
        return Vector3.zero;
    }
    private Vector3 SeparationVector()
    {
        return Vector3.zero;
    }
    private Vector3 EscapeVector()
    {
        return Vector3.zero;
    }
    private Vector3 MoveVector()
    {
        return cMultiplier() * CohesionVector() +
            aMultiplier() * AlignmentVector() +
            sMultiplier() * SeparationVector() +
            eMultiplier() * EscapeVector();
    }
}
