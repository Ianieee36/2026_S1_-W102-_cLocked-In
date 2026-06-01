using UnityEngine;
using UnityEngine.AI;
using TMPro;
using System.Collections;

public class BossController : MonoBehaviour
{
    UnityEngine.AI.NavMeshAgent agent; // Agent for Unity AI Pathfinding

    [Header("Movement Variables")]
    public Transform player; // Variable for the player, you'll have to find player game object on game load because it's not in this scene.
    public Transform VisionPivot; // Variable for the Vision Cone Pivot, drag and drop into the field in inspector.
    private Collider2D playerCollider; // Collider for the player, used for safe zone check
    [HideInInspector] public float moveSpeed;
    [HideInInspector] public float chaseSpeed; // Move faster when chasing

    [Header("Vision Variables")]
    [HideInInspector] public float visionRange; // Vision cone
    [HideInInspector] public float visionAngle = 60f; // Vision cone
    public float minChaseDistance = 2f; // Stopping distance from player when chasing (just to avoid weird jittering)
    public LayerMask obstacleMask; // Variable for the obstacle mask for boss vision, you'll have to find obstacleMask on game load because it's not in this scene.

    [Header("Deteection Variables")]
    public float detection = 0f; // Detection level (0 to 1)
    [HideInInspector] public float detectionRate; // Speed at which detection increases per second
    [HideInInspector] public float decayRate; // Speed at which detection decreases per second
    [HideInInspector] public float timeToLose; // How lon g the player can stay at max detection before losing
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
    public DifficultyManager.Difficulty difficulty; // boss difficulty manager

    [Header("Investigation")]
    public float investigateWaitTime = 5f; //How long the boss looks around for
    public float investigateSoundRange = 10f; //How far the boss can hear
    public float lookAroundSpeed = 2f; //How fast he looks around
    public Vector3 investigatePosition;
    private float investigateTimer = 0f;
    private int waypointBeforeInvestigate;

    [Header("Chances")]


    // To add waypoints for pattrolling add empty game objects in the map scene and tag them with "Waypoint" and name "Waypoint(n)"
    // The boss will move between these points in order and loop back to the start.
    public Transform[] waypoints;
    int currentWaypoint;

    public TextMeshProUGUI detectionText;

    enum BossState { Patrol, Chase, Investigate }
    BossState state;

    void Start()
    {
        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;

        state = BossState.Patrol;

        if (DifficultyManager.Instance != null)
        {
            difficulty = DifficultyManager.Instance.currentDifficulty;
        }

        Debug.Log("Difficulty from manager: " + difficulty); 

        // Find player and waypoints in any scene
        player = GameObject.FindGameObjectWithTag("Player").transform;
        playerCollider = player.GetComponent<Collider2D>();


        GameObject[] wpObjects = GameObject.FindGameObjectsWithTag("Waypoint");

        System.Array.Sort(wpObjects, (a, b) => a.name.CompareTo(b.name));

        waypoints = new Transform[wpObjects.Length];

        for (int i = 0; i < wpObjects.Length; i++)
        {
            waypoints[i] = wpObjects[i].transform;
        }

        ApplyDifficultySettings(); // it applies difficulty settings at start
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

            case BossState.Investigate:
                Investigate();
                break;
        }
    }


    public void InvestigateSound(Vector3 soundPosition)
    {
        Debug.Log("InvestigateSound called, state: " + state + " dist: " + Vector2.Distance(transform.position, soundPosition) + " range: " + investigateSoundRange);
        if (state == BossState.Chase) return;
        float dist = Vector2.Distance(transform.position, soundPosition);
        if (dist > investigateSoundRange) return;

        investigatePosition = soundPosition;
        waypointBeforeInvestigate = currentWaypoint;
        investigateTimer = 0f;
        state = BossState.Investigate;
        Debug.Log("Boss now investigating at: " + soundPosition);
    }

    void Investigate()
    {
        if (Vector2.Distance(transform.position, investigatePosition) > 0.5f)
        {
            agent.speed = moveSpeed;
            agent.isStopped = false;
            agent.SetDestination(investigatePosition);

            Vector2 dir = ((Vector2)agent.velocity).normalized;
            if (dir.sqrMagnitude > 0.01f)
                RotateTowards(dir);
        }
        else
        {
            agent.isStopped = true;
            investigateTimer += Time.deltaTime;

            // Calculate look direction from sine wave
            float angle = Mathf.Sin(investigateTimer * lookAroundSpeed) * 90f;
            Vector2 lookDir = new Vector2(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad));

            // Only rotate to that direction if its not facing a wall
            RaycastHit2D hit = Physics2D.Raycast(transform.position, lookDir, 3f, obstacleMask);
            if (hit.collider == null)
            {
                RotateTowards(lookDir);
            }

            if (investigateTimer >= investigateWaitTime)
            {
                agent.isStopped = false;
                state = BossState.Patrol;
                currentWaypoint = waypointBeforeInvestigate;
            }
        }
    }

    void Patrol()
    {
        // Sets target to current waypoint
        Transform target = waypoints[currentWaypoint];

        agent.speed = moveSpeed;
        agent.SetDestination(target.position);

        Vector2 dir = ((Vector2)agent.velocity).normalized;

        if (dir.sqrMagnitude > 0.01f)
            RotateTowards(dir);

        if (!agent.pathPending && agent.remainingDistance < 0.2f) // If close enough to the waypoint switch to the next one
        {
            currentWaypoint = (currentWaypoint + 1) % waypoints.Length;
        }
    }

    void Chase()
    {
        float dist = Vector2.Distance(transform.position, player.position);

        bool playerInSafeZone = SafeZone.Instance != null && SafeZone.Instance.IsInside(playerCollider); // Safe zone boolean

        // New AI Pathfinding stuff
        agent.speed = chaseSpeed;

        // Safe zone check
        if (playerInSafeZone)
        {
            Patrol(); // If player is in safe zone, boss goes back to patrolling
            return;
        }

        // Stops chasing when really close to avoid weird jittering.
        if (dist > minChaseDistance)
        {
            agent.isStopped = false;
            agent.SetDestination(player.position);

            Vector2 dir = ((Vector2)agent.velocity).normalized;

            if (dir.sqrMagnitude > 0.01f)
                RotateTowards(dir);
        }
        else
        {
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
        bool canSeePlayer = CanSeePlayer();

        // Detection logic
        if (canSeePlayer)
        {   
            // it sets the CEO difficulty so the detection makes it instant.
            if(difficulty == DifficultyManager.Difficulty.CEO)
            {
                detection = 1f;
                state = BossState.Chase;
                StartAlert();
            }
            else
            {
                detection += detectionRate * Time.deltaTime;
            }
        }
        else
        {
            detection -= decayRate * Time.deltaTime;
        }
            
        detection = Mathf.Clamp01(detection);

        // Detection time
        if (canSeePlayer && detection >= 0.9f)
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

                    TryAgain.Instance.PlayerCaught(); // Try again option when caught
                }
                
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
        else if (detection <= 0f && state != BossState.Investigate)
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
        // If player is hiding boss can never see them
        if (PlayerHiding.Instance != null && PlayerHiding.Instance.IsHiding())
            return false;

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

        Vector3 origin = VisionPivot.position;
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

    // the boss's state is based on the difficulty manager 
    // where boss's state is fixed based on the difficulty (Intern, Senior, CEO).
    void ApplyDifficultySettings()
    {
        DifficultyManager diff = DifficultyManager.Instance;

        moveSpeed = diff.moveSpeed;
        chaseSpeed = diff.chaseSpeed;
        visionRange = diff.visionRange;
        detectionRate = diff.detectionRate;
        decayRate = diff.decayRate;
        timeToLose = diff.timeToLose; 
    }

    public void ResetAfterCaught()
    {
        detection = 0f;
        detectedTime = 0f;
        gameOverTriggered = false;

        state = BossState.Patrol;
        StopAlert();
    }

    // Helper for unit tests
    public string GetCurrentStateName() => state.ToString();
}



