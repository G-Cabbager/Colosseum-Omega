namespace MapSpawnerGUI;

/// <summary>核心生成逻辑</summary>
public static class Generator
{
    /// <summary>向上取整除法</summary>
    public static int CeilDiv(int a, int b) => (a + b - 1) / b;

    /// <summary>通用坐标生成</summary>
    public static List<(int X, int Y)> GeneratePositions(
        bool isRightDirection, int cornerX, int cornerY, int rows, int total)
    {
        var positions = new List<(int X, int Y)>();
        if (rows <= 0 || total <= 0) return positions;

        int cols = CeilDiv(total, rows);
        int baseCount = total / rows;
        int remainder = total % rows;
        int startFullRow = (rows - remainder) / 2;

        int topLeftX, topLeftY;
        if (isRightDirection)
        {
            topLeftX = cornerX - (cols - 1);
            topLeftY = cornerY + (cols - 1);
        }
        else
        {
            topLeftX = cornerX - (rows - 1);
            topLeftY = cornerY - (rows - 1);
        }

        const int dcX = 1;
        const int dcY = -1;

        for (int r = 0; r < rows; ++r)
        {
            int numInRow = baseCount;
            if (r >= startFullRow && r < startFullRow + remainder)
                numInRow = baseCount + 1;
            if (numInRow == 0) continue;

            int startCol, endCol;
            if (isRightDirection)
            {
                startCol = cols - numInRow;
                endCol = cols;
            }
            else
            {
                startCol = 0;
                endCol = numInRow;
            }

            int rowBaseX = topLeftX + r;
            int rowBaseY = topLeftY + r;

            for (int c = startCol; c < endCol; ++c)
            {
                int x = rowBaseX + c * dcX;
                int y = rowBaseY + c * dcY;
                positions.Add((x, y));
            }
        }
        return positions;
    }

    /// <summary>生成实体列表</summary>
    public static List<BaseEntity> GenerateEntities(
        bool isRightDirection, int cornerX, int cornerY, int rows, int total,
        BaseEntity baseTemplate, EntityType type)
    {
        var positions = GeneratePositions(isRightDirection, cornerX, cornerY, rows, total);
        var entities = new List<BaseEntity>();

        foreach (var pos in positions)
        {
            var e = new BaseEntity
            {
                Owner = baseTemplate.Owner,
                Type = baseTemplate.Type,
                Health = baseTemplate.Health,
                X = pos.X,
                Y = pos.Y,
                Facing = baseTemplate.Facing,
                SubPos = baseTemplate.SubPos,
                Status = baseTemplate.Status,
                Tag = baseTemplate.Tag,
                Veterancy = baseTemplate.Veterancy,
                Group = baseTemplate.Group,
                Layer = type == EntityType.Unit ? -1 : 0,
                Reserved1 = type == EntityType.Infantry ? 1 : baseTemplate.Reserved1,
                Reserved2 = baseTemplate.Reserved2
            };
            entities.Add(e);
        }
        return entities;
    }

    /// <summary>生成 [Units] 区块文本</summary>
    public static string WriteUnitsBlock(List<BaseEntity> units, int startIndex)
    {
        var sb = new System.Text.StringBuilder();
        sb.AppendLine("[Units]");
        for (int i = 0; i < units.Count; i++)
        {
            var u = units[i];
            sb.AppendLine($"{startIndex + i}={u.Owner},{u.Type},{u.Health},{u.X},{u.Y}," +
                $"{u.Facing},{u.Status},{u.Tag},{u.Veterancy},{u.Group},0,{u.Layer},1,{u.Reserved2}");
        }
        return sb.ToString();
    }

    /// <summary>生成 [Infantry] 区块文本</summary>
    public static string WriteInfantryBlock(List<BaseEntity> infantry, int startIndex)
    {
        var sb = new System.Text.StringBuilder();
        sb.AppendLine("[Infantry]");
        for (int i = 0; i < infantry.Count; i++)
        {
            var inf = infantry[i];
            sb.AppendLine($"{startIndex + i}={inf.Owner},{inf.Type},{inf.Health},{inf.X},{inf.Y}," +
                $"{inf.SubPos},{inf.Status},{inf.Facing},{inf.Tag},{inf.Veterancy},{inf.Group},0,{inf.Reserved1},{inf.Reserved2}");
        }
        return sb.ToString();
    }
}