using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Attractor : MonoBehaviour
{
    public float G = 3f;
    public float D = 0f;
    public float maxDistance = 3f;
    public static List<Attractor> Attractors;
    //public List<Attractor> lines;
    private Rigidbody rb;
    //public bool isCaptured = false;
    public bool isPlayer;
    public bool isBlackhole;
    //private GameObject line;
    public Vector3 originalScale;
    public float disToBlackhole;
    private GameObject player;

    void OnEnable()
    {
        if (Attractors == null)
            Attractors = new List<Attractor>();

        Attractors.Add(this);

        //if (lines == null)
        //    lines = new List<Attractor>();
    }

    private void Start()
    {
        player = GameManager.Player;
        isPlayer = gameObject == player;
        rb = GetComponent<Rigidbody>();
        if (rb == null)
            Debug.LogError("no rb");
        //line = GameAssets.GetPrefab("Line");
        originalScale = transform.localScale;
        disToBlackhole = float.MaxValue;
    }
    void FixedUpdate()
    {
        if (isPlayer)
        {
            float minDistance = float.MaxValue;
            float maxDistance = 1f;
            foreach (Attractor attractor in Attractors)
            {
                if (attractor.isBlackhole)
                {
                    float distance = (rb.position - attractor.rb.position).magnitude;
                    minDistance = Mathf.Min(minDistance, distance);
                    maxDistance = attractor.maxDistance;
                }
            }
            float scale = minDistance > maxDistance ? 1f : minDistance / maxDistance;
            transform.localScale = originalScale * scale;
            return;
        }
        if (isBlackhole)
        {
            foreach (Attractor attractor in Attractors)
            {
                if (attractor != this)
                {
                    attract(attractor);
                }
            }
        }
        else if (player != null)
            attract(player.GetComponent<Attractor>());
    }

    void OnDisable()
    {
        Attractors.Remove(this);
    }

    void attract(Attractor objToAttract)
    {
        //if (!objToAttract.isPlayer)
        //    return;

        Rigidbody rbToAttract = objToAttract.rb;

        Vector3 direction = rb.position - rbToAttract.position;
        float distance = direction.magnitude;
        float forceMagnitude;

        if (isBlackhole && !objToAttract.isPlayer)
        {
            if (objToAttract.disToBlackhole > distance)
            {
                objToAttract.disToBlackhole = distance;
                float scale = distance > maxDistance ? 1f : distance / maxDistance;
                Vector3 localScale = objToAttract.originalScale * scale;
                if (distance > maxDistance)
                {
                    objToAttract.gameObject.transform.localScale = objToAttract.originalScale;
                } else
                {
                    objToAttract.gameObject.transform.localScale -= new Vector3(1, 1, 0) * 0.01f;
                }

            }
        }

        if (distance == 0f || distance > maxDistance)
            return;

        //if (objToAttract.isPlayer)
        //{
            //forceMagnitude = K * Mathf.Abs(distance);
        forceMagnitude = G * (rb.mass * rbToAttract.mass) / Mathf.Pow(distance + D, 2);

        if (isBlackhole && objToAttract.isPlayer)
        {
            forceMagnitude *= 0.3f;
        }
        if (isBlackhole && !objToAttract.isPlayer)
        {
            forceMagnitude *= 2f;
        }
        //}
        //else
        //{
        //    if (distance > maxDistance)
        //    {
        //        //if (lines.Contains(objToAttract))
        //        //    lines.Remove(objToAttract);
        //        //if (objToAttract.lines.Contains(this))
        //        //    objToAttract.lines.Remove(this);
        //        return;
        //    }

        //    forceMagnitude = G * (rb.mass * rbToAttract.mass) / Mathf.Pow(distance + D, 2);
        //}

        //generateLine(objToAttract);

        Vector3 force = direction.normalized * forceMagnitude;

        rbToAttract.AddForce(force);
    }

    //void generateLine(Attractor objToAttract)
    //{
    //    if (gameObject.CompareTag("Planet") && objToAttract.gameObject.CompareTag("Planet"))
    //        return;

    //    if (lines.Contains(objToAttract) || objToAttract.lines.Contains(this))
    //        return;

    //    lines.Add(objToAttract);
    //    GameObject lineGO = Instantiate(line, Vector3.zero, Quaternion.identity);
    //    LineController lineController = lineGO.GetComponent<LineController>();
    //    lineController.a1 = gameObject;
    //    lineController.a2 = objToAttract.gameObject;
    //    lineController.maxDistance = maxDistance;
    //}

    void OnDrawGizmos()
    {
        if (isPlayer)
            return;
        // Draw a yellow sphere at the transform's position
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, maxDistance);
    }
}
