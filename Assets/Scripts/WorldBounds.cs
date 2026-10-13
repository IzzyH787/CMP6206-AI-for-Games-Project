using UnityEngine;

public class WorldBounds : MonoBehaviour
{
    public static WorldBounds Instance;

    [SerializeField] public Transform MinBound;
    [SerializeField] public Transform MaxBound;

    private void Awake()
    {
        Instance = this;
    }
}
