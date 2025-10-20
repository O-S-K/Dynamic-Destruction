using UnityEngine;

public class DemoController : MonoBehaviour
{
   public float force_multiplier = 50f;

   private void Awake()
   {
       Application.targetFrameRate = 120;
   }

   void Update()
    {
        Ray _ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit_info;
        if (Input.GetMouseButtonDown(0))
        {
            if (Physics.Raycast(_ray, out hit_info, 100, 1 << LayerMask.NameToLayer("Destructible"), QueryTriggerInteraction.Ignore))
            {
                hit_info.collider.GetComponent<DestroyedPieceController>().CauseDamage(_ray.direction * force_multiplier, _ray.direction, force_multiplier);
            }
        }
    }
}
