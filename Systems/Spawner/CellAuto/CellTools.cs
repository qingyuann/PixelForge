namespace PixelForge.Spawner.CellAuto;

public class CellTools
{
    private static readonly List<byte> SandColor = new List<byte>{ 194, 178, 128, 255 };
    private static readonly List<byte> WaterColor = new List<byte>{ 135, 206, 250, 255 };
    private static readonly List<byte> StoneColor = new List<byte>{ 153, 101, 40, 255 };
    
    
    public static int ComputeIndex(int i, int j)
    {
        if(i < 0 || i >= CellAutomationSystem._width || j < 0 || j >= CellAutomationSystem._height)
        {
            return -1;
        }
        var index = j * CellAutomationSystem._width + i;
        return index;
    }
    
    public static void SetCellColor(int index, string colorType)
    {
       
        if (colorType == "sand")
        {
            var offset = index * 4;
            //Debug.Log("set sand color");
            CellAutomationSystem._cellColors[offset] = SandColor[0];
            CellAutomationSystem._cellColors[offset + 1] = SandColor[1];
            CellAutomationSystem._cellColors[offset + 2] = SandColor[2];
            CellAutomationSystem._cellColors[offset + 3] = SandColor[3];
        }

        if (colorType == "none")
        {
            var offset = index * 4;
            CellAutomationSystem._cellColors[offset] = 0;
            CellAutomationSystem._cellColors[offset + 1] = 0;
            CellAutomationSystem._cellColors[offset + 2] = 0;
            CellAutomationSystem._cellColors[offset + 3] = 0;
        }
        
        if (colorType == "water")
        {
            var offset = index * 4;
            CellAutomationSystem._cellColors[offset] = WaterColor[0];
            CellAutomationSystem._cellColors[offset + 1] = WaterColor[1];
            CellAutomationSystem._cellColors[offset + 2] = WaterColor[2];
            CellAutomationSystem._cellColors[offset + 3] = WaterColor[3];
        }
        
        if (colorType == "stone")
        {
            var offset = index * 4;
            CellAutomationSystem._cellColors[offset] = StoneColor[0];
            CellAutomationSystem._cellColors[offset + 1] = StoneColor[1];
            CellAutomationSystem._cellColors[offset + 2] = StoneColor[2];
            CellAutomationSystem._cellColors[offset + 3] = StoneColor[3];
        }
    }
    
    
}