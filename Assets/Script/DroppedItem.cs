using UnityEngine;

public class DroppedItem : MonoBehaviour
{
    public AudioClip dropSound;
    private AudioSource audioSource;

    void Awake()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;
    }

    public void PlayDropEffects()
    {
        if (dropSound != null)
            audioSource.PlayOneShot(dropSound);

        BossController boss = FindObjectOfType<BossController>();
        Debug.Log("Boss found: " + (boss != null));
        if (boss != null)
            boss.InvestigateSound(transform.position);
    }
}