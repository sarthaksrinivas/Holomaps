﻿using UnityEngine;

/// <summary>
/// A line rendering the path.
/// </summary>
public class PathLine : MonoBehaviour {

    /// <summary>
    /// Sets the start and end points of the line.
    /// </summary>
    /// <param name="start">The start point of the line.</param>
    /// <param name="end">The end point of the line.</param>
    public void SetEndpoints(Vector3 start, Vector3 end) {
        transform.position = (start + end) / 2;
        transform.up = -(end - start);
        float distance = Vector3.Magnitude(start - end) / 2;
        Vector3 localScale = transform.localScale;
        localScale.y = distance;
        transform.localScale = localScale;
        if (distance != 0) {
            SpriteSheet spriteSheet = GetComponent<SpriteSheet>();
            spriteSheet._scale.y = 0.2f / distance;
            spriteSheet.CalcTextureSize();
        }
    }
}