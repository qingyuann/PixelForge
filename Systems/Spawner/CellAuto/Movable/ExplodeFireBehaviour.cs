using System.Numerics;
using PixelForge.Tools;
using Silk.NET.Core;

namespace PixelForge.Spawner.CellAuto.Movable;

public class ExplodeFireBehaviour : ICellBehaviour
{
    private static void MoveToTarget(int ci, int cj, int idSource, int idTarget)
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
            CellTools.SetCellColor(idTarget, "fire");
            CellTools.SetCellColor(idSource, "none");
        }
        //if idTarget is something, become fire
        else
        {
            //if the cell is sand, explode
            if (CellAutomationSystem._cellEntities[idTarget].isComponentSand || CellAutomationSystem._cellEntities[idTarget].isComponentStone)
            {
                
                for (var x = -1; x < 2; x++)
                {
                    for (var y = -1; y < 2; y++)
                    {
                        var index = CellTools.ComputeIndex(ci + x, cj + y);
                        if (index != -1)
                        {
                            if (CellAutomationSystem._cellEntities[index].isComponentSand || CellAutomationSystem._cellEntities[idTarget].isComponentStone)
                            {
                                CellAutomationSystem._cellEntities[index].isComponentStone = false;
                                CellAutomationSystem._cellEntities[index].isComponentSand = false;
                                CellTools.SetCellColor(index, "none");
                                //set ExplodeFire to random direction
                                var dir = new Vector2(RandomTool.Range(-5, 5), RandomTool.Range(-5, 5));
                                if (!CellAutomationSystem._cellEntities[index].hasComponentExplodeFire)
                                {
                                    CellAutomationSystem._cellEntities[index].AddComponentExplodeFire(dir, 3);
                                }
                                CellAutomationSystem._cellEntities[index].isComponentCellularAutomation = true;
                                CellAutomationSystem._cellEntities[index].isComponentCellUpdate = true;
                                CellTools.SetCellColor(index, "fire");
                            }
                        }
                    }
                }
            }
            else
            {
                CellAutomationSystem._cellEntities[idSource].RemoveComponentExplodeFire();
                CellTools.SetCellColor(idSource, "none");
                if (!CellAutomationSystem._cellEntities[idSource].hasComponentFire)
                {
                    CellAutomationSystem._cellEntities[idSource].AddComponentFire(20, 8);
                }
                CellTools.SetCellColor(idSource, "fire");
            }
            
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
                    CellTools.SetCellColor(id, "none");
                    CellAutomationSystem._cellEntities[id].AddComponentFire(10, 8);
                    CellTools.SetCellColor(id, "fire");
                    return;
                }
                CellTools.SetCellColor(id, "none");
                return;
            }
        }
        
        //move one step to target direction
        if (idTarget != -1)
        {
            MoveToTarget(i,j, id, idTarget);
        }
        
        
        
    }
}