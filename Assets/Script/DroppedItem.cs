using UnityEngine;

public class DroppedItem : MonoBehaviour
{
    public AudioClip dropSound;
    private AudioSource audioSource;

    void Start()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;

        if (dropSound != null)
            audioSource.PlayOneShot(dropSound);

        BossController boss = FindObjectOfType<BossController>();
        Debug.Log("Boss found: " + (boss != null));
        if (boss != null)
            boss.InvestigateSound(transform.position);
    }
}