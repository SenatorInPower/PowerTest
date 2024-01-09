using UnityEngine;

public class MyMonoBehaviourClass : MonoBehaviour
{
    public bool IsActionDone { get; private set; }

    public void PerformAction()
    {
        IsActionDone = true;
    }
}
