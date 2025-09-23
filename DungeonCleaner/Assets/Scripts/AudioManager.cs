using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;
    public AudioSource audioSourcePrefab;
    private Queue<AudioSource> pool = new Queue<AudioSource>();

    public AudioClip enemyHurtClip;
    public AudioClip expGetClip;
    public AudioClip goldGetClip;
    public AudioClip healClip;
    public AudioClip bombClip;
    public AudioClip levelUpClip;
    public AudioClip buttonClickClip;
    public AudioClip bubbleShieldUpgradeClip;
    public AudioClip magnetClip;
    public AudioClip bossWarningClip;
    public AudioClip broomSlashClip;
    public AudioClip clapClip;
    public AudioClip finalBossWarningClip;
    public AudioClip gameOverClip;
    public AudioClip merchantClip;
    public AudioClip victoryClip;

    private void Awake()
    {
        Instance = this;
    }

    public void PlaySound(Vector3 pos, AudioClip clip)
    {
        AudioSource src = GetSource();
        src.transform.position = pos;
        src.clip = clip;
        src.Play();
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
        PlaySound(pos, expGetClip);
        StartCoroutine(CoExpCountMinus(expGetClip.length));
    }

    private IEnumerator CoExpCountMinus(float f)
    {
        yield return new WaitForSeconds(f);
        currentExpSoundCount--;
    }

    public void EnemyHurt(Vector3 pos)
    {
        PlaySound(pos, enemyHurtClip);
    }

    public void GoldGet(Vector3 pos)
    {
        PlaySound(pos, goldGetClip);
    }

    public void FoodGet(Vector3 pos)
    {
        PlaySound(pos, healClip);
    }

    public void Bomb(Vector3 pos)
    {
        PlaySound(pos, bombClip);
    }

    public void Magnet(Vector3 pos)
    {
        PlaySound(pos, magnetClip);
    }

    public void LevelUp()
    {
        PlaySound(Player.Instance.transform.position, levelUpClip);
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
}