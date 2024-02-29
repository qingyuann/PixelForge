using PixelForge.Tools;

namespace PixelForge.Spawner.CellAuto.Movable;

public class ExplodeFireBehaviour : ICellBehaviour
{
    private static void MoveToTarget(int idSource, int idTarget)
    {
        CellAutomationSystem._cellEntities[idTarget].isComponentCellularAutomation = true;
        if (!CellAutomationSystem._cellEntities[idTarget].hasComponentExplodeFire)
        {
            CellAutomationSystem._cellEntities[idTarget].AddComponentExplodeFire(CellAutomationSystem._cellEntities[idSource].componentExplodeFire.Direction, 
                CellAutomationSystem._cellEntities[idSource].componentExplodeFire.TravelTime - 1);
        }
        
        CellAutomationSystem._cellEntities[idTarget].isComponentCellUpdate = true;
        
        CellAutomationSystem._cellEntities[idSource].isComponentCellularAutomation = false;
        CellAutomationSystem._cellEntities[idSource].RemoveComponentExplodeFire();
        
        //Debug.Log("move down");
        
        CellTools.SetCellColor(idTarget, "bombFire");
        CellTools.SetCellColor(idSource, "none");
    }
    
    public static void Act(int i, int j)
    {
        var id = CellTools.ComputeIndex(i, j);
        var dir = CellAutomationSystem._cellEntities[CellTools.ComputeIndex(i, j)].componentExplodeFire.Direction;
        
        if (CellAutomationSystem._cellEntities[id].hasComponentExplodeFire)
        {
            if(CellAutomationSystem._cellEntities[id].componentExplodeFire.TravelTime > 0)
                CellAutomationSystem._cellEntities[id].componentExplodeFire.TravelTime -= 1;
            else
            {
                if (CellAutomationSystem._cellEntities[id].hasComponentExplodeFire)
                {
                    CellAutomationSystem._cellEntities[id].RemoveComponentExplodeFire();
                    CellAutomationSystem._cellEntities[id].AddComponentFire(20, 3);
                    CellTools.SetCellColor(id, "fire");
                    return;
                }
            }
        }
        
        //move one step to target direction
        
        var idTarget = CellTools.ComputeIndex(i + (int)dir.X, j + (int)dir.Y);
        var idTop = CellTools.ComputeIndex(i, j - 1);
        var idDown = CellTools.ComputeIndex(i, j + 1);
        var idLeft = CellTools.ComputeIndex(i-1, j);
        var idRight = CellTools.ComputeIndex(i + 1, j);
        var idTopLeft = CellTools.ComputeIndex(i - 1, j - 1);
        var idTopRight = CellTools.ComputeIndex(i + 1, j - 1);
        var idDownLeft = CellTools.ComputeIndex(i - 1, j + 1);
        var idDownRight = CellTools.ComputeIndex(i + 1, j + 1);
        
        
        if (idTarget != -1)
        {
            MoveToTarget(id, idTarget);
        }
        
        
    }
}