# PowerTest

## Description

PowerTest is a lightweight testing system developed as an alternative to Unity's TestRunner (nunit). Despite its rapid implementation, PowerTest offers effective testing tools that can enhance your development process. The system is focused on speed and ease of use, especially when working with asynchronous code and dynamically loaded assets.


[![Watch the video](https://github.com/SenatorInPower/PowerTest/assets/66920423/7c9af3f6-eae0-47bc-aee2-44d0d6fec4b2)](https://github.com/SenatorInPower/PowerTest/assets/66920423/7c9af3f6-eae0-47bc-aee2-44d0d6fec4b2)


### Advantages and Disadvantages Compared to TestRunner

Advantages:

    ➕ Performance: Tests do not generate analytics, significantly speeding up their execution.
    ➕ Asynchronous Support: Full support for asynchronous code and dynamic asset loading.
    ➕ Breakpoints: Ability to use breakpoints in the development environment to track test results.
    ➕ Flexibility in Setup: Ability to create multiple entry points (Setup).
    ➕ Isolated and Contextual Tests: Tests can be both isolated and context-aware.
    ➕ Asserts Class: An implemented Asserts class, which you can extend.
    ➕ Determinism: Deterministic execution of tests within a class.
    ➕ Integration with Zenject and UniTask: Examples of using Zenject, UniTask, and pure Tasks.
    ➕ Detailed Log: The log includes the time taken, error information, and test results.

Disadvantages

    ➖ Output Structure: The test result output may not be sufficiently structured.
    ➖ Lack of Navigation: No ability to navigate through tests.
    ➖ Editor Mode Limitations: Inability to perform tests in the editor mode.
    ➖ Lack of External Logging: Absence of external logging capabilities.
    ➖ Lack of Secondary Attributes: Shortage of secondary attributes for more detailed test configurations.


### Technical Requirements

- **Unity**: Compatible with various Unity versions. The specific version is not critical.
- **Standard Libraries**: There is no dependency on non-standard libraries.


In case of adding the Zenject and UniTask directives, you will have access to examples of their usage, as well as a special implementation considering UniTask. This allows for the continued use of standard Task as before.

### Installation and Setup

To install PowerTest, follow these steps:

 🔗 Cloning the Repository

   To get started, clone the PowerTest repository using the following command:

   git clone https://github.com/SenatorInPower/PowerTest.git

🎮 Opening the Project in Unity

    Launch Unity Hub 🌐.
    Click on the 'Add' button ➕.
    Navigate to and select the cloned project folder 📁.

   ⚙️ Installing Dependencies

Important: If you're using Zenject and UniTask in your project, follow these steps to ensure they are properly installed and configured:

    Confirm that Zenject and UniTask are installed in your Unity project ✅.
    Follow the specific installation and configuration instructions provided for these dependencies 📘.

Now, PowerTest is ready for use in your Unity project. You can begin writing and running tests using the PowerTest framework.

## Usage

### To use PowerTest in your Unity project, follow these instructions:

🎮 Access PowerTest:

  In the Unity Editor: Click on PowerTest in the top menu bar.

🚀 Start Game:

 Pre-Written Tests: If your tests are already written (see examples in the Example folder), click on StartGame. This is necessary to prepare the environment for running the tests.

▶️ Run Tests During Play Mode:

 Play Mode: Once in Play mode, you will see the RunTest button.
 Test Execution: Click this button to start the execution of the tests.

🔍 Observing Test Execution:

 Execution Delay: Initially, you may notice a delay of about 6 seconds as the tests are executed. This is normal as the tests will complete after all of them have run. Note: In the example, there is an asynchronous delay of 5 seconds.

🛠️ Using Zenject and UniTask:

 Dependencies: If you wish to use Zenject or UniTask in your tests, ensure to add the respective directives in your test scripts.
 Zenject Example: Refer to the provided example for Zenject injection, which is conducted locally through the setup process.


## Writing Tests

### When writing tests with PowerTest, you can use the following attributes to structure your test cases:

[PowerTestSetup]:

 🔹 Initialization Attribute: This attribute marks methods that are executed at the beginning, typically used for initialization.
     Validation: These methods can also fail and utilize asserts for validation.

[PowerTest]:

  🔹 Main Test Method Attribute: Use this attribute to denote your main test methods.
     Core Validation: These are the core tests that will validate the functionality of your code.

[PowerIsolatedTest]:

🔹 Isolation Attribute: Methods with this attribute will have all references in the class reset.
 Ensuring Isolation: Initialization done in these methods will not be taken into account in other tests, ensuring isolation.

[PowerTestTearDown]:

🔹 Finalizing Attribute: This attribute is for finalizing methods that run at the end of your test cycle.
   Cleanup: Typically used for cleanup and releasing resources.
