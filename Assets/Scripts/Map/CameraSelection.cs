using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSelection : MonoBehaviour
{
    Camera m_Camera;
    private GameManager GM;

    void Awake()
    {
        m_Camera = Camera.main;
    }

    void Start()
    {
        GM = FindObjectOfType<GameManager>();
    }

    void Update()
    {
        if(GM == null || !GM.IsGameStarted || GM.IsInUI)
            return;

        if(Input.GetMouseButtonDown(0))
        {
            Vector3 mousePosition = Input.mousePosition;
            Ray ray = m_Camera.ScreenPointToRay(mousePosition);

            if(Physics.Raycast(ray, out RaycastHit hit) && hit.collider != null && hit.collider.gameObject.GetComponent<TileCollider>() != null)
            {
                TileCollider tileCollider = hit.collider.gameObject.GetComponent<TileCollider>();

                if(tileCollider != null && tileCollider.ParentTile != null)
                {
                    Debug.Log("Clicked on tile: " + tileCollider.ParentTile.Type.ToString() + " Loc: " + tileCollider.ParentTile.Location.ToString());

                    GM.MapGenerator.SelectTile(tileCollider.ParentTile);
                }

            }
        }
    }
}
