using Assets.Scripts.Managers;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI
{
    public class DestroyButtonOnClick : MonoBehaviour {

        public int Id { get; set; }
        
        void Start () {
            GetComponent<Button>().onClick.AddListener(DestroyOnClick);
        }

        void DestroyOnClick()
        {
            Destroy(gameObject);
            GameManager.Instance.RemoveRoutePoint(Id);
        }
    }
}
