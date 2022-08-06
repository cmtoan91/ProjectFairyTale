using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PFTTileSlot : MonoBehaviour
{
    Vector3[] _corners;

    public Vector3[] Corners => _corners;

    public void SetCorners(Vector3[] corners)
    {
        _corners = new Vector3[corners.Length];

        for(int i = 0; i < _corners.Length; i++)
        {
            _corners[i] = corners[i] + transform.position;
        }
    }

    private void OnDrawGizmos()
    {
        if(_corners != null)
        {
            Gizmos.color = Color.gray;
            for(int i = 0; i < _corners.Length; i++)
            {
                if(i > 0)
                {
                    Gizmos.DrawLine(_corners[i - 1], _corners[i]);
                }
            }

            Gizmos.DrawLine(_corners[_corners.Length - 1], _corners[0]);
        }
    }
}
