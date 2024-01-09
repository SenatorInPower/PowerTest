using PowerTest;

public class TestInitialization
{
    private SomeClass objectUnderTest;

    [PowerTestSetup]
    public void Setup()
    {
        objectUnderTest = new SomeClass();
        objectUnderTest.Initialize(); // Setup the object
    }

    [PowerTest]
    public void ObjectIsProperlyInitialized()
    {
        Asserts.IsTrue(objectUnderTest.IsInitialized, "Object should be initialized");
    }

    [PowerTestTearDown]
    public void TearDown()
    {
        // Cleanup if needed
    }
}
