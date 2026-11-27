using UnityEngine;

public class Sheep : MonoBehaviour
{
    [Header("Movement")]
    public float minSpeed = 0f;
    public float maxWalkSpeed = 3f;
    public float maxRunSpeed = 7f;

    public float rotationSpeed = 30f;

    [Header("Detection")]
    public float alignmentThreshold = 10f;
    public float flightZoneRadius = 12f;
    public float separationThreshold = 6f;

    [Header("Multipliers")]
    public float baseCohesionMultiplier = 1;
    public float baseAlignmentMultiplier = .3f;
    public float baseSeparationMultiplier = .7f;
    public float baseEscapeMultiplier = 1;

    public float predatorCohesionMultiplier = 1;
    public float predatorAlignmentMultiplier = .2f;
    public float predatorSeparationMultiplier = .4f;

    [Header("Components")]
    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        Vector3 movement = MoveVector();

        // Rotate towards position
        gameObject.transform.rotation = Quaternion.Lerp(gameObject.transform.rotation,
            Quaternion.LookRotation(movement),
            rotationSpeed * Time.deltaTime);

        // Add a bit of random into sheep movement to make more natural
        movement *= Random.Range(0.8f, 1.2f);
        // Set velocity of sheep 
        rb.linearVelocity = new Vector3(movement.x, rb.linearVelocity.y, movement.z);
    }

    // Algorithm from paper: https://www.diva-portal.org/smash/get/diva2:675990/FULLTEXT01.pdf
    private float predatorMultiplier()
    {
        float distToPredator = Vector3.Distance(gameObject.transform.position, SheepFlockManager.Instance.predator.transform.position);
        return ((1 / Mathf.PI) * Mathf.Atan((flightZoneRadius - distToPredator) / 20f)) + 0.5f;
    }
    private float cohesionMultiplier()
    {
        return baseCohesionMultiplier * (1 + (predatorMultiplier() * predatorCohesionMultiplier));
    }
    private float alignmentMultiplier()
    {
        return baseAlignmentMultiplier * (1 + (predatorMultiplier() * predatorAlignmentMultiplier)) ;
    }
    private float separationMultiplier()
    {
        return baseSeparationMultiplier * (1 + (predatorMultiplier() * predatorSeparationMultiplier));
    }
    private float escapeMultiplier()
    {
        return baseEscapeMultiplier;
    }

    private Vector3 CohesionVector()
    {
        int groupSize = 0;
        Vector3 centreOfFlock = Vector3.zero;

        foreach (GameObject _sheep in SheepFlockManager.Instance.sheepInScene) 
        {
            if (_sheep == gameObject)
                continue; // Skip if looking at this sheep

            centreOfFlock += _sheep.transform.position;
            groupSize++;
        }

        // Avoid potential divide by 0 (could happen if all sheep in scene die)
        if (groupSize == 0)
            return Vector3.zero;

        centreOfFlock /= groupSize; // Will get position in centre of all sheep in scene

        Vector3 direction =  Utilities.CalculateDirectionToObject(gameObject, centreOfFlock); // Returns normalised vector of direction to centre
        direction.y = 0; // Prevent vertical movement
        return direction;
    }
    private Vector3 AlignmentVector()
    {
        Vector3 alignmentVector = Vector3.zero;

        foreach(GameObject _sheep in SheepFlockManager.Instance.sheepInScene)
        {
            if (_sheep == gameObject)
                continue;

            if (Vector3.Distance(_sheep.transform.position, gameObject.transform.position) <= alignmentThreshold)
            {
                Vector3 sheepV = _sheep.GetComponent<Rigidbody>().linearVelocity;
                alignmentVector += new Vector3(sheepV.x, 0, sheepV.z);
            }
        }

        return alignmentVector;
    }
    private Vector3 SeparationVector()
    {
        Vector3 separationVector = Vector3.zero;

        foreach(GameObject _sheep in SheepFlockManager.Instance.sheepInScene)
        {
            if (_sheep == gameObject) // SKip if current sheeep = this sheep
                continue;

            // Check if sheep is close enoough that separation shuold be applied
            if (Vector3.Distance(_sheep.transform.position, gameObject.transform.position) <= separationThreshold)
            {
                Vector3 sp = gameObject.transform.position;
                Vector3 sip = _sheep.transform.position;

                float magnitude = Vector3.Magnitude(sp - sip);
                // Algorithm from paper section 3.1.4: https://www.diva-portal.org/smash/get/diva2:675990/FULLTEXT01.pdf
                separationVector += (((sp - sip).normalized) * InverseSquare(magnitude, 1));
            }
        }
        separationVector.y = 0;
        return separationVector;
    }
    private Vector3 EscapeVector()
    {
        // Algorithm from paper section 3.1.6: https://www.diva-portal.org/smash/get/diva2:675990/FULLTEXT01.pdf
        Vector3 sp = gameObject.transform.position;
        Vector3 pp = SheepFlockManager.Instance.predator.transform.position;

        float magnitude = Vector3.Magnitude(sp - pp);

        Vector3 escapeVector = ( (sp - pp).normalized ) * InverseSquare(magnitude, 10);

        escapeVector.y = 0;
        return escapeVector;
    }
    private Vector3 MoveVector()
    {
        Vector3 movement =  (cohesionMultiplier() * CohesionVector()) +
            (alignmentMultiplier() * AlignmentVector()) +
            (separationMultiplier() * SeparationVector()) +
            (escapeMultiplier() * EscapeVector());

        movement.y = 0;
        return Vector3.ClampMagnitude(movement, maxRunSpeed);
    }
    private float InverseSquare(float _distanceBetweenObjects, float _softnessFactor) 
    {
        float epsilon = 0.1f; // Small value used to avoid divide by 0, when x = 0
        return Mathf.Pow(( (_distanceBetweenObjects / _softnessFactor) + epsilon ), -2);
    }
}
