using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VFXController : MonoBehaviour
{
    public GameObject dust_cloud;
    public static VFXController Instance
    {
        get
        {
            if (!_instance)
                _instance = FindObjectOfType<VFXController>();
            if (!_instance)
                Debug.LogError("No VFXController in scene");

            return _instance;
        }
    }
    private static VFXController _instance;
    
    
    public void spawn_dust_cloud(Vector3 position, Vector3 force_direction , float force_multiplier)
    {
        var ps = Instantiate(dust_cloud, position, Quaternion.identity).GetComponent<ParticleSystem>();
        Destroy(ps.gameObject, 2f);
        
        var vel = ps.velocityOverLifetime;
        vel.enabled = false; // Tắt override velocity nếu còn bật

        var forceModule = ps.forceOverLifetime;
        forceModule.enabled = true;

        vel.enabled = true;
        vel.x = force_direction.x * force_multiplier;
        vel.y = force_direction.y * force_multiplier;
        vel.z = force_direction.z * force_multiplier;
    }
}