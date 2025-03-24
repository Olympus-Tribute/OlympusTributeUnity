using UnityEngine;

namespace BuildingsFolder
{
    public class BuildingFlag : MonoBehaviour
    {
        // Flag
        [SerializeField] public GameObject prefabFlag;

        public GameObject InstantiateFlag(int x, int y, uint ownerId)
        {
            (float xWorldCenterCo, float zWorldCenterCo)  = StaticGridTools.MapIndexToWorldCenterCo(x, y);
            
            GameObject flag = Object.Instantiate(prefabFlag, new Vector3(xWorldCenterCo, 0, zWorldCenterCo), Quaternion.identity);
            var nflag = flag.GetComponent<Renderer>();
            foreach (var nflagMaterial in nflag.materials)
            {
                if ( ! nflagMaterial.name.Contains("Flag")) continue;
                
                nflagMaterial.color = OwnersMaterial.GetColor(ownerId);
            }
            return flag;
        }
    }
}