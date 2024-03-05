using System.Net;
using PixelForge.Tools;

namespace PixelForge.Spawner.CellAuto.Movable;



public class FireBehaviour : ICellBehaviour
{
    private static void FireSpread(int idSource, int idTarget)
    {
        var spt = CellAutomationSystem._cellEntities[idSource].componentFire.SpreadTime - 1;
        if (CellAutomationSystem._cellEntities[idSource].hasComponentFire) { CellAutomationSystem._cellEntities[idSource].RemoveComponentFire(); }
        CellAutomationSystem._cellEntities[idSource].isComponentSmoke = true;
        
        if (CellAutomationSystem._cellEntities[idTarget].hasComponentLiquid) { CellAutomationSystem._cellEntities[idTarget].RemoveComponentLiquid(); }
        CellAutomationSystem._cellEntities[idTarget].isComponentSand = false;
        CellAutomationSystem._cellEntities[idTarget].isComponentStone = false;
        if (!CellAutomationSystem._cellEntities[idTarget].hasComponentFire) { CellAutomationSystem._cellEntities[idTarget].AddComponentFire(50, spt); }
        
        CellAutomationSystem._cellEntities[idTarget].isComponentCellUpdate = true;
        CellTools.SetCellColor(idTarget, "none");
        CellTools.SetCellColor(idTarget, "fire", spt, 50);
        CellTools.SetCellColor(idSource, "none");
        CellTools.SetCellColor(idSource, "smoke");
    }
    
    private static void MoveToTarget(int idSource, int idTarget)
    {
        var spt = CellAutomationSystem._cellEntities[idSource].componentFire.SpreadTime;
        CellAutomationSystem._cellEntities[idTarget].isComponentCellularAutomation = true;
        if (!CellAutomationSystem._cellEntities[idTarget].hasComponentFire) {CellAutomationSystem._cellEntities[idTarget].AddComponentFire(50, spt);}
        
        CellAutomationSystem._cellEntities[idSource].isComponentCellularAutomation = false;
        if (CellAutomationSystem._cellEntities[idSource].hasComponentFire){CellAutomationSystem._cellEntities[idSource].RemoveComponentFire();}
        CellAutomationSystem._cellEntities[idTarget].isComponentCellUpdate = true;
        //Debug.Log("move down");
        CellTools.SetCellColor(idTarget, "fire", spt, 50);
        CellTools.SetCellColor(idSource, "none");
    }

    private static void FireWithWater(int idSource, int idTarget)
    {
        // become water steam
        CellAutomationSystem._cellEntities[idSource].isComponentCellularAutomation = false;
        if (CellAutomationSystem._cellEntities[idSource].hasComponentFire)
        {
            CellAutomationSystem._cellEntities[idSource].RemoveComponentFire();
        }

        if (CellAutomationSystem._cellEntities[idTarget].hasComponentLiquid)
        {
            CellAutomationSystem._cellEntities[idTarget].RemoveComponentLiquid();
        }
        
        CellAutomationSystem._cellEntities[idTarget].isComponentSteam = true;
        
        CellTools.SetCellColor(idTarget, "none");
        CellTools.SetCellColor(idTarget, "steam");
        CellTools.SetCellColor(idSource, "none");
    }

    private static void FireWithLiquid(int idSource, int idTarget)
    {
        
        switch (CellAutomationSystem._cellEntities[idTarget].componentLiquid.Flammability)
        {
            case 0:
                // not flammable
                FireWithWater(idSource, idTarget);
                if (CellAutomationSystem._cellEntities[idTarget].hasComponentLiquid) {CellAutomationSystem._cellEntities[idTarget].RemoveComponentLiquid();}
                break;
            case 1:
                // flammable
                FireSpread(idSource, idTarget);
                if (CellAutomationSystem._cellEntities[idTarget].hasComponentLiquid) {CellAutomationSystem._cellEntities[idTarget].RemoveComponentLiquid();}
                break;
           
        }
    }
    
    
    public static void Act(int i, int j)
    {
        
        var id = CellTools.ComputeIndex(i, j);
        var idTop = CellTools.ComputeIndex(i, j - 1);
        var idDown = CellTools.ComputeIndex(i, j + 1);
        var idLeft = CellTools.ComputeIndex(i-1, j);
        var idRight = CellTools.ComputeIndex(i + 1, j);
        var idTopLeft = CellTools.ComputeIndex(i - 1, j - 1);
        var idTopRight = CellTools.ComputeIndex(i + 1, j - 1);
        var idDownLeft = CellTools.ComputeIndex(i - 1, j + 1);
        var idDownRight = CellTools.ComputeIndex(i + 1, j + 1);
        
        // randomly spread to test surroundings
        var ran = RandomTool.Range(0, 8);
        
        

        if (CellAutomationSystem._cellEntities[id].hasComponentFire)
        {
            if (CellAutomationSystem._cellEntities[id].componentFire.LifeTime > 0)
            {
                CellAutomationSystem._cellEntities[id].componentFire.LifeTime -= 1;
                CellTools.SetCellColor(id, "fire", CellAutomationSystem._cellEntities[id].componentFire.SpreadTime, CellAutomationSystem._cellEntities[id].componentFire.LifeTime);
            }
            else
            {
                if (CellAutomationSystem._cellEntities[id].hasComponentFire)
                {
                    CellAutomationSystem._cellEntities[id].RemoveComponentFire();
                    CellAutomationSystem._cellEntities[id].isComponentCellularAutomation = false;
                    CellTools.SetCellColor(id, "none");
                    return;
                }
            }
            
            if (CellAutomationSystem._cellEntities[id].componentFire.SpreadTime <= 0)
            {
                CellAutomationSystem._cellEntities[id].RemoveComponentFire();
                CellAutomationSystem._cellEntities[id].isComponentCellularAutomation = false;
                CellTools.SetCellColor(id, "none");
                return;
            }
        }
        
        
        var flag = false;
        switch (ran)
        {
            case 0:
                if (idTop != -1)
                {
                    //if (CellAutomationSystem._cellEntities[idTop].isComponentWater) { FireWithWater(id, idTop); return; }
                    if (CellAutomationSystem._cellEntities[idTop].hasComponentLiquid) { 
                        FireWithLiquid(id, idTop); 
                        flag = true;
                        
                    }
                    
                    //has something to burn, spread
                    else if (CellAutomationSystem._cellEntities[idTop].isComponentCellularAutomation &&
                        !CellAutomationSystem._cellEntities[idTop].hasComponentFire)
                    {
                        FireSpread(id, idTop); 
                        flag = true;
                        
                    }
                }
                break;
            
            case 1:
                if (idDown != -1)
                {
                    if (CellAutomationSystem._cellEntities[idDown].hasComponentLiquid)
                    {
                        FireWithLiquid(id, idDown); 
                        flag = true;
                    }

                    else if (CellAutomationSystem._cellEntities[idDown].isComponentCellularAutomation &&
                        !CellAutomationSystem._cellEntities[idDown].hasComponentFire)
                    {
                        FireSpread(id, idDown); 
                        flag = true;
                    }
                }
                break;
            
            case 2:
                if (idLeft != -1)
                {
                    if (CellAutomationSystem._cellEntities[idLeft].hasComponentLiquid)
                    {
                        FireWithLiquid(id, idLeft);
                        flag = true;
                    }

                    else if (CellAutomationSystem._cellEntities[idLeft].isComponentCellularAutomation &&
                        !CellAutomationSystem._cellEntities[idLeft].hasComponentFire)
                    {
                        FireSpread(id, idLeft); 
                        flag = true;
                    }
                }
                break;
            case 3:
                if (idRight != -1)
                {
                    if (CellAutomationSystem._cellEntities[idRight].hasComponentLiquid)
                    {
                        FireWithLiquid(id, idRight); 
                        flag = true;
                    }

                    else if (CellAutomationSystem._cellEntities[idRight].isComponentCellularAutomation &&
                        !CellAutomationSystem._cellEntities[idRight].hasComponentFire)
                    {
                        FireSpread(id, idRight); 
                        flag = true;
                    }
                }
                break;
            case 4:
                if (idDownLeft != -1)
                {
                    if (CellAutomationSystem._cellEntities[idDownLeft].hasComponentLiquid)
                    {
                        FireWithLiquid(id, idDownLeft);
                        flag = true;
                    }

                    else if (CellAutomationSystem._cellEntities[idDownLeft].isComponentCellularAutomation &&
                        !CellAutomationSystem._cellEntities[idDownLeft].hasComponentFire)
                    {
                        FireSpread(id, idDownLeft); 
                        flag = true;
                    }
                }
                break;
            case 5:
                if (idDownRight != -1)
                {
                    if (CellAutomationSystem._cellEntities[idDownRight].hasComponentLiquid)
                    {
                        FireWithLiquid(id, idDownRight);
                        flag = true;
                    }

                    else if (CellAutomationSystem._cellEntities[idDownRight].isComponentCellularAutomation &&
                        !CellAutomationSystem._cellEntities[idDownRight].hasComponentFire)
                    {
                        FireSpread(id, idDownRight); 
                        flag = true;
                    }
                }
                break;
            case 6:
                if (idTopLeft != -1)
                {
                    if (CellAutomationSystem._cellEntities[idTopLeft].hasComponentLiquid)
                    {
                        FireWithLiquid(id, idTopLeft); 
                        flag = true;
                    }

                    else if (CellAutomationSystem._cellEntities[idTopLeft].isComponentCellularAutomation &&
                        !CellAutomationSystem._cellEntities[idTopLeft].hasComponentFire)
                    {
                        FireSpread(id, idTopLeft); 
                        flag = true;
                    }
                }
                break;
            case 7:
                if (idTopRight != -1)
                {
                    if (CellAutomationSystem._cellEntities[idTopRight].hasComponentLiquid)
                    {
                        FireWithLiquid(id, idTopRight); 
                        flag = true;
                    }

                    else if (CellAutomationSystem._cellEntities[idTopRight].isComponentCellularAutomation &&
                        !CellAutomationSystem._cellEntities[idTopRight].hasComponentFire)
                    {
                        FireSpread(id, idTopRight);
                        flag = true;
                    }
                }
                break;
            
        }

        if (!flag)
        {
            if(idDown != -1)
                if (!CellAutomationSystem._cellEntities[idDown].isComponentCellularAutomation)
                    MoveToTarget(id, idDown);
            return;
        }
        
        
        

    }
}