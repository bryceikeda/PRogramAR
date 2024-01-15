using UnityEngine;

namespace ProgramAR
{
    namespace Data
    {
        public class TestData : MonoBehaviour
        {
            public DataController dataController;

            #region Unity Functions
#if UNITY_EDITOR
            private void Update()
            {
                if (Input.GetKeyUp(KeyCode.R))
                {
                    TestAddScore(1);
                    Log("Score = " + dataController.Score + " | Highscore = " + dataController.Highscore);
                }
                if (Input.GetKeyUp(KeyCode.T))
                {
                    TestAddScore(-1);
                    Log("Score = " + dataController.Score + " | Highscore = " + dataController.Highscore);
                }
                if (Input.GetKeyUp(KeyCode.Space))
                {
                    TestResetScore();
                    Log("Score = " + dataController.Score + " | Highscore = " + dataController.Highscore);
                }
            }


#endif
            #endregion

            #region Private Functions
            private void TestAddScore(int _delta)
            {
                dataController.Score += _delta;
            }

            private void TestResetScore()
            {
                dataController.Score = 0;
            }

            private void Log(string _msg)
            {
                Debug.Log("[TestData] " + _msg);
            }
            #endregion
        }
    }
}