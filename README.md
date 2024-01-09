# PowerTest

## Description

PowerTest is a lightweight testing system developed as an alternative to Unity's TestRunner (nunit). Despite its rapid implementation, PowerTest offers effective testing tools that can enhance your development process. The system is focused on speed and ease of use, especially when working with asynchronous code and dynamically loaded assets.

### Advantages and Disadvantages Compared to TestRunner

#### Advantages:

    Performance: Tests do not generate analytics, significantly speeding up their execution.
    Asynchronous Support: Full support for asynchronous code and dynamic asset loading.
    Breakpoints: Ability to use breakpoints in the development environment to track test results.
    Flexibility in Setup: Ability to create multiple entry points (Setup).
    Isolated and Contextual Tests: Tests can be both isolated and context-aware.
    Asserts Class: An implemented Asserts class, which you can extend.
    Determinism: Deterministic execution of tests within a class.
    Integration with Zenject and UniTask: Examples of using Zenject, UniTask, and pure Tasks.
    Detailed Log: The log includes the time taken, error information, and test results.

#### Disadvantages:

    Output Structure: The test result output may not be sufficiently structured.
    Lack of Navigation: No ability to navigate through tests.
    Editor Mode Limitations: Inability to perform tests in the editor mode.
    Lack of External Logging: Absence of external logging capabilities.
    Lack of Secondary Attributes: Shortage of secondary attributes for more detailed test configurations.




### Technical Requirements

    Unity: Version is not critical; compatible with various Unity versions.
    Standard Libraries: No dependency on non-standard libraries.

In case of adding the Zenject and UniTask directives, you will have access to examples of their usage, as well as a special implementation considering UniTask. This allows for the continued use of standard Task as before.

### Installation and Setup

To install PowerTest, follow these steps:

    Clone the Repository:
        Use the following command to clone the PowerTest repository:
        git clone https://github.com/SenatorInPower/PowerTest.git

    Open the Project in Unity:
        Open Unity Hub.
        Click on 'Add' and select the cloned project folder.

    Install Dependencies (If Using Zenject and UniTask):
        If you plan to use Zenject and UniTask, ensure that they are installed and configured in your Unity project.

Now, PowerTest is ready for use in your Unity project. You can begin writing and running tests using the PowerTest framework.

### Usage

## To use PowerTest in your Unity project, follow these instructions:

    Access PowerTest:
        In the Unity editor, click on PowerTest in the top menu bar.

    Start Game:
        If your tests are already written (examples can be found in the Example folder), click on StartGame.
        This step is necessary to prepare the environment for running the tests.

    Run Tests During Play Mode:
        Once in Play mode, you will see the RunTest button.
        Click this button to start the execution of the tests.

    Observing Test Execution:
        Initially, you may notice a delay of about 6 seconds as the tests are executed. This is because the tests will be completed after all of them have run, and in the example, there is an asynchronous delay of 5 seconds.

    Using Zenject and UniTask:
        If you wish to use Zenject or UniTask in your tests, make sure to add the respective directives in your test scripts.
        Refer to the provided example for Zenject injection, which is conducted locally through the setup process.


### Writing Tests

## When writing tests with PowerTest, you can use the following attributes to structure your test cases:

    [PowerTestSetup]:
        This attribute marks methods that are executed at the beginning, typically used for initialization.
        These methods can also fail and utilize asserts for validation.

    [PowerTest]:
        Use this attribute to denote your main test methods.
        These are the core tests that will validate the functionality of your code.

    [PowerIsolatedTest]:
        Methods with this attribute will have all references in the class reset.
        Initialization done in these methods will not be taken into account in other tests, ensuring isolation.

    [PowerTestTearDown]:
        This attribute is for finalizing methods that run at the end of your test cycle.
        Typically used for cleanup and releasing resources.
