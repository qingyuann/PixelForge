using PixelForge.Tools;

namespace PixelForge.Spawner.CellAuto.Movable;

public class ExplodeFireBehaviour : ICellBehaviour
{
    private static void MoveToTarget(int idSource, int idTarget)
    {
        //if idTarget is nothing, move to target
        if (!CellAutomationSystem._cellEntities[idTarget].isComponentCellularAutomation && !CellAutomationSystem._cellEntities[idTarget].hasComponentExplodeFire)
        {
            CellAutomationSystem._cellEntities[idTarget].AddComponentExplodeFire(CellAutomationSystem._cellEntities[idSource].componentExplodeFire.Direction, 
                CellAutomationSystem._cellEntities[idSource].componentExplodeFire.TravelTime - 1);
            CellAutomationSystem._cellEntities[idTarget].isComponentCellularAutomation = true;
            CellAutomationSystem._cellEntities[idTarget].isComponentCellUpdate = true;
            CellAutomationSystem._cellEntities[idSource].isComponentCellularAutomation = false;
            CellAutomationSystem._cellEntities[idSource].RemoveComponentExplodeFire();
            CellTools.SetCellColor(idTarget, "bombFire");
            CellTools.SetCellColor(idSource, "none");
        }
        //if idTarget is something, become fire
        else
        {
            CellAutomationSystem._cellEntities[idSource].RemoveComponentExplodeFire();
            CellAutomationSystem._cellEntities[idSource].AddComponentFire(20, 15);
        }
        
        
        
        
        
        //Debug.Log("move down");
        
        
    }
    
    public static void Act(int i, int j)
    {
        var id = CellTools.ComputeIndex(i, j);
        var dir = CellAutomationSystem._cellEntities[CellTools.ComputeIndex(i, j)].componentExplodeFire.Direction;
        var idTarget = CellTools.ComputeIndex(i + (int)dir.X, j + (int)dir.Y);
        if (CellAutomationSystem._cellEntities[id].hasComponentExplodeFire)
        {
            if(CellAutomationSystem._cellEntities[id].componentExplodeFire.TravelTime > 0)
                CellAutomationSystem._cellEntities[id].componentExplodeFire.TravelTime -= 1;
            else
            {
                if (CellAutomationSystem._cellEntities[id].hasComponentExplodeFire)
                {
                    CellAutomationSystem._cellEntities[id].RemoveComponentExplodeFire();
                    CellAutomationSystem._cellEntities[id].AddComponentFire(15, 10);
                    CellTools.SetCellColor(id, "fire");
                    return;
                }
                CellAutomationSystem._cellEntities[id].AddComponentFire(15, 10);
                CellTools.SetCellColor(id, "fire");
                return;
            }
        }
        
        //move one step to target direction
        if (idTarget != -1)
        {
            MoveToTarget(id, idTarget);
        }
        
        
        
    }
}