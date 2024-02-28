using PixelForge.Tools;

namespace PixelForge.Spawner.CellAuto.Movable;

public class LiquidBehaviour : ICellBehaviour
{
    private static int moveFlag = 0;
    private static void MoveToTarget(int idSource, int idTarget)
    {
        //if idTarget is nothing, move to target
        if (!CellAutomationSystem._cellEntities[idTarget].isComponentCellularAutomation && 
            CellAutomationSystem._cellEntities[idSource].hasComponentLiquid && 
            !CellAutomationSystem._cellEntities[idTarget].hasComponentLiquid)
        {
            var coty = CellAutomationSystem._cellEntities[idSource].componentLiquid.ColorType;
            var den = CellAutomationSystem._cellEntities[idSource].componentLiquid.Density;
            var flam = CellAutomationSystem._cellEntities[idSource].componentLiquid.Flammability;
            //Debug.Log("move to target");
            CellAutomationSystem._cellEntities[idSource].isComponentCellularAutomation = false;
            CellAutomationSystem._cellEntities[idSource].RemoveComponentLiquid();
        
            CellAutomationSystem._cellEntities[idTarget].isComponentCellularAutomation = true;
            CellAutomationSystem._cellEntities[idTarget].AddComponentLiquid(coty, den, flam);
            CellAutomationSystem._cellEntities[idTarget].isComponentCellUpdate = true;

            //Debug.Log("move down");
            CellTools.SetCellColor(idTarget, coty);
            CellTools.SetCellColor(idSource, "none");
            
            moveFlag = 1;
            return;
        }
        
        //if idTarget is another liquid, compare density
        if (CellAutomationSystem._cellEntities[idTarget].hasComponentLiquid && 
            CellAutomationSystem._cellEntities[idSource].hasComponentLiquid)
        {
            var coty = CellAutomationSystem._cellEntities[idSource].componentLiquid.ColorType;
            var den = CellAutomationSystem._cellEntities[idSource].componentLiquid.Density;
            var flam = CellAutomationSystem._cellEntities[idSource].componentLiquid.Flammability;
            //if source is heavier than target, move to target
            if (CellAutomationSystem._cellEntities[idSource].componentLiquid.Density > CellAutomationSystem._cellEntities[idTarget].componentLiquid.Density)
            {
               CellAutomationSystem._cellEntities[idSource].componentLiquid.ColorType = CellAutomationSystem._cellEntities[idTarget].componentLiquid.ColorType;
               CellAutomationSystem._cellEntities[idSource].componentLiquid.Density = CellAutomationSystem._cellEntities[idTarget].componentLiquid.Density;
               CellAutomationSystem._cellEntities[idSource].componentLiquid.Flammability = CellAutomationSystem._cellEntities[idTarget].componentLiquid.Flammability;
               
               CellAutomationSystem._cellEntities[idTarget].componentLiquid.ColorType = coty;
               CellAutomationSystem._cellEntities[idTarget].componentLiquid.Density = den;
               CellAutomationSystem._cellEntities[idTarget].componentLiquid.Flammability = flam;
               
               CellAutomationSystem._cellEntities[idTarget].isComponentCellUpdate = true;
               
               CellTools.SetCellColor(idTarget, CellAutomationSystem._cellEntities[idTarget].componentLiquid.ColorType);
               CellTools.SetCellColor(idSource, CellAutomationSystem._cellEntities[idSource].componentLiquid.ColorType);
               
               moveFlag = 1;
               return;
            }
        }
        
    }
    
    public static void Act(int i, int j)
    {
        moveFlag = 0;
        var id = CellTools.ComputeIndex(i, j);
        var idDown = CellTools.ComputeIndex(i, j + 1);
        var idDownLeft = CellTools.ComputeIndex(i - 1, j + 1);
        var idDownRight = CellTools.ComputeIndex(i + 1, j + 1);
        
        var idLeft = CellTools.ComputeIndex(i - 1, j);
        var idLeft2 = CellTools.ComputeIndex(i - 2, j);
        var idLeft3 = CellTools.ComputeIndex(i - 3, j);
        var idLeft4 = CellTools.ComputeIndex(i - 4, j);
        
        var idRight = CellTools.ComputeIndex(i + 1, j);
        var idRight2 = CellTools.ComputeIndex(i + 2, j);
        var idRight3 = CellTools.ComputeIndex(i + 3, j);


        if (idDown != -1)
        {
            MoveToTarget(id, idDown);
        }

        if (idDownLeft != -1 && moveFlag == 0)
        {
            MoveToTarget(id, idDownLeft);
        }

        if(idDownRight != -1 && moveFlag == 0)
        {
            
            MoveToTarget(id, idDownRight);
        }
        
        
        if(idLeft != -1 && moveFlag == 0)
        {
            MoveToTarget(id, idLeft);
        }
        
        /*
        if(idRight != -1 && moveFlag == 0)
        {
            MoveToTarget(id, idRight);
        }

        if (moveFlag == 1)
        {
            return;
        }
        */
       
        
        
        var ran = RandomTool.Range(0, 2);
        
        
        switch (ran)
        {
            case 0:
                
                if (idLeft3 != -1)
                {
                    if (!CellAutomationSystem._cellEntities[idLeft3].isComponentCellularAutomation)
                    {
                        MoveToTarget(id, idLeft3);
                        return;
                    }
                }
                
                if (idLeft2 != -1)
                {
                    if (!CellAutomationSystem._cellEntities[idLeft2].isComponentCellularAutomation)
                    {
                        MoveToTarget(id, idLeft2);
                        return;
                    }
                }
                    
                if (idLeft != -1)
                {
                    if (!CellAutomationSystem._cellEntities[idLeft].isComponentCellularAutomation)
                    {
                        MoveToTarget(id, idLeft);
                        return;
                    }
                }
                break;
            case 1:
                
                if (idRight3 != -1)
                {
                    if (!CellAutomationSystem._cellEntities[idRight3].isComponentCellularAutomation)
                    {
                        MoveToTarget(id, idRight3);
                        return;
                    }
                }
                
                if (idRight2 != -1)
                {
                    if (!CellAutomationSystem._cellEntities[idRight2].isComponentCellularAutomation)
                    {
                        MoveToTarget(id, idRight2);
                        return;
                    }
                }
                
                
                if (idRight != -1)
                {
                    if(!CellAutomationSystem._cellEntities[idRight].isComponentCellularAutomation)
                    {
                        MoveToTarget(id, idRight);
                        return;
                    }
                }
                break;
        }
        
        
    }
}