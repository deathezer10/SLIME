﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowBase : MonoBehaviour
{

    public enum ArrowType
    {
        None,
        Flame,
        Bait,
        Thunder
    }

    [SerializeField]
    private GameObject m_FlameParticlePrefab;

    protected bool m_IsFired = false;
    public bool isFired { get { return m_IsFired; } }

    private ArrowType m_ArrowType = ArrowType.None;
    public ArrowType arrowType { get { return m_ArrowType; } }


    bool isPendingDestruction = false;

    Rigidbody m_Rigidbody;

    public void BuffArrow(ArrowType arrowType)
    {
        if (arrowType != m_ArrowType)
        {
            switch (arrowType)
            {
                case ArrowType.Flame:
                    //delete this if it doesnt work
                    AudioManager.Instance.Play3D("arrowlit", transform.position, AudioManager.AudioType.Additive);
                    Instantiate(m_FlameParticlePrefab, transform);
                    break;
                case ArrowType.Bait: // TBD
                    break;
                case ArrowType.Thunder: // TBD
                    break;
                default:
                    break;
            }
        }

        m_ArrowType = arrowType;
    }

    public void LaunchArrow(float forceAmount)
    {
        if (m_IsFired)
            return;

        m_IsFired = true;

        transform.parent = null;

        m_Rigidbody.useGravity = true;
        m_Rigidbody.isKinematic = false;
        m_Rigidbody.AddForce(transform.up * forceAmount, ForceMode.Impulse);

        var ps = GetComponentInChildren<ParticleSystem>();
        if (ps)
            ps.Stop();
    }

    public void LaunchArrowWithArc(Vector3 destination, float time)
    {
        // finding the distance
        Vector3 distance = destination - transform.position;
        Vector3 distanceXZ = distance;
        distanceXZ.y = 0;

        float sY = distance.y; // vertical dist
        float sXZ = distanceXZ.magnitude;  // horizontal dist

        float velocityX = sXZ / time;
        float velocityY = sY / time + 0.5f * Mathf.Abs(Physics.gravity.y) * time;

        Vector3 result = distanceXZ.normalized;
        result *= velocityX;
        result.y = velocityY;

        GetComponent<Rigidbody>().AddForce(result, ForceMode.VelocityChange);
    }

    public void DestroyArrow()
    {
        StartCoroutine(DestroyArrowRoutine());
    }

    private IEnumerator DestroyArrowRoutine()
    {
        if (isPendingDestruction)
            yield break;
        else
        {
            isPendingDestruction = true;
            m_Rigidbody.useGravity = false;
            m_Rigidbody.isKinematic = true;
            GetComponent<Collider>().enabled = false;
            yield return new WaitForSeconds(2);
            Destroy(gameObject);
        }
    }

    protected void Awake()
    {
        m_Rigidbody = GetComponent<Rigidbody>();
    }

    protected void FixedUpdate()
    {
        if (m_IsFired && m_Rigidbody.velocity != Vector3.zero)
        {
            transform.rotation = Quaternion.LookRotation(m_Rigidbody.velocity);
            transform.Rotate(90, 0, 0);
        }
    }

}