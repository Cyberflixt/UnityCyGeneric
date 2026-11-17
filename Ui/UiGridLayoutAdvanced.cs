using UnityEngine;
using UnityEngine.UI;

[AddComponentMenu("Layout/UI Grid Layout Advanced")]
public class UiGridLayoutAdvanced : LayoutGroup
{
    /// <summary>
    /// Which corner is the starting corner for the grid.
    /// </summary>
    public enum Corner
    {
        UpperLeft = 0,
        UpperRight = 1,
        LowerLeft = 2,
        LowerRight = 3
    }

    /// <summary>
    /// The grid axis we are looking at.
    /// </summary>
    public enum Axis
    {
        Horizontal = 0,
        Vertical = 1
    }

    [SerializeField] protected Dim3 m_cellWidth;
    /// <summary>
    /// Width of the grid cell
    /// </summary>
    public Dim3 cellWidth { get { return m_cellWidth; } set { SetProperty(ref m_cellWidth, value); } }

    [SerializeField] protected Dim3 m_cellHeight;
    /// <summary>
    /// Height of the grid cell
    /// </summary>
    public Dim3 cellHeight { get { return m_cellHeight; } set { SetProperty(ref m_cellHeight, value); } }

    [SerializeField] protected Corner m_StartCorner = Corner.UpperLeft;

    /// <summary>
    /// Which corner should the first cell be placed in?
    /// </summary>
    public Corner startCorner { get { return m_StartCorner; } set { SetProperty(ref m_StartCorner, value); } }

    [SerializeField] protected Axis m_StartAxis = Axis.Horizontal;

    /// <summary>
    /// Which axis should cells be placed along first
    /// </summary>
    public Axis startAxis { get { return m_StartAxis; } set { SetProperty(ref m_StartAxis, value); } }

    [SerializeField] protected Vector2 m_Spacing = Vector2.zero;

    /// <summary>
    /// The spacing to use between layout elements in the grid on both axises.
    /// </summary>
    public Vector2 spacing { get { return m_Spacing; } set { SetProperty(ref m_Spacing, value); } }

    /// <summary>
    /// The current number of rows in the grid.
    /// </summary>
    public int rowCount { get; private set; }

    /// <summary>
    /// The current number of columns in the grid.
    /// </summary>
    public int columnCount { get; private set; }

    /// <summary>
    /// Helper method to calculate actual pixel size of cells based on Dim3 constraints and current RectTransform size.
    /// Note: Treats Percentage fields as 0-100 (divided by 100f).
    /// </summary>
    private Vector2 GetCalculatedCellSize()
    {
        float parentWidth = rectTransform.rect.width;
        float parentHeight = rectTransform.rect.height;

        float w = m_cellWidth.px + 
                 (m_cellWidth.xPercentage / 100f * parentWidth) + 
                 (m_cellWidth.yPercentage / 100f * parentHeight);

        float h = m_cellHeight.px + 
                 (m_cellHeight.xPercentage / 100f * parentWidth) + 
                 (m_cellHeight.yPercentage / 100f * parentHeight);

        return new Vector2(Mathf.Max(0, w), Mathf.Max(0, h));
    }

    /// <summary>
    /// Calculates the current column and row count based on child count and available space.
    /// </summary>
    private void CalculateGridDimensions(Vector2 cellSize, out int actualColumns, out int actualRows)
    {
        if (rectChildren.Count == 0)
        {
            actualColumns = 0;
            actualRows = 0;
            return;
        }

        float innerWidth = rectTransform.rect.width - padding.horizontal;
        float innerHeight = rectTransform.rect.height - padding.vertical;

        // Protect against zero division if settings are 0
        float spaceAndCellX = Mathf.Max(0.001f, cellSize.x + m_Spacing.x);
        float spaceAndCellY = Mathf.Max(0.001f, cellSize.y + m_Spacing.y);

        if (m_StartAxis == Axis.Horizontal)
        {
            int cellsPerMainAxis = Mathf.Max(1, Mathf.FloorToInt((innerWidth + m_Spacing.x + 0.001f) / spaceAndCellX));
            actualColumns = cellsPerMainAxis;
            actualRows = Mathf.CeilToInt(rectChildren.Count / (float)actualColumns);
        }
        else
        {
            int cellsPerMainAxis = Mathf.Max(1, Mathf.FloorToInt((innerHeight + m_Spacing.y + 0.001f) / spaceAndCellY));
            actualRows = cellsPerMainAxis;
            actualColumns = Mathf.CeilToInt(rectChildren.Count / (float)actualRows);
        }
    }

    /// <summary>
    /// Computes the 0-indexed (column, row) coordinate for a given active child index.
    /// </summary>
    private Vector2Int CalculateCellCoordinate(int index, int actualColumns, int actualRows)
    {
        int positionX, positionY;

        if (m_StartAxis == Axis.Horizontal)
        {
            positionX = index % actualColumns;
            positionY = index / actualColumns;
        }
        else
        {
            positionX = index / actualRows;
            positionY = index % actualRows;
        }

        // Adjust X/Y indexes based on the chosen starting corner
        if (m_StartCorner == Corner.UpperRight || m_StartCorner == Corner.LowerRight)
            positionX = actualColumns - 1 - positionX;

        if (m_StartCorner == Corner.LowerLeft || m_StartCorner == Corner.LowerRight)
            positionY = actualRows - 1 - positionY;

        return new Vector2Int(positionX, positionY);
    }

    /// <summary>
    /// Returns the 0-indexed (column, row) coordinate of an active layout child by its index in rectChildren.
    /// Returns (-1, -1) if the index is out of bounds.
    /// </summary>
    public Vector2Int GetCellCoordinate(int childIndex)
    {
        if (childIndex < 0 || childIndex >= rectChildren.Count)
            return new Vector2Int(-1, -1);

        // Ensure dimensions are accurate even if called before a layout rebuild completes
        Vector2 cellSize = GetCalculatedCellSize();
        CalculateGridDimensions(cellSize, out int actualColumns, out int actualRows);

        return CalculateCellCoordinate(childIndex, actualColumns, actualRows);
    }

    /// <summary>
    /// Returns the 0-indexed (column, row) coordinate of a specific RectTransform child.
    /// Returns (-1, -1) if the item is not an active layout child of this grid.
    /// </summary>
    public Vector2Int GetCellCoordinate(RectTransform item)
    {
        if (item == null)
            return new Vector2Int(-1, -1);

        int index = rectChildren.IndexOf(item);
        return GetCellCoordinate(index);
    }

    /// <summary>
    /// Called by the layout system to calculate the horizontal layout size.
    /// </summary>
    public override void CalculateLayoutInputHorizontal()
    {
        base.CalculateLayoutInputHorizontal();

        int minColumns = 0;
        int preferredColumns = 0;

        // Give basic sizing estimates back to the Layout System
        if (m_StartAxis == Axis.Horizontal)
        {
            minColumns = 1;
            preferredColumns = Mathf.CeilToInt(Mathf.Sqrt(rectChildren.Count));
        }
        else
        {
            minColumns = Mathf.CeilToInt(Mathf.Sqrt(rectChildren.Count));
            preferredColumns = Mathf.CeilToInt(Mathf.Sqrt(rectChildren.Count));
        }

        Vector2 cellSize = GetCalculatedCellSize();
        float minSpace = padding.horizontal + (cellSize.x + m_Spacing.x) * minColumns - m_Spacing.x;
        float prefSpace = padding.horizontal + (cellSize.x + m_Spacing.x) * preferredColumns - m_Spacing.x;

        SetLayoutInputForAxis(Mathf.Max(0, minSpace), Mathf.Max(0, prefSpace), -1, 0);
    }

    /// <summary>
    /// Called by the layout system to calculate the vertical layout size.
    /// </summary>
    public override void CalculateLayoutInputVertical()
    {
        int minRows = 0;
        Vector2 cellSize = GetCalculatedCellSize();

        if (m_StartAxis == Axis.Horizontal)
        {
            float innerWidth = rectTransform.rect.width - padding.horizontal;
            float cellAndSpacingX = Mathf.Max(0.001f, cellSize.x + m_Spacing.x);
            int cellsPerMainAxis = Mathf.Max(1, Mathf.FloorToInt((innerWidth + m_Spacing.x + 0.001f) / cellAndSpacingX));
            minRows = Mathf.CeilToInt(rectChildren.Count / (float)cellsPerMainAxis);
        }
        else
        {
            minRows = 1;
        }

        float minSpace = padding.vertical + (cellSize.y + m_Spacing.y) * minRows - m_Spacing.y;
        SetLayoutInputForAxis(Mathf.Max(0, minSpace), Mathf.Max(0, minSpace), -1, 1);
    }

    public override void SetLayoutHorizontal()
    {
        SetCellsAlongAxis(0);
    }

    public override void SetLayoutVertical()
    {
        SetCellsAlongAxis(1);
    }

    private void SetCellsAlongAxis(int axis)
    {
        Vector2 cellSize = GetCalculatedCellSize();
        CalculateGridDimensions(cellSize, out int actualColumns, out int actualRows);

        columnCount = actualColumns;
        rowCount = actualRows;

        if (rectChildren.Count == 0) return;

        // Space required without padding (GetStartOffset will incorporate standard LayoutGroup childAlignment + padding)
        float requiredSpaceX = actualColumns * cellSize.x + (actualColumns - 1) * m_Spacing.x;
        float requiredSpaceY = actualRows * cellSize.y + (actualRows - 1) * m_Spacing.y;

        float startOffsetMainX = GetStartOffset(0, requiredSpaceX);
        float startOffsetMainY = GetStartOffset(1, requiredSpaceY);

        for (int i = 0; i < rectChildren.Count; i++)
        {
            Vector2Int coord = CalculateCellCoordinate(i, actualColumns, actualRows);

            // Apply positions
            if (axis == 0)
            {
                float posX = startOffsetMainX + coord.x * (cellSize.x + m_Spacing.x);
                SetChildAlongAxis(rectChildren[i], 0, posX, cellSize.x);
            }
            else
            {
                float posY = startOffsetMainY + coord.y * (cellSize.y + m_Spacing.y);
                SetChildAlongAxis(rectChildren[i], 1, posY, cellSize.y);
            }
        }
    }
}