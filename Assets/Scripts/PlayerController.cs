using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    enum PlayerState 
    {
        //None = 0,
        PlacingTurrets = 0,
        CheckingTurret,
        EndPlayerState,
    }

    public GameObject m_turretPrefab;
    public Transform m_mouseMarker;

    private PlayerState m_currentState;

    void Awake()
    {
        m_currentState = PlayerState.PlacingTurrets;
    }

    // Update is called once per frame
    void Update()
    {
        ProcessGeneralMouseControls();

        if (m_currentState == PlayerState.PlacingTurrets)
        {
            PlaceTurret();
        }
    }

    private void ProcessGeneralMouseControls()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit mouseMovingHit;
        if (Physics.Raycast(ray, out mouseMovingHit, 100f))
        {
            Vector3 mousePos = mouseMovingHit.point;
            m_mouseMarker.position = AlignToGrid(mousePos);
        }

        if (Input.GetMouseButtonDown(1))
        {
            m_currentState = (PlayerState)((int)++m_currentState % (int)PlayerState.EndPlayerState);
            Debug.Log("Current State: " + m_currentState.ToString());
        }
    }

    private void PlaceTurret()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit placingTurretsHit;
            if (Physics.Raycast(ray, out placingTurretsHit, 100f))
            {
                if (placingTurretsHit.transform.CompareTag("Ground"))
                {
                    Instantiate(m_turretPrefab, AlignToGrid(placingTurretsHit.point), placingTurretsHit.transform.rotation);
                    //https://learn.unity.com/tutorial/unity-navmesh#5c7f8528edbc2a002053b497
                    //Debug.Log("Did Hit: " + placingTurretsHit.transform.tag);
                }
            }
            else
            {
                //Debug.Log("Did not Hit");
            }
        }
    }

    private Vector3 AlignToGrid(Vector3 pos)
    {
        Vector3 newPos = new Vector3();
        newPos.x = Mathf.Floor(pos.x);
        newPos.z = Mathf.Floor(pos.z);
        return newPos;
    }
}
