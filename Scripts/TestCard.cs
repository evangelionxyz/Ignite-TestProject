using Ignite;

namespace TestProject;

public class TestCard : Entity
{
    [SerializeField] private TestScriptableObject testScriptableObject;

    public override void OnCreate()
    {
        if (testScriptableObject != null)
        {
            Debug.Log($"MyString value: {testScriptableObject.MyString}");
            Debug.Log($"MyInt value: {testScriptableObject.MyInt}");
            Debug.Log($"MyFloat value: {testScriptableObject.MyFloat}");
            Debug.Log($"MyVector3 value: {testScriptableObject.MyVector3.X}");
            Debug.Log($"MyEntity value: {testScriptableObject.MyEntity.ToString()}");
        }
        else
        {
            Debug.Log("Error: TestScriptableObject is null", Debug.LogLevel.Error);
        }

        var gaEntity = FindEntity("GameManager");
        if (gaEntity != null)
        {
            var ga = gaEntity as GameManager;
            if (ga != null)
                ga.NewRun("Test");
        }

    }
}
