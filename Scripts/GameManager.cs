using Ignite;
using System;
using System.Collections.Generic;

namespace TestProject;

public class GameManager  : Entity
{
    public Entity? cardTemplate;
    private List<Entity> cards = new List<Entity>();
    private List<Vector3> startPositions = new List<Vector3>();
    private List<Vector3> targetPositions = new List<Vector3>();
    private List<Vector3> startRotations = new List<Vector3>();
    private List<Vector3> targetRotations = new List<Vector3>();

    private Entity? lastHoveredEntity = null;

    [SerializeField]
    private float hoverOffset = 0.25f;
    private float hoverSpeed = 8.0f;
    private ulong hoverLastID = 0;

    private bool isAnimating = false;
    private bool animationPlayed = false;
    private float animationTime = 0.0f;
    private float animationDuration = 1.2f;
    private float animationStartDelay = 0.2f;
    private float delayTimer = 0.0f;

    private bool singleCardAnimating = false;
    private Entity? singleAnimatingEntity = null;
    private Vector3 singleStartPos;
    private Vector3 singleTargetPos;
    private float singleAnimTime = 0.0f;
    private float singleAnimDuration = 0.5f;
    public int cardCount = 8;
    public float cardSpacing = 0.5f;
    public float startZ = -3.0f;
    public float targetZ = -1.5f;
    public float maxFanAngle = 18.0f;

    public override void OnCreate()
    {
        if (cardTemplate == null)
            return;

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
            startRotations.Add(new Vector3(0.0f, -90.0f * Mathf.Deg2Rad, 0.0f));
        }

        RecomputeTargets();
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

        Entity? hovered = null;
        if (pickEntity != null)
        {
            for (int i = 0; i < cards.Count; i++)
            {
                var c = cards[i];
                if (c.ID == pickEntity.ID)
                {
                    hovered = c;
                    break;
                }
            }
        }

        if (!isAnimating && !animationPlayed)
        {
            delayTimer += deltaTime;
            if (delayTimer >= animationStartDelay)
            {
                isAnimating = true;
                animationTime = 0.0f;
                delayTimer = 0.0f;
            }
        }

        if (isAnimating)
        {
            animationTime += deltaTime;
            float t = MathF.Min(1.0f, animationTime / animationDuration);

            float ease = 1 - MathF.Pow(1 - t, 3);

            for (int i = 0; i < cards.Count; i++)
            {
                var card = cards[i];
                var start = startPositions[i];
                var target = targetPositions[i];

                card.Translation = Vector3.Lerp(start, target, ease);

                var sR = startRotations[i];
                var tR = targetRotations[i];
                float rx = Lerp(sR.X, tR.X, ease);
                float ry = Lerp(sR.Y, tR.Y, ease);
                float rz = Lerp(sR.Z, tR.Z, ease);
                card.Rotation = Quaternion.Euler(rx, ry, rz);
            }

            if (t >= 1.0f)
            {
                isAnimating = false;
                animationPlayed = true;
            }
        }

        if (!isAnimating)
        {
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

            if (hovered != null)
            {
                lastHoveredEntity = hovered;
                if (hoverLastID != hovered.ID)
                {
                    hoverLastID = hovered.ID;
                }

                for (int i = 0; i < cards.Count; i++)
                {
                    var card = cards[i];
                    var target = targetPositions[i];
                    float desiredZ = target.Z + (card == hovered ? hoverOffset : 0.0f);
                    var desired = new Vector3(card.Translation.X, card.Translation.Y, desiredZ);
                    var before = card.Translation;
                    var after = Vector3.Lerp(before, desired, MathF.Min(1.0f, deltaTime * hoverSpeed));
                    card.Translation = after;
                }
            }
            else
            {
                if (lastHoveredEntity != null)
                {
                    if (cards.Contains(lastHoveredEntity))
                    {
                        cards.Remove(lastHoveredEntity);
                        cards.Add(lastHoveredEntity);

                        singleStartPos = lastHoveredEntity.Translation;
                        singleTargetPos = lastHoveredEntity.Translation;
                        singleAnimatingEntity = lastHoveredEntity;
                        singleAnimTime = 0.0f;
                        singleAnimDuration = 0.5f;
                        singleCardAnimating = true;
                    }
                    lastHoveredEntity = null;
                    hoverLastID = 0;
                }
            }
        }
    }

    private void RecomputeTargets()
    {
        targetPositions.Clear();
        targetRotations.Clear();

        float mid = (cards.Count - 1) / 2.0f;
        for (int i = 0; i < cards.Count; i++)
        {
            float x = (i - mid) * cardSpacing;
            float z = targetZ + MathF.Abs(i - mid) * 0.03f;
            targetPositions.Add(new Vector3(x, 0.0f, z));
            float angle = ((i - mid) / MathF.Max(1.0f, mid)) * maxFanAngle;
            float yRotRad = -90.0f * Mathf.Deg2Rad;
            targetRotations.Add(new Vector3(0.0f, yRotRad, 0.0f));
        }
    }

    // Helper linear interpolation
    private static float Lerp(float a, float b, float t)
    {
        return a + (b - a) * t;
    }
}