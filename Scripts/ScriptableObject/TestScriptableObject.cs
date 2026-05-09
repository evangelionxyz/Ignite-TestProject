using System;

using Ignite;
using static Ignite.Mathf;

namespace TestProject;

[CreateAssetMenu(FileName = "TestScriptableObject", MenuName = "Game/TestSO")]
public class TestScriptableObject : ScriptableObject
{
    [SerializeField]
    public string MyString;

    [SerializeField]
    public int MyInt;

    [SerializeField]
    public float MyFloat;

    [SerializeField]
    public Vector3 MyVector3;

    [SerializeField]
    public Entity MyEntity;
}
