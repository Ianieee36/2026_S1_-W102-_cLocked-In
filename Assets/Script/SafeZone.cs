using UnityEngine;
using System.Collections.Generic;

public class SafeZone : MonoBehaviour
{
    public static SafeZone Instance { get; private set; }

    private readonly HashSet<Collider2D> _occupants = new();

    void Awake() => Instance = this;

    void OnTriggerEnter2D(Collider2D other) => _occupants.Add(other);
    void OnTriggerExit2D(Collider2D other) => _occupants.Remove(other);

    public bool IsInside(Collider2D col) => _occupants.Contains(col);
}
