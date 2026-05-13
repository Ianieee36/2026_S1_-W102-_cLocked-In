using UnityEngine;
using UnityEngine.AI;
using TMPro;
using System.Collections;

public class BossController : MonoBehaviour
{
    UnityEngine.AI.NavMeshAgent agent;

    public Transform player;
    public Transform VisionPivot;
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
    public float rotationSpeed = 5f; // How fast the boss rotates towards the player

    [Header("Boss Alert")]
    public GameObject alertUI; // flashing red UI when the boss detects the player
    public Animator alertAnimator; // Animator for the alert UI
    public AudioSource alertAudio; // Audio source for the alert sound

    [Header("Game Over")]
    public AudioSource gameOverAudio;
    private bool gameOverTriggered = false;

    private bool alertActive = false; // Whether the alert is currently active


    // To add waypoints for pattrolling add empty game objects in the map scene and tag them with "Waypoint" and name "Waypoint(n)"
    // The boss will move between these points in order and loop back to the start.
    public Transform[] waypoints;
    int currentWaypoint;

    public TextMeshProUGUI detectionText;

    //Rigidbody2D rb;

    enum BossState { Patrol, Chase }
    BossState state;

    void Start()
    {
        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;

        //rb = GetComponent<Rigidbody2D>();
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

    //void MoveTo(Vector2 target)
    //{
    //    // Move towards target (either player or waypoint)
    //    Vector2 dir = (target - (Vector2)transform.position).normalized;
    //    rb.linearVelocity = dir * moveSpeed;
        
    //    // Rotate towards movement direction
    //    RotateTowards(dir);
    //}

    void Patrol()
    {
        // Sets target to current waypoint
        Transform target = waypoints[currentWaypoint];

        //MoveTo(target.position);

        // If close enough to the waypoint switch to the next one
        //if (Vector2.Distance(transform.position, target.position) < 0.2f)
        //{
        //    currentWaypoint = (currentWaypoint + 1) % waypoints.Length;
        //}

        agent.speed = moveSpeed;
        agent.SetDestination(target.position);

        Vector2 dir = ((Vector2)agent.velocity).normalized;

        if (dir.sqrMagnitude > 0.01f)
            RotateTowards(dir);

        if (!agent.pathPending && agent.remainingDistance < 0.2f)
        {
            currentWaypoint = (currentWaypoint + 1) % waypoints.Length;
        }
    }

    void Chase()
    {
        float dist = Vector2.Distance(transform.position, player.position);

        // New AI Pathfinding stuff
        agent.speed = chaseSpeed;

        //Vector2 dir = (player.position - transform.position).normalized;

        // Stops chasing when really close to avoid weird jittering.
        if (dist > minChaseDistance)
        {
            //rb.linearVelocity = dir * chaseSpeed;
            //RotateTowards(dir);
            agent.isStopped = false;
            agent.SetDestination(player.position);

            Vector2 dir = ((Vector2)agent.velocity).normalized;

            if (dir.sqrMagnitude > 0.01f)
                RotateTowards(dir);
        }
        else
        {
            //rb.linearVelocity = Vector2.zero;
            agent.isStopped = true;
        }
    }

    void StartAlert()
    {
        if (alertActive) return;
        alertActive = true;

        if(alertUI != null)
           alertUI.SetActive(true);

        if (alertAnimator != null)
            alertAnimator.SetTrigger("Alert");
        
        if(alertAudio != null)
            alertAudio.Play();

    }

    void StopAlert()
    {
        if(!alertActive) return;

        alertActive = false;
        
        if(alertAudio != null)
            alertAudio.Stop();

        if (alertUI != null)
            alertUI.SetActive(false);
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
                if(!gameOverTriggered)
                {

                    gameOverTriggered = true;

                    if (alertAudio != null)
                        alertAudio.Stop(); // Stop alert sound when game is over
                    
                    if(alertUI != null)
                        alertUI.SetActive(false); // Hide alert UI when game is over

                    if (gameOverAudio != null)
                        gameOverAudio.PlayOneShot(gameOverAudio.clip); // Play game over sound

                    // Delay freeze so sounds plays properly
                    StartCoroutine(GameOverDelay());
                }
                

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
        {
            state = BossState.Chase;
            StartAlert(); // Added alert when boss starts chasing
        }
        else if (detection <= 0f)
        {
            state = BossState.Patrol;
            StopAlert(); // Added stop alert when boss goes back to patrolling
        }
            

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

        float angle = Vector2.Angle(VisionPivot.right, dir);
        if (angle > visionAngle / 2f) return false;

        float dist = Vector2.Distance(transform.position, player.position);
        if (dist > visionRange) return false;

        RaycastHit2D hit = Physics2D.Raycast(transform.position, dir, visionRange);

        if (hit.collider != null)
        {
            if (hit.transform != player && ((1 << hit.collider.gameObject.layer) & obstacleMask) != 0)
                return false;
        }

        return true;
    }

    void RotateTowards(Vector2 dir)
    {
        if (dir.sqrMagnitude < 0.001f) return;

        float targetAngle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        float angle = Mathf.LerpAngle(VisionPivot.eulerAngles.z, targetAngle, rotationSpeed * Time.deltaTime);
        VisionPivot.rotation = Quaternion.Euler(0, 0, angle);
    }

    Vector3 DirFromAngle(float angle)
    {
        float rad = (angle + VisionPivot.eulerAngles.z) * Mathf.Deg2Rad;
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

    IEnumerator GameOverDelay()
    {
        yield return new WaitForSecondsRealtime(0.5f); // small delay so sound plays
        Time.timeScale = 0f; // Freeze the game
    }
}



