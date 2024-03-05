using PixelForge.Tools;

namespace PixelForge.Spawner.CellAuto.Movable;

public class SmokeBehaviour : ICellBehaviour
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
        
        var ran = RandomTool.Range(0, 3);
        var flag = 0;
        switch (ran)
        {
            case 0:
                if (idTopLeft != -1)
                {
                    if (!CellAutomationSystem._cellEntities[idTopLeft].isComponentCellularAutomation)
                    {
                        MoveToTarget(id, idTopLeft);
                        return;
                    }
                    else
                    {
                        flag = 1;
                    }
                }
                break;
            case 1:
                if (idTop != -1)
                {
                    if (!CellAutomationSystem._cellEntities[idTop].isComponentCellularAutomation)
                    {
                        //Debug.Log("steam move up");
                        MoveToTarget(id, idTop);
                        return;
                    }
                    else
                    {
                        flag = 1;
                    }
                }
                break;
            case 2:
                if (idTopRight != -1)
                {
                    if (!CellAutomationSystem._cellEntities[idTopRight].isComponentCellularAutomation)
                    {
                        MoveToTarget(id, idTopRight);
                    }
                    else
                    {
                        flag = 1;
                    }
                }
                break;
        }
        
        if(flag == 1)
        {
            CellAutomationSystem._cellEntities[id].isComponentSteam = false;
            CellAutomationSystem._cellEntities[id].isComponentCellularAutomation = false;
            CellTools.SetCellColor(id, "none");
        }
        
    }
    
    private static void MoveToTarget(int idSource, int idTarget)
    {
        CellAutomationSystem._cellEntities[idTarget].isComponentCellularAutomation = true;
        CellAutomationSystem._cellEntities[idTarget].isComponentSmoke = true;
        
        CellAutomationSystem._cellEntities[idSource].isComponentCellularAutomation = false;
        CellAutomationSystem._cellEntities[idSource].isComponentSmoke = false;
        
        //Debug.Log("move down");
        
        CellTools.SetCellColor(idTarget, "smoke");
        CellTools.SetCellColor(idSource, "none");
    }
    
}