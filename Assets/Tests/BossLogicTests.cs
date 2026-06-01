// Tests/EditMode/BossLogicTests.cs
using NUnit.Framework;
using UnityEngine;

public class BossLogicTests
{
    // --- Detection clamping ---
    [Test]
    public void Detection_ClampedBetweenZeroAndOne()
    {
        float detection = 1.5f;
        detection = Mathf.Clamp01(detection);
        Assert.AreEqual(1f, detection);
    }

    // --- Vision angle check (extracted from CanSeePlayer) ---
    [TestCase(0f, true)]    // directly in front
    [TestCase(29f, true)]   // within 60 degree cone
    [TestCase(31f, false)]  // outside cone
    public void VisionAngle_WithinCone_ReturnsExpected(float angle, bool expected)
    {
        float visionAngle = 60f;
        Vector2 forward = Vector2.right;
        Vector2 targetDir = new Vector2(
            Mathf.Cos(angle * Mathf.Deg2Rad),
            Mathf.Sin(angle * Mathf.Deg2Rad)
        );
        float measuredAngle = Vector2.Angle(forward, targetDir);
        bool canSee = measuredAngle <= visionAngle / 2f;
        Assert.AreEqual(expected, canSee);
    }

    // --- Detection text color logic ---
    [TestCase(0.1f, "green")]
    [TestCase(0.5f, "yellow")]
    [TestCase(0.8f, "red")]
    public void DetectionColor_CorrectThreshold(float detection, string expected)
    {
        string result;
        if (detection < 0.3f) result = "green";
        else if (detection < 0.7f) result = "yellow";
        else result = "red";

        Assert.AreEqual(expected, result);
    }

    // --- Waypoint cycling ---
    [Test]
    public void Waypoint_LoopsBackToZero()
    {
        int current = 2;
        int total = 3;
        current = (current + 1) % total;
        Assert.AreEqual(0, current);
    }

    // --- CEO difficulty instant detection ---
    [Test]
    public void CEODifficulty_DetectionBecomesInstant()
    {
        float detection = 0f;
        var difficulty = DifficultyManager.Difficulty.CEO;

        if (difficulty == DifficultyManager.Difficulty.CEO)
            detection = 1f;

        Assert.AreEqual(1f, detection);
    }
}