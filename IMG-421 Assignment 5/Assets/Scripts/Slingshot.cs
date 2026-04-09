using System.Collections;
using System.Collections.Generic;
using JetBrains.Rider.Unity.Editor;
using Unity.VisualScripting;
using UnityEngine;

public class Slingshot : MonoBehaviour
{
    [Header("Inscribed")]
    public GameObject projectilePrefab;
    public float velocityMult = 10f;
    public GameObject projLinePrefab;

    [Header("Dynamic")]
    public GameObject launchPoint;
    public Vector3 launchPos;
    public GameObject projectile;
    public bool aimingMode;

    void Awake()
    {
        Transform launchPointTrans = transform.Find("LaunchPoint");
        launchPoint = launchPointTrans.gameObject;
        launchPoint.SetActive(false);
        launchPos = launchPointTrans.position;
    }

    void OnMouseEnter() 
    {
        launchPoint.SetActive(true);
        //print("Slingshot:OnMouseEnter()");
    }

    void OnMouseExit()
    {
        launchPoint.SetActive(false);
        //print("Slingshot:OnMouseExit()");
    }

    void OnMouseDown()
    {
        aimingMode = true;
        projectile = Instantiate(projectilePrefab) as GameObject;
        projectile.transform.position = launchPos;
        projectile.GetComponent<Rigidbody>().isKinematic = true;
    }

    void Update()
    {
        if (!aimingMode) return;

        Vector3 mousePos2D = Input.mousePosition;

        mousePos2D.z = -Camera.main.transform.position.z;
        Vector3 mousePos3D = Camera.main.ScreenToWorldPoint(mousePos2D);

        Vector3 mouseDelta = mousePos3D -launchPos;
        float maxMagnitude = this.GetComponent<SphereCollider>().radius;

        if (mouseDelta.magnitude > maxMagnitude)
        {
            mouseDelta.Normalize();
            mouseDelta *= maxMagnitude;
        }

        Vector3 projPos = launchPos + mouseDelta;
        projectile.transform.position = projPos;

        if (Input.GetMouseButtonUp(0))
        {
            aimingMode = false;

            float[] angles = GameManager.Instance.currentDifficulty == GameManager.Difficulty.Easy
                ? new float[] { -15f, 0f, 15f }
                : new float[] { 0f };

            foreach (float angle in angles)
            {
                // Use the existing projectile for the middle shot, instantiate for the others
                GameObject proj = (angle == 0f) ? projectile : Instantiate(projectilePrefab);
                proj.transform.position = launchPos;

                Rigidbody projRB = proj.GetComponent<Rigidbody>();
                projRB.isKinematic = false;
                
                projRB.collisionDetectionMode = CollisionDetectionMode.Continuous;

                float adjustedMult = velocityMult;
                switch (GameManager.Instance.currentDifficulty)
                {
                    case GameManager.Difficulty.Easy:   adjustedMult = velocityMult * 2f;   break;
                    case GameManager.Difficulty.Medium: adjustedMult = velocityMult * 1.5f; break;
                    case GameManager.Difficulty.Hard:   adjustedMult = velocityMult;        break;
                }

                Vector3 velocity = -mouseDelta * adjustedMult;
                velocity = Quaternion.Euler(0, 0, angle) * velocity;
                projRB.velocity = velocity;

                Instantiate<GameObject>(projLinePrefab, proj.transform);
            }

            FollowCam.SWITCH_VIEW(FollowCam.eView.slingshot);
            FollowCam.POI = projectile;
            projectile = null;
            MissionDemolition.SHOT_FIRED();
        }

    }
}
