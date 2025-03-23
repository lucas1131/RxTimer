# RxTimer
Simple timer application made using reactive programming

Technical assignment for my application at Telexistence.

## Application requirements
1. Implement Clock functionality and its UI to check current time on user time zone
2. Implement Timer functionality and its UI
	1. Can start, stop, reset and pause the timer
	2. Play sound when finished.
3. Implement Stopwatch functionality and its UI
	1. Can start, stop, reset and make lap time
	2. Show lap time when lapping
4. User can check current time when using Timer or Stopwatch
5. When Timer or Stopwatch is running, user can still use other functions
6. Make suitable UI for iOS/iPad devices

## Internal requirements
1. The code must implement Edit Mode and Play Mode tests
2. Code must have C# interfaces to integrate with other projects
3. Use Rx (Reactive Extensions)
4. Implement interface to divide dependency as Di (Dependency Injection)

## Team leader questions
1. As we said, this application will be used on iOS/iPad devices. Do you have any concern for UI?
A. For the most part no, expect maybe on really old devices with low resolutions the UI is a bit cramped but everything should still be functions. I have checked the UI on most low and high end devices for each manufacturer available in the built-in Unity simulator. I have also tested on my own Android phone. The interface seems to be properly responsive for mobile phones, just a bit cramped on windows.

2. How would you refactor the code and/or project after release? What would you prioritize as “must happen” versus “nice to have” changes
A. That depends on the direction the team wants to go. There are a few improvement opportunities. 
	1. There is a placeholder clock icon on the clock screen to indicate that it is, in fact, a clock, but since it's only an icon, there is no realtime animation or anything. This is probably a nice to have since you can still use the digital time regardless, so it's not crucial.
	2. I didn't separate the presenter from the view logic because the inteded application (as a clock) was way to simple in design and decided the complexity cost outweighted the benefits from this separation. However, if we were to integrate this in a bigger, more consolidated project that already has proper separation for presenter and view layers, then this should probably be a must have for code consistency.
	3. Currently each function is it's own instance with it's own counters and intervels. This is fine if the intention is to only have one app running anyway but if we wanted to integrate in a bigger project with multiple simultaneous clocks/stopwatches/timers running, this could become a bottleneck. One solution could be to have an underlaying time counter that all features use and they simply query that counter to calculate their own functions. If performance is critical, this would be a must have.
	4. As I was basing the design on my own phone's clock app, I ended up also creating the UI for the Alarm tab and even started sketching the code interface for it, but I then notices that the requirements didn't mention an alarm functionality. Because of this, I stopped working on it but this could be a nice feature to have as well. I decided instead to focus on the given requirements and leave the UI prototype as a pitch for a PO to decide if this should be included/prioritized.
	5. Persistence. It would be really easy to implement some sort of persistance manager, inject into each functionality and have them persist previous session values. For end-user, this is probably a must have.
	6. Realted to 5., having the option of changing timezone and adding multiple clocks could be useful for people who travel regularly. I think this is also a must have but this depends on what specific use case for this app is and the target user.
	7. Add proper support for network time instead of only relying on system time. There is a NetworkClockProvider class made just to show a proof of concept on how you could go about selecting which provider to use in the project, but it's logic is just a copy of the system provider with a different name. This could be a must have if we don't want users possibly manipulating the values by changing system time, if not, it's a nice to have.
	8. I also think that supporting dragging to switch tabs would really improve general usability, making it more accessible. I also believe this would be a nice to have.

3. [Optional] This application will be used on VR application. Share your concerns and your opinion on what need to take into account to support it in VR?
A. I rarely used VR so my knowledged on this is a bit limited. Everything here will be mostly based on what I feel would be a good or bad idea without having actually worked or used much VR.
I think a canvas based UI wouldn't work well, I have a feeling it would be really jittery since the canvas is an overlay and I'm not even sure how a canvas could be implemented in VR. I also don't think a point-and-click based UI, like the one in this app, would be suitable for VR, gestures would probably feel more natural, for example, looking at your wrists to bring up the clock app (or a hint on the control to bring up the app) and using the other hand to "drag" to switch between the menus.
Consequence of this, the UI will need to be completely remade to work as world UI instead of canvas UI. Also the UI needs to "follow" the user and gestures really smoothly since VR can easily cause motion sickness.

## Project dependencies
- [UniTask](https://github.com/Cysharp/UniTask)
- [UniRx](https://github.com/neuecc/UniRx)
- [Zenject](https://github.com/modesttree/Zenject)
- [NSubstitute](https://nsubstitute.github.io/) and a upm ready [Unity wrapper](https://github.com/Thundernerd/Unity3D-NSubstitute)
- [FluentAssertions](https://fluentassertions.com/) and a upm ready [Unity wrapper](https://github.com/BoundfoxStudios/fluentassertions-unity)
- [Google Material font icons](https://fonts.google.com/icons)
- [One royalty free alarm beep SFX](https://pixabay.com/sound-effects/bedside-clock-alarm-95792/)

## Time spent
- Initial setup and some random problems fixing: 2h
- UI: 4h
- Studying UniRx: 1h
- Clock functionality: about 40min
- Timer functionality: 2h
- Stopwatch functionality: 1h
- Editor tests + related refactors: 2h
- PlayMode tests: full 8h day
	- Relearning PlayMode tests: 1h
	- Troubleshooting PlayMode tests timing issues: 3h
	- Implementing tests and related refactors: 4h
- Timer beep: 15min
- UI polishing: 1h 30min
- Code cleanup and small refactors and improvements: 40min
- Total time: about 25h across 4 days (there is probably some untracked time here and there)

I tend to have a bit of a hard time with UI, I am more on the backend side so I am a bit slow on implementing UI, although I can do it eventually.

I am very used to writing edit mode tests and those were really simple to make but I hardly ever had the chance to work in a place with edit mode tests. I worked on a job that we had integration tests on the backend only but nothing on Unity, so I stumbled a lot and took some time to learn how to do it properly, and I still think there are a few problems with my PlayMode tests.

Also Unity 2021 (every version) was crashing on my personal computer. I had to borrow my brother's computer (which is a MacOS that I don't use) to work on this, which is way the initial setup time took so long and also slowed me down in general.


## Project decisions
### Model-View-Presenter pattern
I didn't follow the full MVP pattern as I didn't separate the View from the Presenter because I decided the UI implementation the UI controls were too simple for this usecase and the overhead wasn't worth it. It still has proper division between the Model and the Presenter. All business logic are in the model classes although they don't have the Model suffix, just their functionality name, like Stopwatch.

The presenter does not have any business logic but it does have UI state and UI update logic. These two responsibilities could be separated further into a proper View that only do UI updates in response to the presenter state changes for a true MVP pattern. 

Everything still uses Rx for observing, so dependencies are inverted. Tipically, the logic class have to reference the UI controller who references the UI class so the data goes from logic class -> controller -> UI, but the way it is now, the UI class (presenter) references the logic class (i.e stopwatch) and reacts to changes in the exposed properties using Rx properties. UI updates happen using Observables as well, so the data goes UI/Presenter <- logic class.

### Interfaces
Everything apart from the presenters have interfaces, with the exception of the ILapPresenter. The reason the other presenters doesn't have interfaces, is simply because they aren't injected anywhere. If true MVP pattern was used, with Presenter and View separated, they would have interfaces as well, but not the views, unless they also need to be injected.

Injection is the primary reason for the interfaces, but there is a second strong reason and that is testability. Having interfaces for everything facilitates mocking classes and allow for easy testing. Interfaces also server as an indication of what a class can do and how to do it, so it's like a built-in documentation. The last point, and in this case the least relevant, is extension points. And interface indicates that that behaviour can be switched for a new implementation if wanted. There is only one instance of this as a proof of concept with the SystemClockProvider and NetworkClockProvider both implementing IClock, and a flag in the ClockInstaller to choose which one to install, but there are a few other places this could be useful:

- If we wanted to allow more customization for displayed time, we could have a ITimeFormatter that has multiple ways, or even do dynamic time formatting to eliminate leading "00:"
- Implement another logger system that uses an actual logging service, like firebase. The only logger in the project uses Unity's Debug.Log, which isn't a production logger.

### Tests
Workflow and mindset are mostly inspired on Behaviour Driven Development, which is very close to Test Driven Development but has more focus on unifying domain language between Product Owner, Developer and QA. That is a big reason for why the tests are written using the Given-When-Then language, which is commonly used for user stories. The idea is that for every set of requirement Given-When-Then in an User Story, there is at least one matching set of Given-When-Then in the codebase's test and a matching Given-When-Then in the QA test plan.

For the tests structure, I kept the structure the same as the app code, except instead of having one test file for one class, I have one test file for each **expected behaviour** of a class. So for example, the **Timer** class could be located in

```/RxClock/Clock/Timer/Timer.cs```

And the respective test class would be in

```/Tests/[Editor|PlayMode]/RxClock/Clock/Timer/Timer/TimerShould.Start.cs```

Note that there is one extra directy that matches the class' original script name, to group all tests for that class in the same directory. I believe this keeps test files more organized and smaller, since tests classes can get big really fast because of all the mocks and assertions, and exceptionally faster in this case for using the Given-When-Then language. Here's an example of a test class:
```CSharp
public partial class TimerShould
{
    [Test]
    public void BeRunningWhenStartIsCalled()
    {
        GivenTimerIsStopped();
            
        WhenStarting(TimeSpan.FromMinutes(10));

        ThenTimerShouldBeRunning();
    }
    
    [Test]
    public void DoNothingWhenStartIsCalledIfAlreadyRunning()
    {
        GivenTimerIsRunning();
            
        WhenStarting(TimeSpan.FromMinutes(10));

        ThenTimerShouldBeRunning();
        ThenTimerShouldLogAboutDoingNothing();
    }
    
    [Test]
    public void DoNothingIfStartIsCalledWithZeroTime()
    {
        GivenTimerIsStopped();
            
        WhenStarting(TimeSpan.Zero);

        ThenTimerShouldNotBeRunning();
        ThenTimerShouldLogAboutDoingNothing();
    }

    private void GivenTimerIsStopped()
    {
        timer.Stop();
    }
    
    private void WhenStarting(TimeSpan timeToCount) => timer.Start(timeToCount);

    private void ThenTimerShouldBeRunning()
    {
        timer.IsRunning.Value.Should().BeTrue();
    }

    private void ThenTimerShouldLogAboutDoingNothing()
    {
        loggerMock.Received().Info(Arg.Any<string>()); 
    }
}
```

Some related resources:
https://www.agilealliance.org/glossary/bdd/
https://katalon.com/resources-center/blog/bdd-testing
https://martinfowler.com/bliki/GivenWhenThen.html


