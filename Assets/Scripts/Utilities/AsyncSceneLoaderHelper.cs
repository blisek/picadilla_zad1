using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts.Managers;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Scripts.Utilities
{
    public static class AsyncSceneLoaderHelper
    {
        public static IEnumerator SupervisedLevelLoadCoroutine(AsyncOperation asyncLoadLoadOperation, Action<AsyncOperation> sceneLoadProgressOverdrawCallback, int progressOverdrawCounterMinValue = 1)
        {
            if(asyncLoadLoadOperation == null)
                throw new ArgumentNullException("asyncLoadLoadOperation");

            var progressOverdrawCounter = 0;
            while (true)
            {
                if (UpdateLoadProcess(asyncLoadLoadOperation, progressOverdrawCounterMinValue, ref progressOverdrawCounter))
                {
                    if (sceneLoadProgressOverdrawCallback != null)
                        sceneLoadProgressOverdrawCallback(asyncLoadLoadOperation);
                    yield break;
                }

                yield return null;
            }
        }

        private static bool UpdateLoadProcess(AsyncOperation sceneLoadTask, int progressOverdrawCounterMinValue, ref int progressOverdrawCounter)
        {
            if (sceneLoadTask.isDone || sceneLoadTask.progress <= .89f)
                return false;
            
            return ++progressOverdrawCounter >= progressOverdrawCounterMinValue;
        }
    }
    
}
