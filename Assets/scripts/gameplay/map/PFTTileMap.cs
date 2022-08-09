using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MapDesigner
{
    public class PFTTileMap : MonoBehaviour
    {
        #region props
        [SerializeField]
        List<PFTTileSlot> _allSlot = new List<PFTTileSlot>();
        #endregion

        public void AddTileSlotToMap(PFTTileSlot slot)
        {
            _allSlot.Add(slot);
        }
    }
}