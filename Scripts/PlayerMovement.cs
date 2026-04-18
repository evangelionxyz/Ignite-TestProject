using Ignite;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace TestProject;

public class PlayerMovement : Entity
{
    AudioSource? audioSource;
    Widget? widget;
    public override void OnCreate()
    {
        audioSource = GetComponent<AudioSource>();

        widget = GetComponent<Widget>();
        var startButton = widget!.GetButton("start_button");
        var optionsButton = widget!.GetButton("options_button");
        var exitButton = widget!.GetButton("exit_button");

        startButton!.OnHoverEnterEvent += Buttons_OnHoverEnter;
        optionsButton!.OnHoverEnterEvent += Buttons_OnHoverEnter;
        exitButton!.OnHoverEnterEvent += Buttons_OnHoverEnter;

        startButton!.OnHoverExitEvent += Buttons_OnHoverExit;
        optionsButton!.OnHoverExitEvent += Buttons_OnHoverExit;
        exitButton!.OnHoverExitEvent += Buttons_OnHoverExit;
    }

    public override void OnDestroy()
    {
        var startButton = widget!.GetButton("start_button");
        var optionsButton = widget!.GetButton("options_button");
        var exitButton = widget!.GetButton("exit_button");

        startButton!.OnHoverEnterEvent -= Buttons_OnHoverEnter;
        optionsButton!.OnHoverEnterEvent -= Buttons_OnHoverEnter;
        exitButton!.OnHoverEnterEvent -= Buttons_OnHoverEnter;

        startButton!.OnHoverExitEvent -= Buttons_OnHoverExit;
        optionsButton!.OnHoverExitEvent -= Buttons_OnHoverExit;
        exitButton!.OnHoverExitEvent -= Buttons_OnHoverExit;
    }


    private void Buttons_OnHoverExit()
    {
        Console.WriteLine("C# Button Hover Exit");
    }

    private void Buttons_OnHoverEnter()
    {
        audioSource!.Play();
        Console.WriteLine("C# Button Hover Entered");
    }

    public override void OnUpdate(float deltaTime)
    {
    }
}