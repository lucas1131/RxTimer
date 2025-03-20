using System.Collections;
using UnityEngine;
using UnityEngine.TestTools;
using Zenject;

namespace RxClock.Tests.PlayMode.SceneTests
{
    public class ClockAppSceneShould : SceneTestFixture
    {
        [UnityTest]
        public IEnumerator LoadWithoutErrors()
        {
            yield return LoadScene("ClockApp");

            yield return new WaitForSeconds(2f);
        }
    }
}