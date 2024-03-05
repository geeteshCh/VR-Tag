using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Teleporter : MonoBehaviour
{
    // Start is called before the first frame update
    public int teleportationCredits;
    public int rayLength = 10;
    public float delay = 0.1f;
    private bool aboutToTeleport = false;
    private Vector3 teleportPos = new Vector3();
    public Material tmat;
    public GameObject righController;
    public GameObject pointer;
    public PlayerSpawner ps;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (teleportationCredits > 0)
        {
            RaycastHit hit;
            
            if (OVRInput.Get(OVRInput.Button.One))
            {
                if (Physics.Raycast(righController.transform.position, righController.transform.forward, out hit,
                        rayLength * 10))
                {
                    if (hit.collider.GameObject().tag == "Planet")
                    {
                        
                        aboutToTeleport = true;
                        teleportPos = hit.point;

                        GameObject myLine = new GameObject();
                        myLine.transform.position = righController.transform.position;
                        myLine.AddComponent<LineRenderer>();

                        LineRenderer lr = myLine.GetComponent<LineRenderer>();
                        lr.material = tmat;

                        lr.startWidth = 0.01f;
                        lr.endWidth = 0.01f;
                        lr.SetPosition(0, righController.transform.position);
                        lr.SetPosition(1, hit.point);
                        GameObject.Destroy(myLine, delay);
                        pointer.SetActive(true);
                        pointer.transform.position = hit.point;
                    }
                    else
                    {
                        aboutToTeleport = false;
                        pointer.SetActive(false);
                    }

                }
            }
            

            if (OVRInput.GetUp(OVRInput.Button.One) && aboutToTeleport)
            {
                pointer.SetActive(false);
                aboutToTeleport = false;
                transform.position = teleportPos;
                teleportationCredits-=1;
                ps.teleportationCreditText.text = teleportationCredits.ToString();
            }
        }
    }
}
