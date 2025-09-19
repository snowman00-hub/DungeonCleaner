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

    public void EnemyHurt(Vector3 pos)
    {
        PlaySound(pos, enemyHurtClip);
    }

    public void ExpGet(Vector3 pos)
    {
        PlaySound(pos, expGetClip);
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
}

