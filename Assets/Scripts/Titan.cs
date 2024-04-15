using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Titan : MonoBehaviour
{
    public ParticleSystem onShootFX;
    public ShockWave wave;
    public Transform shootStart;
    public float reloadingTime;
    List<ParticleSystem> shootsFX = new();
    Animator animator => GetComponent<Animator>();


    IEnumerator Landing()
    {
        ShockWave waveObj = Instantiate(wave, transform.position, Quaternion.identity);        
        yield return new WaitUntil(waveObj.IsDestroyed);
    }
    IEnumerator ShootAllEnnemies(Enemy[] enemies)
    {
        foreach (Enemy enemy in enemies)
        {
            animator.SetTrigger("Reload");
            AudioManager.instance.Play("TitanReload");
            yield return new WaitForSeconds(reloadingTime);
            if (enemy == null) continue;
            if (enemy.isDestroying) continue;
            AudioManager.instance.Play("TitanShoot");
            ParticleSystem shootClone = Instantiate(onShootFX, shootStart.transform.position, Quaternion.identity);
            shootsFX.Add(shootClone);
            var vel = shootClone.velocityOverLifetime;
            Vector3 dir = enemy.transform.position - shootClone.transform.position;
            vel.orbitalOffsetX = dir.x;
            vel.orbitalOffsetY = dir.y;
            vel.orbitalOffsetZ = dir.z;
            yield return new WaitForSeconds(reloadingTime / 2);
        }
    }

    private void Awake()
    {
        StartCoroutine(Attack());
    }

    IEnumerator Attack()
    {
        AudioManager.instance.Play("Summon");
        yield return Landing();
        SurvivalManager.instance.StopEnemies();
        yield return ShootAllEnnemies(FindObjectsOfType<Enemy>());
        yield return new WaitWhile(() => shootsFX.Exists(fx => fx != null));
        SurvivalManager.instance.NextWave();
        animator.SetTrigger("EndShoot");
        Leave();
    }

    void Leave()
    {
        AudioManager.instance.Play("TitanLeaves");
        Destroy(gameObject);
    }
}
