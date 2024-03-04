using PixelForge.Tools;

namespace PixelForge.Spawner.CellAuto.Movable;

public class SteamBehaviour : ICellBehaviour
{
    public static void Act(int i, int j)
    {
        var id = CellTools.ComputeIndex(i, j);
        var idTop = CellTools.ComputeIndex(i, j-1);
        var idTop2 = CellTools.ComputeIndex(i, j - 2);
        var idTop3 = CellTools.ComputeIndex(i, j - 3);
        var idTopLeft = CellTools.ComputeIndex(i-1, j-1);
        var idTopRight = CellTools.ComputeIndex(i+1, j-1);
        
        if (j < 3)
        {
            CellAutomationSystem._cellEntities[id].isComponentSteam = false;
            CellAutomationSystem._cellEntities[id].isComponentCellularAutomation = false;
            CellTools.SetCellColor(id, "none");
            return;
        }
        
        
        if (idTop2 != -1)
        {
            if (!CellAutomationSystem._cellEntities[idTop2].isComponentCellularAutomation)
            {
                MoveToTarget(id, idTop2);
                return;
            }
        }
        
        
        if (idTop != -1)
        {
            if (!CellAutomationSystem._cellEntities[idTop].isComponentCellularAutomation)
            {
                //Debug.Log("steam move up");
                MoveToTarget(id, idTop);
                return;
            }
        }
        
        if (idTopLeft != -1)
        {
            if (!CellAutomationSystem._cellEntities[idTopLeft].isComponentCellularAutomation)
            {
                MoveToTarget(id, idTopLeft);
                return;
            }
        }
        
        
        if (idTopRight != -1)
        {
            if (!CellAutomationSystem._cellEntities[idTopRight].isComponentCellularAutomation)
            {
                MoveToTarget(id, idTopRight);
                return;
            }
        }
        
        if(idTop != -1 || idTop2 != -1 || idTop3 != -1)
        {
            CellAutomationSystem._cellEntities[id].isComponentSteam = false;
            CellAutomationSystem._cellEntities[id].isComponentCellularAutomation = false;
            CellTools.SetCellColor(id, "none");
        }
        
    }
    
    private static void MoveToTarget(int idSource, int idTarget)
    {
        CellAutomationSystem._cellEntities[idTarget].isComponentCellularAutomation = true;
        CellAutomationSystem._cellEntities[idTarget].isComponentSteam = true;
        
        CellAutomationSystem._cellEntities[idSource].isComponentCellularAutomation = false;
        CellAutomationSystem._cellEntities[idSource].isComponentSteam = false;
        
        //Debug.Log("move down");
        
        CellTools.SetCellColor(idTarget, "steam");
        CellTools.SetCellColor(idSource, "none");
    }

}