using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

public class BoxCenter : MonoBehaviour
{
    public Vector3 endPos = Vector3.zero;
    private void Start()
    {
        endPos = CenterObject();
    }

    public Vector3 CenterObject()
    {
        Vector2 startPos = transform.position;
        float blockDistance = GameManager.Instance.GetMoveDistance;
        float halfBlockDistance = GameManager.Instance.GetMoveDistance / 2;
        float modifierX = 0;
        float modifierY = 0;

        if (startPos.x >= 0)
            modifierX += halfBlockDistance;
        else
            modifierX -= halfBlockDistance;

        if (startPos.y >= 0)
            modifierY += halfBlockDistance;
        else
            modifierY -= halfBlockDistance;

        transform.position = new Vector3((int)(startPos.x / blockDistance) * blockDistance + modifierX, (int)(startPos.y / blockDistance) * blockDistance + modifierY);

        return transform.position;
    }
}

