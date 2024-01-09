using PowerTest;
using UnityEngine;

public class TestMyMonoBehaviourClass : MonoBehaviour
{
    private MyMonoBehaviourClass objectUnderTest;

    [PowerTestSetup]
    public void Setup()
    {
        GameObject testObject = new GameObject("TestObject");
        objectUnderTest = testObject.AddComponent<MyMonoBehaviourClass>();
    }

    [PowerTest]
    public void TestAction()
    {
        objectUnderTest.PerformAction();
        Asserts.IsTrue(objectUnderTest.IsActionDone, "Action should be performed");
    }

    [PowerTestTearDown]
    public void TearDown()
    {
        Destroy(objectUnderTest.gameObject); // Cleanup the GameObject
    }
}
