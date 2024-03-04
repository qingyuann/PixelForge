using PixelForge.Tools;

namespace PixelForge.Spawner.CellAuto;

public class CellTools
{
    private static readonly List<byte> SandColor = new List<byte>{ 194, 178, 128, 255 };
    private static readonly List<byte> WaterColor = new List<byte>{ 135, 206, 250, 255 };
    private static readonly List<byte> AcidColor = new List<byte>{ 76, 187, 23, 255 };
    private static readonly List<byte> LavaColor = new List<byte>{ 255, 69, 0, 255 };
    private static readonly List<byte> OliColor = new List<byte>{ 59, 41, 30, 255 };
    private static readonly List<byte> StoneColor = new List<byte>{ 153, 101, 40, 255 };
    private static readonly List<byte> FireColor = new List<byte>{ 255, 165, 0, 255 };
    private static readonly List<byte> SteamColor = new List<byte> { 255, 220, 220, 150};
    private static readonly List<byte> SmokeColor = new List<byte> { 120, 100, 100, 250};
    private static readonly List<byte> BombFireColor1 = new List<byte> { 255, 215, 0, 255};
    private static readonly List<byte> BombFireColor2 = new List<byte> { 255, 165, 0, 255};
    private static readonly List<byte> BombFireColor3 = new List<byte> { 255, 0, 0, 255};
    private static readonly List<byte> BombFireColor4 = new List<byte> { 139, 0, 0, 255};
    
    
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
            
            return;
        }

        if (colorType == "none")
        {
            var offset = index * 4;
            CellAutomationSystem._cellColors[offset] = 0;
            CellAutomationSystem._cellColors[offset + 1] = 0;
            CellAutomationSystem._cellColors[offset + 2] = 0;
            CellAutomationSystem._cellColors[offset + 3] = 0;
            
            return;
        }
        
        if (colorType == "water")
        {
            var offset = index * 4;
            CellAutomationSystem._cellColors[offset] = WaterColor[0];
            CellAutomationSystem._cellColors[offset + 1] = WaterColor[1];
            CellAutomationSystem._cellColors[offset + 2] = WaterColor[2];
            CellAutomationSystem._cellColors[offset + 3] = WaterColor[3];
            
            return;
        }
        
        if (colorType == "oli")
        {
            var offset = index * 4;
            CellAutomationSystem._cellColors[offset] = OliColor[0];
            CellAutomationSystem._cellColors[offset + 1] = OliColor[1];
            CellAutomationSystem._cellColors[offset + 2] = OliColor[2];
            CellAutomationSystem._cellColors[offset + 3] = OliColor[3];
            
            return;
        }
        
        if (colorType == "lava")
        {
            var offset = index * 4;
            CellAutomationSystem._cellColors[offset] = LavaColor[0];
            CellAutomationSystem._cellColors[offset + 1] = LavaColor[1];
            CellAutomationSystem._cellColors[offset + 2] = LavaColor[2];
            CellAutomationSystem._cellColors[offset + 3] = LavaColor[3];
            
            return;
        }
        
        if (colorType == "acid")
        {
            var offset = index * 4;
            CellAutomationSystem._cellColors[offset] = AcidColor[0];
            CellAutomationSystem._cellColors[offset + 1] = AcidColor[1];
            CellAutomationSystem._cellColors[offset + 2] = AcidColor[2];
            CellAutomationSystem._cellColors[offset + 3] = AcidColor[3];
            
            return;
        }
        
        
        
        if (colorType == "stone")
        {
            var offset = index * 4;
            CellAutomationSystem._cellColors[offset] = StoneColor[0];
            CellAutomationSystem._cellColors[offset + 1] = StoneColor[1];
            CellAutomationSystem._cellColors[offset + 2] = StoneColor[2];
            CellAutomationSystem._cellColors[offset + 3] = StoneColor[3];
        }
        
        if (colorType == "fire")
        {
            var offset = index * 4;
            CellAutomationSystem._cellColors[offset] = FireColor[0];
            CellAutomationSystem._cellColors[offset + 1] = FireColor[1];
            CellAutomationSystem._cellColors[offset + 2] = FireColor[2];
            CellAutomationSystem._cellColors[offset + 3] = FireColor[3];
        }

        if (colorType == "steam")
        {
            var offset = index * 4;
            CellAutomationSystem._cellColors[offset] = SteamColor[0];
            CellAutomationSystem._cellColors[offset + 1] = SteamColor[1];
            CellAutomationSystem._cellColors[offset + 2] = SteamColor[2];
            CellAutomationSystem._cellColors[offset + 3] = SteamColor[3];
        }
        
        if (colorType == "smoke")
        {
            var offset = index * 4;
            CellAutomationSystem._cellColors[offset] = SmokeColor[0];
            CellAutomationSystem._cellColors[offset + 1] = SmokeColor[1];
            CellAutomationSystem._cellColors[offset + 2] = SmokeColor[2];
            CellAutomationSystem._cellColors[offset + 3] = SmokeColor[3];
        }
        
        if (colorType == "bombFire")
        {
            var ran = RandomTool.Range(0,4);
            var offset = index * 4;
            switch (ran)
            {
                case 0:
                    CellAutomationSystem._cellColors[offset] = BombFireColor1[0];
                    CellAutomationSystem._cellColors[offset + 1] = BombFireColor1[1];
                    CellAutomationSystem._cellColors[offset + 2] = BombFireColor1[2];
                    CellAutomationSystem._cellColors[offset + 3] = BombFireColor1[3];
                    break;
                case 1:
                    CellAutomationSystem._cellColors[offset] = BombFireColor2[0];
                    CellAutomationSystem._cellColors[offset + 1] = BombFireColor2[1];
                    CellAutomationSystem._cellColors[offset + 2] = BombFireColor2[2];
                    CellAutomationSystem._cellColors[offset + 3] = BombFireColor2[3];
                    break;
                case 2:
                    CellAutomationSystem._cellColors[offset] = BombFireColor3[0];
                    CellAutomationSystem._cellColors[offset + 1] = BombFireColor3[1];
                    CellAutomationSystem._cellColors[offset + 2] = BombFireColor3[2];
                    CellAutomationSystem._cellColors[offset + 3] = BombFireColor3[3];
                    break;
                case 3:
                    CellAutomationSystem._cellColors[offset] = BombFireColor4[0];
                    CellAutomationSystem._cellColors[offset + 1] = BombFireColor4[1];
                    CellAutomationSystem._cellColors[offset + 2] = BombFireColor4[2];
                    CellAutomationSystem._cellColors[offset + 3] = BombFireColor4[3];
                    break;
            }

            
            
            
        }
        
    }
    
    
}