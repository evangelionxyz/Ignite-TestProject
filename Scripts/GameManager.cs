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

    private Entity lastHovered = null;
    private float hoverOffset = 0.25f; // how much to move forward when hovered
    private float hoverSpeed = 8.0f;
    private string hoverLastName = null;

    private bool isAnimating = false;
    private bool animationPlayed = false;
    private float animationTime = 0.0f;
    private float animationDuration = 1.2f;
    private float animationStartDelay = 0.2f; // small delay before animation
    private float delayTimer = 0.0f;
    // single-card animation when moving only the hovered entity to the end
    private bool singleCardAnimating = false;
    private Entity singleAnimatingEntity = null;
    private Vector3 singleStartPos;
    private Vector3 singleTargetPos;
    private float singleAnimTime = 0.0f;
    private float singleAnimDuration = 0.5f;
    public int cardCount = 8;
    public float cardSpacing = 0.5f; // horizontal spacing between cards
    public float startZ = -3.0f; // bottom center start Y
    public float targetZ = -1.5f; // target Y for the stacked cards
    public float maxFanAngle = 18.0f; // max angle in degrees for fanned cards

    public override void OnCreate()
    {
        if (cardTemplate == null)
            return;

        // ensure previous state is cleared if OnCreate is called more than once
        cards.Clear();
        startPositions.Clear();
        startRotations.Clear();
        targetPositions.Clear();
        targetRotations.Clear();

        for (int i = 0; i < cardCount; i++)
        {
            var c = Instantiate(cardTemplate, new Vector3(0.0f, 0.0f, startZ));
            c.Visible = true;
            cards.Add(c);

            startPositions.Add(new Vector3(0.0f, 0.0f, startZ));
            // store rotations in degrees for consistency with Quaternion.Euler
            // store rotations in radians (use Deg2Rad as requested)
            startRotations.Add(new Vector3(0.0f, -90.0f * Mathf.Deg2Rad, 0.0f));
        }

        RecomputeTargets();

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
        Entity pickEntity = PickEntityAt(Input.MousePosition.X, Input.MousePosition.Y);

        // determine hovered card (match by name to be robust if PickEntityAt returns a different instance)
        Entity hovered = null;
        if (pickEntity != null)
        {
            var pickName = pickEntity.GetName();
            for (int i = 0; i < cards.Count; i++)
            {
                var c = cards[i];
                if (c.GetName() == pickName)
                {
                    hovered = c;
                    break;
                }
            }
        }

        // wait for start delay then animate (only once)
        // Only run the start-delay logic if we haven't played the initial animation yet.
        // Do NOT return here so that hover handling and other update logic still run after
        // the first animation has completed.
        if (!isAnimating && !animationPlayed)
        {
            delayTimer += deltaTime;
            if (delayTimer >= animationStartDelay)
            {
                isAnimating = true;
                animationTime = 0.0f;
                // reset delay timer so it doesn't immediately retrigger
                delayTimer = 0.0f;
            }
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
                // convert interpolated Euler angles (stored in radians) to degrees for Quaternion.Euler
                float rx = Lerp(sR.X, tR.X, ease) * Mathf.Rad2Deg;
                float ry = Lerp(sR.Y, tR.Y, ease) * Mathf.Rad2Deg;
                float rz = Lerp(sR.Z, tR.Z, ease) * Mathf.Rad2Deg;
                card.Rotation = Quaternion.Euler(rx, ry, rz);
            }

            if (t >= 1.0f)
            {
                isAnimating = false;
                animationPlayed = true; // mark complete so it doesn't restart
            }
        }

        // If not animating, handle hover smoothing, single-card animations and reordering
        if (!isAnimating)
        {
            // process any single-card animation (only the hovered entity should animate to its new spot)
            if (singleCardAnimating && singleAnimatingEntity != null)
            {
                singleAnimTime += deltaTime;
                float t2 = MathF.Min(1.0f, singleAnimTime / singleAnimDuration);
                float ease2 = 1 - MathF.Pow(1 - t2, 3);
                singleAnimatingEntity.Translation = Vector3.Lerp(singleStartPos, singleTargetPos, ease2);
                if (t2 >= 1.0f)
                {
                    singleCardAnimating = false;
                    singleAnimatingEntity = null;
                }
            }

            // if hovering, smoothly nudge hovered card forward
            if (hovered != null)
            {
                lastHovered = hovered;
                var hoveredName = hovered.GetName();
                if (hoverLastName != hoveredName)
                {
                    hoverLastName = hoveredName;
                    Console.WriteLine($"Hover enter: {hoverLastName}");
                }

                for (int i = 0; i < cards.Count; i++)
                {
                    var card = cards[i];
                    var target = targetPositions[i];
                    float desiredZ = target.Z + (card == hovered ? hoverOffset : 0.0f);
                    var desired = new Vector3(target.X, target.Y, desiredZ);
                    var before = card.Translation;
                    var after = Vector3.Lerp(before, desired, MathF.Min(1.0f, deltaTime * hoverSpeed));
                    card.Translation = after;
                    // debug when hovered card movement is small or not applied
                    if (card == hovered)
                    {
                        Console.WriteLine($"Hovered move {card.GetName()} from {before} to {after} desiredZ={desiredZ}");
                    }
                }
            }
            else
            {
                // not hovering: if we had a lastHovered, move only that entity to the end
                if (lastHovered != null)
                {
                    Console.WriteLine($"Hover exit: {lastHovered.GetName()}");
                    if (cards.Contains(lastHovered))
                    {
                        // compute a target X to the right of the current right-most card (excluding the hovered one)
                        float rightmostX = float.MinValue;
                        for (int i = 0; i < cards.Count; i++)
                        {
                            var c = cards[i];
                            if (c == lastHovered)
                                continue;
                            rightmostX = MathF.Max(rightmostX, c.Translation.X);
                        }
                        if (rightmostX == float.MinValue)
                            rightmostX = 0.0f;

                        var start = lastHovered.Translation;
                        var newX = rightmostX + cardSpacing;
                        // keep Y and Z the same as the hovered card's current translation for a simple move
                        var newTarget = new Vector3(newX, start.Y, start.Z);

                        // update ordering but DO NOT recompute global targets or start the full layout animation
                        cards.Remove(lastHovered);
                        cards.Add(lastHovered);

                        // setup single-card animated move for only the hovered entity
                        singleStartPos = start;
                        singleTargetPos = newTarget;
                        singleAnimatingEntity = lastHovered;
                        singleAnimTime = 0.0f;
                        singleAnimDuration = 0.5f;
                        singleCardAnimating = true;
                    }
                    lastHovered = null;
                    hoverLastName = null;
                }
            }
        }
    }

    private void RecomputeTargets()
    {
        targetPositions.Clear();
        targetRotations.Clear();
        // compute mid based on actual number of cards
        float mid = (cards.Count - 1) / 2.0f;
        for (int i = 0; i < cards.Count; i++)
        {
            float x = (i - mid) * cardSpacing;
            float z = targetZ + MathF.Abs(i - mid) * 0.03f;
            targetPositions.Add(new Vector3(x, 0.0f, z));
            float angle = ((i - mid) / MathF.Max(1.0f, mid)) * maxFanAngle;
            // calculate angle in radians using Deg2Rad as requested
            float angleRad = angle * Mathf.Deg2Rad;
            float yRotRad = -90.0f * Mathf.Deg2Rad;
            targetRotations.Add(new Vector3(0.0f, yRotRad, angleRad));
        }
    }

    private void StartLayoutAnimation(float duration)
    {
        // capture current translations/rotations as start
        startPositions.Clear();
        startRotations.Clear();
        for (int i = 0; i < cards.Count; i++)
        {
            var c = cards[i];
            startPositions.Add(c.Translation);
            // approximate start rotation by using current target rotation if available
            if (i < targetRotations.Count)
                startRotations.Add(targetRotations[i]);
            else
                startRotations.Add(new Vector3(0, -90.0f * Mathf.Deg2Rad, 0));
        }
        animationDuration = duration;
        animationTime = 0.0f;
        isAnimating = true;
        animationPlayed = false;
    }

    // Helper linear interpolation
    private static float Lerp(float a, float b, float t)
    {
        return a + (b - a) * t;
    }
}