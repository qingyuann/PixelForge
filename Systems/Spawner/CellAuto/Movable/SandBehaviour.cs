namespace PixelForge.Spawner.CellAuto.Movable;

public class SandBehaviour : ICellBehaviour
{
    public static void Act(int i, int j)
    {
        var id = CellTools.ComputeIndex(i, j);
        var idDown = CellTools.ComputeIndex(i, j + 1);
        var idDown2 = CellTools.ComputeIndex(i, j + 2);
        var idDown3 = CellTools.ComputeIndex(i, j + 3);
        var idDownLeft = CellTools.ComputeIndex(i - 1, j + 1);
        var idDownRight = CellTools.ComputeIndex(i + 1, j + 1);

        
        if (idDown3 != -1)
        {
            if (!CellAutomationSystem._cellEntities[idDown3].isComponentCellularAutomation)
            {
                MoveToTarget(id, idDown3);
                return;
            }
        }
        
        if (idDown2 != -1)
        {
            if (!CellAutomationSystem._cellEntities[idDown2].isComponentCellularAutomation)
            {
                MoveToTarget(id, idDown2);
                return;
            }
        }

        if (idDown != -1)
        {
            if (CellAutomationSystem._cellEntities[idDown].isComponentWater)
            {
                SwitchSandWater(id, idDown);
                return;
            }
            if (!CellAutomationSystem._cellEntities[idDown].isComponentCellularAutomation)
            {
                MoveToTarget(id, idDown);
                return;
            }
        }
        
        

        if(idDownRight != -1)
        {
            if(!CellAutomationSystem._cellEntities[idDownRight].isComponentCellularAutomation)
            {
                CellAutomationSystem._cellEntities[idDownRight].isComponentCellularAutomation = true;
                CellAutomationSystem._cellEntities[idDownRight].isComponentSand = true;
                CellAutomationSystem._cellEntities[id].isComponentCellularAutomation = false;
                CellAutomationSystem._cellEntities[id].isComponentSand = false;
                //Debug.Log("move down right");
                CellTools.SetCellColor(idDownRight, "sand");
                CellTools.SetCellColor(id, "none");
                
                return;
            }
        }
        
        if (idDownLeft != -1)
        {
            if (!CellAutomationSystem._cellEntities[idDownLeft].isComponentCellularAutomation)
            {
                CellAutomationSystem._cellEntities[idDownLeft].isComponentCellularAutomation = true;
                CellAutomationSystem._cellEntities[idDownLeft].isComponentSand = true;
                CellAutomationSystem._cellEntities[id].isComponentCellularAutomation = false;
                CellAutomationSystem._cellEntities[id].isComponentSand = false;
                //Debug.Log("move down left");
                CellTools.SetCellColor(idDownLeft, "sand");
                CellTools.SetCellColor(id, "none");
                return;
            }
        }
        
        
    }

    private static void MoveToTarget(int idSource, int idTarget)
    {
        CellAutomationSystem._cellEntities[idTarget].isComponentCellularAutomation = true;
        CellAutomationSystem._cellEntities[idTarget].isComponentSand = true;
        CellAutomationSystem._cellEntities[idSource].isComponentCellularAutomation = false;
        CellAutomationSystem._cellEntities[idSource].isComponentSand = false;
        //Debug.Log("move down");
        CellTools.SetCellColor(idTarget, "sand");
        CellTools.SetCellColor(idSource, "none");
    }

    private static void SwitchSandWater(int idSand, int idWater)
    {
        CellAutomationSystem._cellEntities[idWater].isComponentSand = true;
        CellAutomationSystem._cellEntities[idWater].isComponentWater = false;
        
        CellAutomationSystem._cellEntities[idSand].isComponentWater = true;
        CellAutomationSystem._cellEntities[idSand].isComponentSand = false;
        
        CellTools.SetCellColor(idWater, "sand");
        CellTools.SetCellColor(idSand, "water");
    }
}