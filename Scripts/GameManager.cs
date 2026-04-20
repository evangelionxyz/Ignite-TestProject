using Ignite;
using System;
using System.Collections.Generic;

namespace TestProject;

public class GameManager  : Entity
{
    public Entity cardTemplate;
    private List<Entity> cards = new List<Entity>();
    private List<Vector3> startPositions = new List<Vector3>();
    private List<Vector3> targetPositions = new List<Vector3>();
    private List<Vector3> startRotations = new List<Vector3>();
    private List<Vector3> targetRotations = new List<Vector3>();

    private bool isAnimating = false;
    private bool animationPlayed = false;
    private float animationTime = 0.0f;
    private float animationDuration = 1.2f; // seconds
    private float animationStartDelay = 0.2f; // small delay before animation
    private float delayTimer = 0.0f;
    public int cardCount = 8;
    public float cardSpacing = 0.5f; // horizontal spacing between cards
    public float startY = -3.0f; // bottom center start Y
    public float targetY = -1.5f; // target Y for the stacked cards
    public float maxFanAngle = 18.0f; // max angle in degrees for fanned cards

    public override void OnCreate()
    {
        if (cardTemplate == null)
            return;

        // instantiate cards at bottom center stacked together
        for (int i = 0; i < cardCount; i++)
        {
            var c = Instantiate(cardTemplate, new Vector3(0.0f, startY, 0.0f));
            c.Visible = true;
            cards.Add(c);

            // record start positions/rotations (all from bottom center)
            startPositions.Add(new Vector3(0.0f, startY, 0.0f));
            startRotations.Add(new Vector3(0.0f, 0.0f, 0.0f));
        }

        // compute targets: spread left-to-right and slightly fanned
        float mid = (cardCount - 1) / 2.0f;
        for (int i = 0; i < cardCount; i++)
        {
            float x = (i - mid) * cardSpacing;
            // slight vertical variation to mimic held cards
            float y = targetY + MathF.Abs(i + mid) * 0.03f;
            targetPositions.Add(new Vector3(x, y, 0.0f));

            // angle spreads across the hand, negative on left, positive on right
            float angle = ((i + mid) / MathF.Max(1.0f, mid)) * maxFanAngle;
            targetRotations.Add(new Vector3(0.0f, 0.0f, angle));
        }

        // start with a short delay then animate automatically (simulates "push")
        isAnimating = false;
        animationPlayed = false;
        delayTimer = 0.0f;
    }

    public override void OnDestroy()
    {
    }

    public override void OnUpdate(float deltaTime)
    {
        // wait for start delay then animate (only once)
        if (!isAnimating)
        {
            if (animationPlayed)
                return;

            delayTimer += deltaTime;
            if (delayTimer >= animationStartDelay)
            {
                isAnimating = true;
                animationTime = 0.0f;
                // reset delay timer so it doesn't immediately retrigger
                delayTimer = 0.0f;
            }
            return;
        }

        if (isAnimating)
        {
            animationTime += deltaTime;
            float t = MathF.Min(1.0f, animationTime / animationDuration);
            // ease out cubic for smoother stop
            float ease = 1 - MathF.Pow(1 - t, 3);

            for (int i = 0; i < cards.Count; i++)
            {
                var card = cards[i];
                var start = startPositions[i];
                var target = targetPositions[i];

                card.Translation = Vector3.Lerp(start, target, ease);

                var sR = startRotations[i];
                var tR = targetRotations[i];
                // convert Euler angles to a Quaternion for the engine rotation
                card.Rotation = Quaternion.Euler
                (
                    Lerp(sR.X, tR.X, ease),
                    Lerp(sR.Y, tR.Y, ease),
                    Lerp(sR.Z, tR.Z, ease)
                );
            }

            if (t >= 1.0f)
            {
                isAnimating = false;
                animationPlayed = true; // mark complete so it doesn't restart
            }
        }
    }

    // Helper linear interpolation
    private static float Lerp(float a, float b, float t)
    {
        return a + (b - a) * t;
    }
}