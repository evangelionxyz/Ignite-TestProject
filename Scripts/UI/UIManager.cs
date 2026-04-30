using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Ignite;

namespace TestProject.UI;

internal class UIManager : Entity
{
    private WidgetComponent widget;
    private AudioSourceComponent hoverAudio;
    private AudioSourceComponent clickAudio;

    private WidgetButton startButton;
    private WidgetButton stopButton;
    private WidgetButton exitButton;

    public override void OnCreate()
    {
        Entity hoverEntity = FindChild("HoverSound");
        hoverAudio = hoverEntity.GetComponent<AudioSourceComponent>();

        Entity clickEntity = FindChild("ClickSound");
        clickAudio = clickEntity.GetComponent<AudioSourceComponent>();

        widget = GetComponent<WidgetComponent>();
        startButton = widget.GetButton("start_button");
        stopButton = widget.GetButton("stop_button");
        exitButton = widget.GetButton("exit_button");

        if (hoverAudio != null)
        {
            if (startButton != null) startButton.OnHoverEnterEvent += Buttons_OnHoverEnterEvent;
            if (stopButton != null) stopButton.OnHoverEnterEvent += Buttons_OnHoverEnterEvent;
            if (exitButton != null) exitButton.OnHoverEnterEvent += Buttons_OnHoverEnterEvent;
        }

        if (clickAudio != null)
        {
            if (startButton != null) startButton.OnClickEvent += Buttons_OnClickEvent;
            if (stopButton != null) stopButton.OnClickEvent += Buttons_OnClickEvent;
            if (exitButton != null) exitButton.OnClickEvent += Buttons_OnClickEvent;
        }
    }

    private void Buttons_OnHoverEnterEvent()
    {
        hoverAudio.Play();
    }

    private void Buttons_OnClickEvent()
    {
        clickAudio.Play();
    }

    public override void OnDestroy()
    {
        if (startButton != null) startButton.OnHoverEnterEvent -= Buttons_OnHoverEnterEvent;
        if (stopButton != null) stopButton.OnHoverEnterEvent -= Buttons_OnHoverEnterEvent;
        if (exitButton != null) exitButton.OnHoverEnterEvent -= Buttons_OnHoverEnterEvent;

        if (startButton != null) startButton.OnClickEvent -= Buttons_OnClickEvent;
        if (stopButton != null) stopButton.OnClickEvent -= Buttons_OnClickEvent;
        if (exitButton != null) exitButton.OnClickEvent -= Buttons_OnClickEvent;
    }

    public override void OnUpdate(float deltaTime)
    {   
    }
}
