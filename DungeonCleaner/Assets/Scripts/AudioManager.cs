using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;
    public AudioSource audioSourcePrefab;
    private Queue<AudioSource> pool = new Queue<AudioSource>();

    [SerializeField] private AudioClip enemyHurtClip;
    [SerializeField] private AudioClip expGetClip;
    [SerializeField] private AudioClip goldGetClip;
    [SerializeField] private AudioClip healClip;
    [SerializeField] private AudioClip bombClip;
    [SerializeField] private AudioClip levelUpClip;
    [SerializeField] private AudioClip buttonClickClip;
    [SerializeField] private AudioClip bubbleShieldUpgradeClip;
    [SerializeField] private AudioClip magnetClip;
    [SerializeField] private AudioClip bossWarningClip;
    [SerializeField] private AudioClip broomSlashClip;
    [SerializeField] private AudioClip clapClip;
    [SerializeField] private AudioClip finalBossWarningClip;
    [SerializeField] private AudioClip gameOverClip;
    [SerializeField] private AudioClip merchantClip;
    [SerializeField] private AudioClip victoryClip;
    [SerializeField] private AudioClip boxDestroy;
    [SerializeField] private AudioClip bossFire;
    [SerializeField] private AudioClip bossDash;

    private AudioSource audioSource2D;

    private void Awake()
    {
        Instance = this;
        audioSource2D = GetComponent<AudioSource>();
    }

    public void PlaySound(Vector3 pos, AudioClip clip)
    {
        AudioSource src = GetSource();
        src.transform.position = pos;
        src.clip = clip;
        src.Play();
        StartCoroutine(CoReturnAfterPlay(src));
    }

    private void PlaySound(Vector3 pos, AudioClip clip, float volume)
    {
        AudioSource src = GetSource();
        src.transform.position = pos;
        src.clip = clip;
        src.PlayOneShot(clip, volume);
        StartCoroutine(CoReturnAfterPlay(src));
    }

    private AudioSource GetSource()
    {
        if (pool.Count > 0)
            return pool.Dequeue();

        return Instantiate(audioSourcePrefab, transform);
    }

    private IEnumerator CoReturnAfterPlay(AudioSource src)
    {
        yield return new WaitForSeconds(src.clip.length);
        pool.Enqueue(src);
    }

    private int MaxExpSoundCount = 15;
    private int currentExpSoundCount = 0;

    public void ExpGet(Vector3 pos)
    {
        if (currentExpSoundCount >= MaxExpSoundCount)
            return;

        currentExpSoundCount++;
        audioSource2D.PlayOneShot(expGetClip, 0.7f);
        StartCoroutine(CoExpCountMinus(expGetClip.length));
    }

    private IEnumerator CoExpCountMinus(float f)
    {
        yield return new WaitForSeconds(f);
        currentExpSoundCount--;
    }

    public void EnemyHurt(Vector3 pos)
    {
        PlaySound(pos, enemyHurtClip, 0.7f);
    }

    public void GoldGet(Vector3 pos)
    {
        audioSource2D.PlayOneShot(goldGetClip, 2.0f);
    }

    public void FoodGet(Vector3 pos)
    {
        audioSource2D.PlayOneShot(healClip, 2.0f);
    }

    public void Bomb(Vector3 pos)
    {
        audioSource2D.PlayOneShot(bombClip, 4.0f);
    }

    public void BoxDestroy(Vector3 pos)
    {
        audioSource2D.PlayOneShot(boxDestroy, 3.0f);
    }

    public void Magnet(Vector3 pos)
    {
        audioSource2D.PlayOneShot(magnetClip);
    }

    public void LevelUp()
    {
        PlaySound(Player.Instance.transform.position, levelUpClip, 0.7f);
    }

    public void Click()
    {
        PlaySound(Player.Instance.transform.position, buttonClickClip);
    }

    public void BubbleShield()
    {
        PlaySound(Player.Instance.transform.position, bubbleShieldUpgradeClip);
    }

    public void BossWarning()
    {
        PlaySound(Player.Instance.transform.position, bossWarningClip);
    }

    public void BroomSlash()
    {
        PlaySound(Player.Instance.transform.position, broomSlashClip);
    }

    public void Clap()
    {
        PlaySound(Player.Instance.transform.position, clapClip);
    }

    public void FinalBossWarning()
    {
        PlaySound(Player.Instance.transform.position, finalBossWarningClip);
    }

    public void GameOver()
    {
        PlaySound(Player.Instance.transform.position, gameOverClip);
    }

    public void MerchantCome()
    {
        PlaySound(Player.Instance.transform.position, merchantClip);
    }

    public void Victory()
    {
        PlaySound(Player.Instance.transform.position, victoryClip);
    }

    public void BossDash()
    {
        audioSource2D.PlayOneShot(bossDash, 3.3f);
    }

    public void BossFire()
    {
        audioSource2D.PlayOneShot(bossFire, 4.5f);
    }
}