using Unity.VisualScripting;
using UnityEngine;
[CreateAssetMenu(menuName = "Actions/New Combat Action")]
public class CombatActions : ScriptableObject
{
    

    [SerializeField] private string animName;

    public string animationName => animName;
}
