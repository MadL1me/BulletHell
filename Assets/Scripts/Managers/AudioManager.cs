using Unity;
using UnityEngine;


public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;


    [SerializeField] private AudioClip[] soundTracks;
    [SerializeField] private AudioClip[] playerShootsSounds;
    [SerializeField] private AudioClip[] footstepsSound;
    [SerializeField] private AudioSource source;

    [SerializeField] private AudioClip revolverRollSound;
    [SerializeField] private AudioClip addBulletToRollSound;
    [SerializeField] private AudioClip bulletComboSound;
    [SerializeField] private AudioClip newBulletComboSound;
    [SerializeField] private AudioClip bookOpenSound;
    [SerializeField] private AudioClip leafoverSound;

    public void PlayShootSound() => AudioSource.PlayClipAtPoint(playerShootsSounds[Random.Range(0, playerShootsSounds.Length - 1)], Player.Instance.transform.position);
    public void PlayFootstepSound() => AudioSource.PlayClipAtPoint(footstepsSound[Random.Range(0, footstepsSound.Length - 1)], Player.Instance.transform.position);
    public void PlayRevolverRollSound() => AudioSource.PlayClipAtPoint(revolverRollSound, Player.Instance.transform.position);
    public void PlayBulletComboSound() => AudioSource.PlayClipAtPoint(bulletComboSound, Player.Instance.transform.position);
    public void PlayNewComboSound() => AudioSource.PlayClipAtPoint(newBulletComboSound, Player.Instance.transform.position);
    public void PlayAddBulletToRoll() => AudioSource.PlayClipAtPoint(addBulletToRollSound, Player.Instance.transform.position);
    public void PlayBookOpenSound() 
    {
        Time.timeScale = 1;
        AudioSource.PlayClipAtPoint(bookOpenSound, Player.Instance.transform.position);
        Time.timeScale = 0;
    }
    public void PlayLeafoverSound()
    {
        Time.timeScale = 1;
        AudioSource.PlayClipAtPoint(leafoverSound, Player.Instance.transform.position);
        Time.timeScale = 0;
    }
    private void Awake()
    {
        Instance = this;
        source = GetComponent<AudioSource>();
    }
}