using Assets.Scripts.Managers;
using UnityEngine;

namespace Assets.Scripts.UI
{

    public class StartBtnHandler : MonoBehaviour
    {
        [SerializeField]
        private string _destinationSceneName;

        public void OnClick()
        {
            if (_destinationSceneName == null)
            {
                Debug.LogWarning("_destinationSceneName field is null");
                return;
            }

            GameManager.Instance.LoadLevel(this, _destinationSceneName, op =>
            {
                Debug.LogFormat("Load level: {0}", op.progress);
                op.allowSceneActivation = true;
            });
        }
    }


}