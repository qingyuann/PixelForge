using System.Numerics;
using Box2DX.Dynamics;
using Entitas;
using PixelForge.Spawner.CellAuto;
using Render;
using Silk.NET.Input;

namespace PixelForge.Physics;


public class BoxCollisionSystem : IInitializeSystem, IExecuteSystem
{
    private	readonly Contexts _contexts;
    public static Vector2 Position;
    private int _w;
    private int _h;

    private static Byte[] boxData;
    IGroup<GameEntity> _cellRenderGroup;

    public BoxCollisionSystem(Contexts contexts)
    {
        _contexts = contexts;
        _w = 10;
        _h = 20;
        boxData = new Byte[_w * _h * 4];
        Position = new Vector2(0, 0);
        _cellRenderGroup = _contexts.game.GetGroup( GameMatcher.AllOf( GameMatcher.ComponentCellAutoTexture, GameMatcher.MatRenderSingle ) );
    }
    
    
    public void Initialize()
    {
        Position = new Vector2(125, 250);
        DrawBox();
        //shot explosive fire to mouse direction from box position
        InputSystem.AddMouseDownEvent( OnMouseDown );
    }

    public void Execute()
    {
        
        var cellJ = (int)Position.Y;
        var cellI = (int)Position.X;

        if ( cellJ-_h/2 > 0 && !IsStoneThere( GetCellIndex(cellI, cellJ - _h/2 ) ))
        {
            Position.Y -= 1;
            MoveBoxCell(cellI, cellJ, cellI, (int)Position.Y);
        }
       
        
        //move left
        if (InputSystem.GetKey(Key.A))
        {
            cellJ = (int)Position.Y;
            cellI = (int)Position.X;
            if (cellI > 0 && 
                !IsStoneThere( GetCellIndex(cellI - _w/2, cellJ) ) )
            {
                Position.X -= 1;
                MoveBoxCell(cellI, cellJ, (int)Position.X, cellJ);
                
            }
        }
        
        //move right
        if (InputSystem.GetKey(Key.D))
        {
            cellJ = (int)Position.Y;
            cellI = (int)Position.X;
            if (cellI <= CellAutomationSystem._width && 
                !IsStoneThere( GetCellIndex(cellI + _w/2, cellJ) ))
            {
                Position.X += 1;
                MoveBoxCell(cellI, cellJ, (int)Position.X, cellJ);
            }
        }
        
        //jump
        if (InputSystem.GetKeyDown(Key.W))
        {
            cellJ = (int)Position.Y;
            cellI = (int)Position.X;
            if (cellJ <= CellAutomationSystem._height && 
                !IsStoneThere( GetCellIndex(cellI, cellJ + _h/2 + 20) ) )
            {
                Position.Y += 20;
                MoveBoxCell(cellI, cellJ, cellI, (int)Position.Y);
            }
        }
        
        SetBoxPosition();
        
        
    }

    private void OnMouseDown(IMouse mouse, MouseButton button)
    {
        if (button == MouseButton.Left)
        {
            var x = (int)(mouse.Position.X / GameSetting.WindowWidth * CellAutomationSystem._width);
            var y = (int)(mouse.Position.Y / GameSetting.WindowHeight * CellAutomationSystem._height);
            
            var cx = (int)Position.X;
            var cy = (int)Position.Y;
            
            
            //Generate a box of fire bomb in current position
            //The box is 5x5
            
            //if the mouse position is on the right side of the box
            if (x > cx+_w/2)
            {
                for(var i = _w/2; i < _w/2+5; i++)
                {
                    for(var j = 0; j < 5; j++)
                    {
                        var index = GetCellIndex(cx + i, cy + j);
                        if (index != -1)
                        { 
                            var dir = new Vector2((x - cx)/10.0f, (y - 250 + cy)/10.0f);
                            if (!CellAutomationSystem._cellEntities[index].hasComponentExplodeFire)
                            {
                                CellAutomationSystem._cellEntities[index].AddComponentExplodeFire(dir, 20);
                            }
                            CellAutomationSystem._cellEntities[index].isComponentCellUpdate = true;
                            CellTools.SetCellColor(index, "bombFire");
                        }
                    }
                }
            }
            else if (x < cx-_w/2)
            {
                for(var i = _w/2; i < _w/2+5; i++)
                {
                    for(var j = 0; j < 5; j++)
                    {
                        var index = GetCellIndex(cx - i, cy + j);
                        if (index != -1)
                        { 
                            var dir = new Vector2((x - cx)/10.0f, (y - 250 + cy)/10.0f);
                            if (!CellAutomationSystem._cellEntities[index].hasComponentExplodeFire)
                            {
                                CellAutomationSystem._cellEntities[index].AddComponentExplodeFire(dir, 20);
                            }
                            CellAutomationSystem._cellEntities[index].isComponentCellUpdate = true;
                            CellTools.SetCellColor(index, "bombFire");
                        }
                    }
                }
            }
            
        }
    }
    
    
    
    private int GetCellIndex(int x, int y)
    {
        return CellTools.ComputeIndex(x, 250-y);
    }
    

    private bool IsStoneThere(int index)
    {
        if (index != -1)
        {
            //Debug.Log(i);
            //Debug.Log(j);
            if(CellAutomationSystem._cellEntities[index].isComponentStone)
            {
                //Debug.Log(j);
                return true;
            }
        }
        return false;
    }

    private void SetBoxPosition()
    {
        foreach(var e in _cellRenderGroup.GetEntities())
        {
            if (e.componentName.Name == "quad4")
            {
                e.componentPosition.X = Position.X / CellAutomationSystem._width * 2 - 1;
                //Debug.Log(e.componentPosition.X);
                e.componentPosition.Y = Position.Y / CellAutomationSystem._height * 2 - 1;
                //Debug.Log(e.componentPosition.Y);
                break;
            }
        }
    }

    private void SetBoxCell(int x, int y)
    {
        for (int i = -_w / 2+1; i < _w / 2-1; i++)
        {
            for(int j = -_h / 2; j < _h / 2; j++)
            {
                var currentX = x + i;
                var currentY = y + j;
                var index = GetCellIndex(currentX, currentY);
                if (index != -1)
                {
                    //Debug.Log(currentX);
                    //Debug.Log(currentY);
                    CellAutomationSystem._cellEntities[index].isComponentCellularAutomation = true;
                    CellAutomationSystem._cellEntities[index].isComponentPlayer = true;
                }
            }
        }
    }
    
    private void RemoveBoxCell(int x, int y)
    {
        for (int i = -_w / 2+1; i < _w / 2-1; i++)
        {
            for(int j = -_h / 2; j < _h / 2; j++)
            {
                var currentX = x + i;
                var currentY = y + j;
                var index = GetCellIndex(currentX, currentY);
                if (index != -1)
                {
                    CellAutomationSystem._cellEntities[index].isComponentCellularAutomation = false;
                    CellAutomationSystem._cellEntities[index].isComponentPlayer = false;
                }
            }
        }
    }

    private void MoveBoxCell(int sourceX, int sourceY, int targetX, int targetY)
    {
        RemoveBoxCell(sourceX, sourceY);
        SetBoxCell(targetX, targetY);
    }
    
    
    private void DrawBox()
    {
        foreach(var e in _cellRenderGroup.GetEntities())
        {
            if (e.componentName.Name == "quad4")
            {
                if (e.hasMatRenderSingle && e.matRenderSingle.Renderer is not null)
                {
                    SetTexture(e);
                    break;
                }
            }
        }
        
        InitBoxData();
        
        foreach(var e in _cellRenderGroup.GetEntities())
        {
            if (e.componentName.Name == "quad4")
            {
                if (e.matRenderSingle.Renderer is not null)
                {
                    //Debug.Log("update texture");
                    e.matRenderSingle.Renderer.Textures["MainTex"].UpdateImageContent(boxData, (uint)_w, (uint)_h);
                }
                //e.componentPosition.X = 0;
                //e.componentPosition.Y = 1;
                break;
            }
            
        }
    }
    
    private void SetEdgePixel(int posX, int posY)
    {
        var offset = (posY * _w + posX) * 4;
        boxData[offset] = 255;
        boxData[offset + 1] = 255;
        boxData[offset + 2] = 255;
        boxData[offset + 3] = 255;
    }
    
    private void InitBoxData()
    {
        for (int i = 0; i < _w; i++)
        {
            SetEdgePixel(i, 0);
            SetEdgePixel(i, _h -1);
        }
        
        for (int j = 0; j < _h; j++)
        {
            SetEdgePixel(0, j);
            SetEdgePixel(_w - 1, j);
        }
    }
    
    private void SetTexture( GameEntity e ) {
        Texture tempTexture = new Texture( GlobalVariable.GL, boxData, (uint)_w, (uint)_h );
        if( !e.hasMatPara ) {
            e.AddMatPara( null, new(){
                {
                    "MainTex", (tempTexture,default)
                }
            } );
        }
    }
    
}