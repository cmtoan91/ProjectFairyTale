using UnityEngine;

namespace MainGame
{
    [CreateAssetMenu(fileName = "cardinfo", menuName = "ScriptableObjects/Card Effects/Effect", order = 1)]

    public abstract class SO_CardEffect : ScriptableObject
    {
        public abstract void ActivateEffect();
    }
}