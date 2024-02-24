using PixelForge.Tools;

namespace PixelForge.Spawner.CellAuto.Movable;

public class FireBehaviour : ICellBehaviour
{
    private static void FireSpread(int idSource, int idTarget)
    {
        CellAutomationSystem._cellEntities[idSource].isComponentCellularAutomation = false;
        CellAutomationSystem._cellEntities[idSource].RemoveComponentFire();
        
        CellAutomationSystem._cellEntities[idTarget].AddComponentFire(10);
        
        CellTools.SetCellColor(idTarget, "fire");
        CellTools.SetCellColor(idSource, "none");
    }
    
    private static void MoveToTarget(int idSource, int idTarget)
    {
        CellAutomationSystem._cellEntities[idTarget].isComponentCellularAutomation = true;
        CellAutomationSystem._cellEntities[idTarget].isComponentFire = true;
        CellAutomationSystem._cellEntities[idSource].isComponentCellularAutomation = false;
        CellAutomationSystem._cellEntities[idSource].isComponentFire = false;
        //Debug.Log("move down");
        CellTools.SetCellColor(idTarget, "fire");
        CellTools.SetCellColor(idSource, "none");
    }

    public static void FireWithWater(int idSource, int idTarget)
    {
        // become water steam
        CellAutomationSystem._cellEntities[idSource].isComponentCellularAutomation = false;
        CellAutomationSystem._cellEntities[idSource].isComponentFire = false;
        
        CellAutomationSystem._cellEntities[idTarget].isComponentSteam = true;
        
        CellTools.SetCellColor(idTarget, "steam");
        CellTools.SetCellColor(idSource, "none");
    }
    
    public static void Act(int i, int j)
    {
        var id = CellTools.ComputeIndex(i, j);
        var idTop = CellTools.ComputeIndex(i - 1, j);
        var idDown = CellTools.ComputeIndex(i, j + 1);
        var idLeft = CellTools.ComputeIndex(i, j - 1);
        var idRight = CellTools.ComputeIndex(i + 1, j);
        var idTopLeft = CellTools.ComputeIndex(i - 1, j - 1);
        var idTopRight = CellTools.ComputeIndex(i + 1, j - 1);
        var idDownLeft = CellTools.ComputeIndex(i - 1, j + 1);
        var idDownRight = CellTools.ComputeIndex(i + 1, j + 1);
        
        // randomly spread to test surroundings
        var ran = RandomTool.Range(0, 8);
        
        if (idDown != -1)
        {
            if (!CellAutomationSystem._cellEntities[idDown].isComponentCellularAutomation)
            {
                MoveToTarget(id, idDown);
                return;
            }
        }
        
        switch (ran)
        {
            case 0:
                if (idTop != -1)
                {
                    if (CellAutomationSystem._cellEntities[idTop].isComponentWater) { FireWithWater(id, idTop); return; }
                    if (CellAutomationSystem._cellEntities[idTop].isComponentCellularAutomation) { FireSpread(id, idTop); }
                }
                break;
            case 1:
                if (idDown != -1)
                {
                    if (CellAutomationSystem._cellEntities[idDown].isComponentWater) { FireWithWater(id, idDown); return; }
                    if (CellAutomationSystem._cellEntities[idDown].isComponentCellularAutomation) { FireSpread(id, idDown); }
                }
                break;
            case 2:
                if (idLeft != -1)
                {
                    if (CellAutomationSystem._cellEntities[idLeft].isComponentWater) { FireWithWater(id, idLeft); return; }
                    if (CellAutomationSystem._cellEntities[idLeft].isComponentCellularAutomation) { FireSpread(id, idLeft); }
                }
                break;
            case 3:
                if (idRight != -1)
                {
                    if (CellAutomationSystem._cellEntities[idRight].isComponentWater) { FireWithWater(id, idRight); return; }
                    if (CellAutomationSystem._cellEntities[idRight].isComponentCellularAutomation) { FireSpread(id, idRight); }
                }
                break;
            case 4:
                if (idDownLeft != -1)
                {
                    if (CellAutomationSystem._cellEntities[idDownLeft].isComponentWater) { FireWithWater(id, idDownLeft); return; }
                    if (CellAutomationSystem._cellEntities[idDownLeft].isComponentCellularAutomation) { FireSpread(id, idDownLeft); }
                }
                break;
            case 5:
                if (idDownRight != -1)
                {
                    if (CellAutomationSystem._cellEntities[idDownRight].isComponentWater) { FireWithWater(id, idDownRight); return; }
                    if (CellAutomationSystem._cellEntities[idDownRight].isComponentCellularAutomation) { FireSpread(id, idDownRight); }
                }
                break;
            case 6:
                if (idTopLeft != -1)
                {
                    if (CellAutomationSystem._cellEntities[idTopLeft].isComponentWater) { FireWithWater(id, idTopLeft); return; }
                    if (CellAutomationSystem._cellEntities[idTopLeft].isComponentCellularAutomation) { FireSpread(id, idTopLeft); }
                }
                break;
            case 7:
                if (idTopRight != -1)
                {
                    if (CellAutomationSystem._cellEntities[idTopRight].isComponentWater) { FireWithWater(id, idTopRight); return; }
                    if (CellAutomationSystem._cellEntities[idTopRight].isComponentCellularAutomation) { FireSpread(id, idTopRight); }
                }
                break;
            
        }

        
        
        

    }
}