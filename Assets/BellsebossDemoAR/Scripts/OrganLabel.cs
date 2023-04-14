using UnityEngine;

namespace BellsebossDemoAR.Scripts
{
    [CreateAssetMenu (fileName = "OrganLabel", menuName = "Bellseboss/Organs")]
    public class OrganLabel : ScriptableObject
    {
        [SerializeField] [TextArea (3, 9)] private string description;
        
        public string Description => description;
    }
}