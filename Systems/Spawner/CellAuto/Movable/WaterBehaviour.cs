namespace PixelForge.Spawner.CellAuto.Movable;

public class WaterBehaviour : ICellBehaviour
{
    private static int _velocity = 2;
    private static string _colorType = "water";

    private static void MoveToTarget(int idSource, int idTarget)
    {
        CellAutomationSystem._cellEntities[idSource].isComponentCellularAutomation = false;
        CellAutomationSystem._cellEntities[idSource].isComponentWater = false;
        
        CellAutomationSystem._cellEntities[idTarget].isComponentCellularAutomation = true;
        CellAutomationSystem._cellEntities[idTarget].isComponentWater = true;

        //Debug.Log("move down");
        CellTools.SetCellColor(idTarget, "water");
        CellTools.SetCellColor(idSource, "none");
    }
    
    public static void Act(int i, int j)
    {
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
            if (!CellAutomationSystem._cellEntities[idDown].isComponentCellularAutomation)
            {
                MoveToTarget(id, idDown);
                return;
            }
        }

        if (idDownLeft != -1)
        {
            if (!CellAutomationSystem._cellEntities[idDownLeft].isComponentCellularAutomation)
            {
                MoveToTarget(id, idDownLeft);
                
                return;

            }
        }

        if(idDownRight != -1)
        {
            if(!CellAutomationSystem._cellEntities[idDownRight].isComponentCellularAutomation)
            {
                MoveToTarget(id, idDownRight);
                
                return;
            }
        }
        
        
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
        
        
    }
}