using Ignite;
using System;
using System.Collections.Generic;
using TestProject.Scripts;
using TestProject.Scripts.Cards;
using TestProject.Scripts.Run;
using TestProject.Scripts.Run.Misc;

using static Ignite.Mathf;
namespace TestProject;

public class GameManager  : Entity
{
    enum EMyEnum
    {
        Default = 0,
        Test,
        HelloWorld
    }

    [SerializeField]
    private EMyEnum myEnum = EMyEnum.Default;
    
    [SerializeField]
    private Entity cardMining;
    
    [SerializeField]
    private Entity cardReforestation;
    
    [SerializeField]
    private Entity cardEducation;

    private List<Entity> cards = new List<Entity>();
    private List<Vector3> startPositions = new List<Vector3>();
    private List<Vector3> targetPositions = new List<Vector3>();
    private List<Vector3> startRotations = new List<Vector3>();
    private List<Vector3> targetRotations = new List<Vector3>();

    private Entity lastHoveredEntity = null;

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

    [SerializeField]
    private int cardCount = 8;

    [SerializeField]
    private float cardSpacing = 0.5f;

    [SerializeField]
    private float startZ = -3.0f;
    
    [SerializeField]
    private float targetZ = -1.5f;
    
    [SerializeField]
    private float maxFanAngle = 18.0f;

    private Run _run;

    public override void OnCreate()
    {
        if (cardMining == null || cardEducation == null || cardReforestation == null)
            return;

        cards.Clear();
        startPositions.Clear();
        startRotations.Clear();

        // prepare an array of the three card prefabs and a single RNG
        var prototypes = new Entity[] { cardMining, cardReforestation, cardEducation };
        var rand = new Random();
        for (int i = 0; i < cardCount; i++)
        {
            // pick one of the three cards at random for this slot
            var prefab = prototypes[rand.Next(prototypes.Length)];
            var c = Instantiate(prefab, new Vector3(0.0f, 0.0f, startZ));
            c.Visible = true;
            cards.Add(c);

            startPositions.Add(new Vector3(0.0f, 0.0f, startZ));
            startRotations.Add(new Vector3(0.0f, -90.0f * Mathf.Deg2Rad, 0.0f));
        }

        RecomputeTargets();

        isAnimating = true;
        animationPlayed = false;
        delayTimer = 0.0f;
    }

    public override void OnDestroy()
    {
    }

    public override void OnUpdate(float deltaTime)
    {
        // Console.WriteLine(myString);
        Ray ray = Physics.ScreenToWorldRay(Input.MousePosition);
        Entity hovered = Physics.Raycast(ray);
        
        if (isAnimating && animationPlayed)
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

            if (animationTime <= deltaTime) // First frame of animation
            {
                Debug.Log($"GameManager: Animation Started - Duration: {animationDuration}, Cards: {cards.Count}");
            }

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
                Debug.Log("GameManager: Animation Finished");
            }
        }

        if (!isAnimating && animationPlayed)
        {
            if (hovered != null)
            {
                lastHoveredEntity = hovered;
                if (hoverLastID != hovered.ID)
                {
                    hoverLastID = hovered.ID;
                }
            }

            for (int i = 0; i < cards.Count; i++)
            {
                var card = cards[i];
                var targetPos = targetPositions[i];
                var targetRot = targetRotations[i];
                bool isHovered = (hovered != null) && (card == hovered || hovered.GetParent() == card);
                
                // Move with Z axis Forward/Backward
                float desiredZ = targetPos.Z + (isHovered ? hoverOffset : 0.0f);
                Vector3 desiredPos = new Vector3(targetPos.X, targetPos.Y, desiredZ);
                
                // Un-rotate the card so we can read it when hovered
                Vector3 desiredRotEuler = isHovered ? new Vector3(targetRot.X, targetRot.Y, 0.0f) : targetRot;
                Quaternion desiredRot = Quaternion.Euler(desiredRotEuler.X, desiredRotEuler.Y, desiredRotEuler.Z);

                float lerpFactor = MathF.Min(1.0f, deltaTime * hoverSpeed);
                card.Translation = Vector3.Lerp(card.Translation, desiredPos, lerpFactor);
                card.Rotation = LerpQuaternion(card.Rotation, desiredRot, lerpFactor);
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
            // Arc downwards on Y axis to create a fan shape
            float y = -MathF.Pow(MathF.Abs(i - mid), 2) * 0.05f;
            float z = targetZ + MathF.Abs(i - mid) * 0.01f;
            targetPositions.Add(new Vector3(x, y, z));
            
            float angle = -((i - mid) / MathF.Max(1.0f, mid)) * maxFanAngle;
            float yRotRad = -90.0f * Mathf.Deg2Rad;
            float zRotRad = angle * Mathf.Deg2Rad;
            targetRotations.Add(new Vector3(0.0f, yRotRad, zRotRad));
        }
    }

    private Quaternion LerpQuaternion(Quaternion a, Quaternion b, float t)
    {
        float dot = a.X * b.X + a.Y * b.Y + a.Z * b.Z + a.W * b.W;
        if (dot < 0.0f)
        {
            b.X = -b.X; b.Y = -b.Y; b.Z = -b.Z; b.W = -b.W;
        }
        return new Quaternion(
            Lerp(a.X, b.X, t),
            Lerp(a.Y, b.Y, t),
            Lerp(a.Z, b.Z, t),
            Lerp(a.W, b.W, t)
        ).Normalized;
    }

    // Helper linear interpolation
    private static float Lerp(float a, float b, float t)
    {
        return a + (b - a) * t;
    }

    public void NewRun(string seed)
    {
        _run = new Run(seed);
        BeginNewShopPhase();
    }

    private void BeginNewShopPhase()
    {
        if (_run == null) return;

        var shop = _run.StartShop();
        EventBus.EmitShopStarted(shop);
    }

    public void OnShopFinished() => BeginNewTermPhase();

    private void BeginNewTermPhase()
    {
        if (_run == null) return;

        var term = _run.StartTerm();
        term.DrawHand();
        EventBus.EmitTermStarted(term);
    }

    public void OnTermExecuted()
    {
        if (_run == null) return;
        if (_run.CurrentTerm == null) return;

        var result = _run.CurrentTerm.ExecuteTerm();

        CommitMetricsChanges(result);
        CommitEffects(result);

        if (CheckGameOver(result)) return;

        EventBus.EmitTermResolved(result);
        BeginNewShopPhase();
    }

    private bool CheckGameOver(TermContext result)
    {
        if (result is null) return false;
        if (_run == null) return false;

        if (_run.State.GdpTarget > result.TotalGdpGenerated)
        {
            EventBus.EmitGameOver(EGameOverReason.EconomicCollapse);
            return true;
        }

        if (_run.State.Biosphere < 0)
        {
            EventBus.EmitGameOver(EGameOverReason.EcologicalDisaster);
            return true;
        } 

        if (_run.State.LowClassApproval < 0 || _run.State.HighClassApproval < 0)
        {
            EventBus.EmitGameOver(EGameOverReason.Revolution);
            return true;
        }

        return false;
    }

    private void CommitEffects(TermContext result)
    {
        foreach (var cardId in result.CardIdToSpawn)
        {
            if (cardId == -1)
                continue;

            if (CardRegistry.Get(cardId) is not PolicyCard card)
                continue;

            _run.DeckManager.TryAddCardToDeck(card);
        }
    }

    private void CommitMetricsChanges(TermContext result)
    {
        var surplus = result.TotalGdpGenerated - _run.State.GdpTarget;
        _run.State.AddTreasury(surplus);
        _run.State.ApplyBiosphereChange(result.TotalBiosphereChange);
        _run.State.ApplyHighClassApprovalChange(result.TotalHighClassApprovalChange);
        _run.State.ApplyLowClassApprovalChange(result.TotalLowClassApprovalChange);
    }
}
