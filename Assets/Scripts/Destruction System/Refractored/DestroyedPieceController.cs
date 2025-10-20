using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyedPieceController : MonoBehaviour
{
    public bool is_connected = true;
    [HideInInspector] 
    public bool visited = false;
    public List<DestroyedPieceController> connected_to;

    public static bool is_dirty = false;

    private Rigidbody _rigidbody;
    private Vector3 _starting_pos;
    private Quaternion _starting_orientation;
    private Vector3 _starting_scale;

    [SerializeField]
    private bool _configured = false;
    private bool _connections_found = false;

    private void Awake()
    {
        Initialize();
    }

    public void Initialize()
    {
        _rigidbody = GetComponent<Rigidbody>();
        connected_to = new List<DestroyedPieceController>();
        _starting_pos = transform.position;
        _starting_orientation = transform.rotation;
        _starting_scale = transform.localScale;
        transform.localScale *= 1.02f;
    }
    
    private void OnCollisionEnter(Collision collision)
    {
        if (!_configured)
        {
            var neighbour = collision.gameObject.GetComponent<DestroyedPieceController>();
            Debug .Log("Collided with: " + collision.gameObject.name);
            if (neighbour)
            {
                if(!connected_to.Contains(neighbour))
                    connected_to.Add(neighbour);
            }
        }
        else if (collision.gameObject.CompareTag("Floor"))
        {
            VFXController.Instance.spawn_dust_cloud(transform.position, collision.relativeVelocity.normalized, collision.relativeVelocity.magnitude * 0.1f);
        }
    }

    public void MakeStatic()
    {
        _configured = true;
        _rigidbody.isKinematic = true;
        _rigidbody.useGravity = true;

        transform.localScale = _starting_scale;
        transform.position = _starting_pos;
        transform.rotation = _starting_orientation;
    }

    public void CauseDamage(Vector3 force, Vector3 force_direction, float force_multiplier)
    {
        is_connected = false;
        _rigidbody.isKinematic = false;
        is_dirty = true;
        _rigidbody.AddForce(force, ForceMode.Impulse);
        VFXController.Instance.spawn_dust_cloud(transform.position, force_direction, force_multiplier);
    }

    public void Drop()
    {
        is_connected = false;
        _rigidbody.isKinematic = false;
    }
}
