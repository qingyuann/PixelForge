using System.Drawing;
using Entitas;
using PixelForge.Spawner.CellAuto.Movable;
using Silk.NET.Input;
using Render;


namespace PixelForge.Spawner.CellAuto;

public class CellAutomationSystem : IInitializeSystem, IExecuteSystem
{
    private	readonly Contexts _contexts;
    IGroup<GameEntity> _cellRenderGroup;
    public static int _width;
    public static int _height;
    public static GameEntity[] _cellEntities;
    public static byte[] _cellColors;
    
    
    public CellAutomationSystem( Contexts contexts )
    {
        _contexts = contexts;
        _width = 250;
        _height = 250;
        _cellColors = new byte[_width * _height * 4];
        _cellEntities = new GameEntity[_width * _height];
        _cellRenderGroup = _contexts.game.GetGroup( GameMatcher.AllOf( GameMatcher.ComponentCellAutoTexture, GameMatcher.MatRenderSingle ) );
    }

    public void Initialize()
    {
        
        InitCellEntities();
        InitCellColors();
        
        foreach(var e in _cellRenderGroup.GetEntities())
        {
            if(e.hasMatRenderSingle && e.matRenderSingle.Renderer is not null)
            {
                SetTexture(e);
            }
        }
        
        //generate a sand on click position
        InputSystem.AddMouseDownEvent( OnMouseDown );
    }
    
    
    
    public void Execute()
    {
        TestGenerateStone();
        if (InputSystem.GetKey(Key.K))
        {
            TestGenerateSand();
        }

        if (InputSystem.GetKey(Key.L))
        {
            TestGenerateWater();
        }
        
        
        
        for(int j = _height-1; j >= 0; j--)
            for (int i = _width-1; i >= 0; i--)
            {
                var e = _cellEntities[CellTools.ComputeIndex(i, j)];
                if (e.isComponentSand)
                {
                    SandBehaviour.Act(i, j);
                }
                if (e.isComponentWater)
                {
                    WaterBehaviour.Act(i, j);
                }
            }
        
        //update the color of the texture
        foreach(var e in _cellRenderGroup.GetEntities())
        {
            if( e.matRenderSingle.Renderer is not null )
            {
                //Debug.Log("update texture");
                e.matRenderSingle.Renderer.Textures["MainTex"].UpdateImageContent( _cellColors, (uint)_width, (uint)_height );
            }
        }
        
    }
    
    private void InitCellEntities()
    {
        for (int i = 0; i < _width * _height; i++)
        {
            _cellEntities[i] = _contexts.game.CreateEntity();
            _cellEntities[i].isComponentCellularAutomation = false;
        }
    }
    private void InitCellColors()
    {
        for (int i = 0; i < _width * _height  * 4; i++)
        {
            _cellColors[i] = 0;
        }
    }

    void SetTexture( GameEntity e ) {
        Texture tempTexture = new Texture( GlobalVariable.GL, _cellColors, (uint)_width, (uint)_height );
        if( !e.hasMatPara ) {
            e.AddMatPara( null, new Dictionary<string, object>(){
                {
                    "MainTex", tempTexture
                }
            } );
        }
    }
    
    private void OnMouseDown(IMouse mouse, MouseButton button) {
        
        if (button == MouseButton.Left)
        {
            var x = (int) (mouse.Position.X / 1000 * _width);
            var y = (int)(mouse.Position.Y / 1000 * _height);
            
            //Debug.Log("x: " + x + " y: " + y);
            
            int widthSize = 20; 
            int heightSize = 3;
            Random random = new Random();
            
            for (int i = -widthSize / 2; i <= widthSize / 2; i++)
            {
                for (int j = -heightSize / 2; j <= heightSize / 2; j++)
                {
                    if (random.NextDouble() < 0.4)
                    {
                        var currentX = x + i;
                        var currentY = y + j;
                        var index = CellTools.ComputeIndex(currentX, currentY);

                        if (index == -1)
                        {
                            Debug.Log("index out of range: " + currentX + ", " + currentY);
                            continue;
                        }

                        var e = _cellEntities[index];
                        e.isComponentCellularAutomation = true;
                        e.isComponentSand = true;

                        CellTools.SetCellColor(index, "sand");
                    }
                    
                }
            }
        }
        
        if (button == MouseButton.Right)
        {
            var x = (int) (mouse.Position.X / 1000 * _width);
            var y = (int)(mouse.Position.Y / 1000 * _height);
            
            //Debug.Log("x: " + x + " y: " + y);
           
            int widthSize = 20; 
            int heightSize = 3;
            Random random = new Random();
            
            for (int i = -widthSize / 2; i <= widthSize / 2; i++)
            {
                for (int j = -heightSize / 2; j <= heightSize / 2; j++)
                {
                    if (random.NextDouble() < 0.4)
                    {
                        var currentX = x + i;
                        var currentY = y + j;
                        var index = CellTools.ComputeIndex(currentX, currentY);

                        if (index == -1)
                        {
                            Debug.Log("index out of range: " + currentX + ", " + currentY);
                            continue;
                        }

                        var e = _cellEntities[index];
                        e.isComponentCellularAutomation = true;
                        e.isComponentWater = true;

                        CellTools.SetCellColor(index, "water");
                    }
                }
            }
        }
    }

    private void TestGenerateSand()
    {
        
        var x = (int) (0.5 * _width);
        var y = (int) (0.2 * _height);
        
        var index = CellTools.ComputeIndex(x, y);
    
        //Debug.Log("index" + index);
        var e = _cellEntities[index];
        e.isComponentCellularAutomation = true;
        e.isComponentSand = true;

        CellTools.SetCellColor(index, "sand");
    }
    
    private void TestGenerateWater()
    {
        
        var x = (int) (0.5 * _width);
        var y = (int) (0.2 * _height);
        
        var index = CellTools.ComputeIndex(x, y);
    
        //Debug.Log("index" + index);
        var e = _cellEntities[index];
        e.isComponentCellularAutomation = true;
        e.isComponentWater = true;

        CellTools.SetCellColor(index, "water");
    }
    

    private void TestGenerateStone()
    {
        var x = (int) (0.5 * _width);
        var y = (int) (0.6 * _height);
        
        int widthSize = 120; 
        int heightSize = 20;
        
            
        for (int i = -widthSize / 2; i <= widthSize / 2; i++)
        {
            for (int j = -heightSize / 2; j <= heightSize / 2; j++)
            {
                var currentX = x + i;
                var currentY = y + j;
                var index = CellTools.ComputeIndex(currentX, currentY);

                if (index == -1)
                {
                    Debug.Log("index out of range: " + currentX + ", " + currentY);
                    continue;
                }

                var e = _cellEntities[index];
                e.isComponentCellularAutomation = true;
                e.isComponentStone = true;

                CellTools.SetCellColor(index, "stone");
                    
            }
        }
        
    }
    
}