using Gameplay;
using UnityEngine;

[RequireComponent(typeof(RectTransform))]
public class FieldGenerator : MonoBehaviour
{
    [SerializeField] private GameObject horizontalLine;
    [SerializeField] private GameObject verticalLine;
    [SerializeField] private GameObject cell;

    [SerializeField] private Rules rules;

    private RectTransform rt;

    private RectTransform Rt
    {
        get
        {
            if (rt != null)
            {
                return rt;
            }

            rt = GetComponent<RectTransform>();
            return rt;
        }
    }

    /// <summary>
    /// Get field of empty cells. Size is determined by rules object.
    /// </summary>
    /// <returns>Two dimensional array of empty cells</returns>
    public Cell[,] GenerateField()
    {
        var fieldDimensions = rules.FieldDimensions;
        var distance = Rt.rect.width / fieldDimensions;

        for (var i = 1; i < fieldDimensions; i++)
        {
            var horizontalLineRt = Instantiate(horizontalLine, Rt).GetComponent<RectTransform>();
            horizontalLineRt.localPosition += new Vector3(0, -distance * i);

            var verticalLineRt = Instantiate(verticalLine, Rt).GetComponent<RectTransform>();
            verticalLineRt.localPosition += new Vector3(distance * i, 0);
        }

        var cells = new Cell[fieldDimensions, fieldDimensions];
        for (var i = 0; i < fieldDimensions; i++)
        {
            for (var j = 0; j < fieldDimensions; j++)
            {
                var cellRt = Instantiate(cell, Rt).GetComponent<RectTransform>();
                cellRt.localPosition += new Vector3(distance * j, -distance * i);
                cellRt.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, distance);
                cellRt.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, distance);

                cells[i,j] = cellRt.GetComponent<Cell>();
            }
        }

        return cells;
    }
}