using UnityEngine;
using TMPro;

public class BossController : MonoBehaviour
{
    public Transform player;
    public float moveSpeed = 1.5f;
    public float chaseSpeed = 4f; // Move faster when chasing

    public float visionRange = 6f; // Vision cone
    public float visionAngle = 60f; // Vision cone
    public float minChaseDistance = 2f; // Stopping distance from player when chasing (just to avoid weird jittering)
    public LayerMask obstacleMask;

    public float detection = 0f; // Detection level (0 to 1)
    public float detectionRate = 1f; // Speed at which detection increases
    public float decayRate = 0.5f; // Speed at which detection decreasess
    public float timeToLose = 5f; // How lon g the player can stay at max detection before losing
    public float detectedTime = 0f; // How long the player has been at max detection

    // To add waypoints for pattrolling add empty game objects in the map scene and tag them with "Waypoint" and name "Waypoint(n)"
    // The boss will move between these points in order and loop back to the start.
    public Transform[] waypoints;
    int currentWaypoint;

    public TextMeshProUGUI detectionText;

    Rigidbody2D rb;

    enum BossState { Patrol, Chase }
    BossState state;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        state = BossState.Patrol;

        // Find player and waypoints in any scene
        player = GameObject.FindGameObjectWithTag("Player").transform;

        GameObject[] wpObjects = GameObject.FindGameObjectsWithTag("Waypoint");

        System.Array.Sort(wpObjects, (a, b) => a.name.CompareTo(b.name));

        waypoints = new Transform[wpObjects.Length];

        for (int i = 0; i < wpObjects.Length; i++)
        {
            waypoints[i] = wpObjects[i].transform;
        }
    }

    void Update()
    {
        UpdateDetection();

        switch (state)
        {
            case BossState.Patrol:
                Patrol();
                break;
            case BossState.Chase:
                Chase();
                break;
        }
    }

    void MoveTo(Vector2 target)
    {
        // Move towards target (either player or waypoint)
        Vector2 dir = (target - (Vector2)transform.position).normalized;
        rb.linearVelocity = dir * moveSpeed;
        
        // Rotate towards movement direction
        RotateTowards(dir);
    }

    void Patrol()
    {
        // Sets target to current waypoint
        Transform target = waypoints[currentWaypoint];

        MoveTo(target.position);

        // If close enough to the waypoint switch to the next one
        if (Vector2.Distance(transform.position, target.position) < 0.2f)
        {
            currentWaypoint = (currentWaypoint + 1) % waypoints.Length;
        }
    }

    void Chase()
    {
        float dist = Vector2.Distance(transform.position, player.position);

        Vector2 dir = (player.position - transform.position).normalized;

        // Stops chasing when really close to avoid weird jittering.
        if (dist > minChaseDistance)
        {
            rb.linearVelocity = dir * chaseSpeed;
            RotateTowards(dir);
        }
        else
        {
            rb.linearVelocity = Vector2.zero;
        }
    }

    void UpdateDetection()
    {
        // Detection logic
        if (CanSeePlayer())
            detection += detectionRate * Time.deltaTime;
        else
            detection -= decayRate * Time.deltaTime;

        detection = Mathf.Clamp01(detection);

        // Detection time
        if (detection >= 0.9f)
        {
            detectedTime += Time.deltaTime;

            if (detectedTime >= timeToLose)
            {
                Time.timeScale = 0f;
                // TODO: Game over logic <----------------------------------------------------
            }
        }
        else
        {
            detectedTime = 0f;
        }

        // Detection state
        if (detection >= 1f)
            state = BossState.Chase;
        else if (detection <= 0f)
            state = BossState.Patrol;


        // Detection UI Update
        detectionText.text = "Detection: " + Mathf.RoundToInt(detection * 100f) + "%";
        if (detection < 0.3f)
            detectionText.color = Color.green;
        else if (detection < 0.7f)
            detectionText.color = Color.yellow;
        else
            detectionText.color = Color.red;
    }

    bool CanSeePlayer()
    {
        // Check if player is within vision cone and not blocked by obstacles
        Vector2 dir = (player.position - transform.position).normalized;

        float angle = Vector2.Angle(transform.right, dir);
        if (angle > visionAngle / 2f) return false;

        float dist = Vector2.Distance(transform.position, player.position);
        if (dist > visionRange) return false;

        RaycastHit2D hit = Physics2D.Raycast(transform.position, dir, visionRange, obstacleMask);
        if (hit.collider != null && hit.collider.transform != player) return false;

        return true;
    }

    void RotateTowards(Vector2 dir)
    {
        if (dir.sqrMagnitude < 0.001f) return;

        float targetAngle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        float angle = Mathf.LerpAngle(transform.eulerAngles.z, targetAngle, 10f * Time.deltaTime);
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }

    Vector3 DirFromAngle(float angle)
    {
        float rad = (angle + transform.eulerAngles.z) * Mathf.Deg2Rad;
        return new Vector3(Mathf.Cos(rad), Mathf.Sin(rad));
    }


    // Vision Cone Drawer for debugging (only visible when the boss is selected in the editor)
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;

        Vector3 origin = transform.position;
        float halfAngle = visionAngle / 2f;

        Vector3 leftDir = DirFromAngle(-halfAngle);
        Vector3 rightDir = DirFromAngle(halfAngle);

        Gizmos.DrawLine(origin, origin + leftDir * visionRange);
        Gizmos.DrawLine(origin, origin + rightDir * visionRange);

        int segments = 20;
        Vector3 prevPoint = origin + leftDir * visionRange;

        for (int i = 1; i <= segments; i++)
        {
            float angle = Mathf.Lerp(-halfAngle, halfAngle, i / (float)segments);
            Vector3 nextPoint = origin + DirFromAngle(angle) * visionRange;
            Gizmos.DrawLine(prevPoint, nextPoint);
            prevPoint = nextPoint;
        }
    }
}