using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HollowImage : UnityEngine.UI.MaskableGraphic, UnityEngine.ICanvasRaycastFilter
{
    public RectTransform HollowRect;
    public Vector2 pos = Vector2.zero;
    public Vector2 size = new Vector2(100, 100);

    private void Update()
    {
        if (!HollowRect) return;

        if (HollowRect && pos != HollowRect.anchoredPosition || size != HollowRect.sizeDelta)
        {
            pos = HollowRect.anchoredPosition;
            size = HollowRect.sizeDelta;
            SetVerticesDirty();
            SetLayoutDirty();
        }
    }

    static void AddQuad(UnityEngine.UI.VertexHelper vertexHelper, Vector2 posMin, Vector2 posMax, Color32 color)
    {
        int startIndex = vertexHelper.currentVertCount;

        var v = UnityEngine.UIVertex.simpleVert;

        v.position = new Vector3(posMin.x, posMin.y, 0);
        v.color = color;
        vertexHelper.AddVert(v);

        v.position = new Vector3(posMin.x, posMax.y, 0);
        v.color = color;
        vertexHelper.AddVert(v);

        v.position = new Vector3(posMax.x, posMax.y, 0);
        v.color = color;
        vertexHelper.AddVert(v);

        v.position = new Vector3(posMax.x, posMin.y, 0);
        v.color = color;
        vertexHelper.AddVert(v);

        vertexHelper.AddTriangle(startIndex, startIndex + 1, startIndex + 2);
        vertexHelper.AddTriangle(startIndex + 2, startIndex + 3, startIndex);
    }

    public bool IsRaycastLocationValid(Vector2 sp, Camera eventCamera)
    {
        return !RectTransformUtility.RectangleContainsScreenPoint(HollowRect, sp, eventCamera);
    }

    protected override void OnPopulateMesh(UnityEngine.UI.VertexHelper vh)
    {
        if (!HollowRect) return;

        Color32 color32 = color;

        vh.Clear();

        Rect innerRect = new Rect(pos, size);

        Rect outerRect = GetPixelAdjustedRect();

        AddQuad(vh, new Vector2(outerRect.xMin, outerRect.yMax), new Vector2(outerRect.xMax, innerRect.center.y), color);

        AddQuad(vh, new Vector2(outerRect.xMin, innerRect.center.y), new Vector2(HollowRect.anchoredPosition.x - innerRect.width/2, HollowRect.anchoredPosition.y - innerRect.height/2), color);

        AddQuad(vh, new Vector2(HollowRect.anchoredPosition.x + innerRect.width / 2, innerRect.center.y), new Vector2(outerRect.xMax, HollowRect.anchoredPosition.y -  innerRect.height/2), color);

        AddQuad(vh, new Vector2(outerRect.xMin, HollowRect.anchoredPosition.y - innerRect.height / 2), new Vector2(outerRect.xMax, -outerRect.yMax), color);

    }

}
