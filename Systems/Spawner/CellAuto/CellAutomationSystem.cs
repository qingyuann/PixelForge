using System.Drawing;
using System.Numerics;
using Component;
using Entitas;
using PixelForge.Spawner.CellAuto.Movable;
using PixelForge.Tools;
using Silk.NET.Input;
using Silk.NET.OpenGL;
using Texture = Render.Texture;


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
        
        TestGenerateStone();
        
        //generate a sand on click position
        InputSystem.AddMouseDownEvent( OnMouseDown );
    }
    
    
    
    public void Execute()
    {
        
        if (InputSystem.GetKey(Key.K))
        {
            TestGenerateSand();
        }

        if (InputSystem.GetKey(Key.L))
        {
            TestGenerateWater();
        }
        
        if (InputSystem.GetKeyDown(Key.J))
        {
            TestGenerateBombFire();
        }
        
        if (InputSystem.GetKeyDown(Key.I))
        {
            TestGenerateOli();
        }
        
        if (InputSystem.GetKeyDown(Key.O))
        {
            TestGenerateAcid();
        }
        
        // from bottom to top, from right to left
        for(int j = _height-1; j >= 0; j--)
            for (int i = _width-1; i >= 0; i--)
            {
                var e = _cellEntities[CellTools.ComputeIndex(i, j)];
                if (e.isComponentCellUpdate == false)
                {
                    if (e.hasComponentFire)
                    {
                        FireBehaviour.Act(i, j);
                        continue;
                    }
                    if (e.hasComponentExplodeFire)
                    {
                        ExplodeFireBehaviour.Act(i, j);
                        continue;
                    }
                    if (e.hasComponentLiquid)
                    {
                        LiquidBehaviour.Act(i, j);
                        continue;
                    }
                    if (e.isComponentSand)
                    {
                        SandBehaviour.Act(i, j);
                        continue;
                    }
                    
                    /*
                    if (e.isComponentWater)
                    {
                        WaterBehaviour.Act(i, j);
                        continue;
                    }
                    */
                    
                }
                e.isComponentCellUpdate = true;
            }
        
        // from top to bottom, from right to left
        for(int j = 0; j < _height; j++)
            for (int i = _width-1; i >= 0; i--)
            {
                var e = _cellEntities[CellTools.ComputeIndex(i, j)];
                if (e.isComponentSteam)
                {
                    SteamBehaviour.Act(i, j);
                }

                if (e.isComponentSmoke)
                {
                    SmokeBehaviour.Act(i, j);
                }
                e.isComponentCellUpdate = false;
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
            _cellEntities[i].isComponentCellUpdate = false;
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
            
            int widthSize = 30; 
            int heightSize = 10;
            Random random = new Random();
            
            for (int i = -widthSize / 2; i <= widthSize / 2; i++)
            {
                for (int j = -heightSize / 2; j <= heightSize / 2; j++)
                {
                    if (random.NextDouble() < 0.6)
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

                        CellTools.SetCellColor(index, "steam");
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
                        if(!e.hasComponentFire){e.AddComponentFire(200, 5);}

                        CellTools.SetCellColor(index, "fire");
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
    
    private void TestGenerateOli()
    {
        
        var x = (int) (0.5 * _width);
        var y = (int) (0.2 * _height);
        
        var index = CellTools.ComputeIndex(x, y);
    
        //Debug.Log("index" + index);
        var e = _cellEntities[index];
        e.isComponentCellularAutomation = true;
        //e.isComponentWater = true;
        if(!e.hasComponentLiquid){e.AddComponentLiquid("oli", 0,1);}

        CellTools.SetCellColor(index, "oli");
    }
    
    private void TestGenerateWater()
    {
        var x = (int) (0.5 * _width);
        var y = (int) (0.2 * _height);
        
        var index = CellTools.ComputeIndex(x, y);
    
        //Debug.Log("index" + index);
        var e = _cellEntities[index];
        e.isComponentCellularAutomation = true;
        //e.isComponentWater = true;
        if(!e.hasComponentLiquid){e.AddComponentLiquid("water", 1,0);}

        CellTools.SetCellColor(index, "water");
    }
    
    private void TestGenerateAcid()
    {
        var x = (int) (0.5 * _width);
        var y = (int) (0.2 * _height);
        
        var index = CellTools.ComputeIndex(x, y);
    
        //Debug.Log("index" + index);
        var e = _cellEntities[index];
        e.isComponentCellularAutomation = true;
        //e.isComponentWater = true;
        if(!e.hasComponentLiquid){e.AddComponentLiquid("acid", 2,1);}

        CellTools.SetCellColor(index, "acid");
    }
    
    
    private void TestGenerateFire()
    {
        
        var x = (int) (0.5 * _width);
        var y = (int) (0.2 * _height);
        
        var index = CellTools.ComputeIndex(x, y);
    
        //Debug.Log("index" + index);
        var e = _cellEntities[index];
        e.isComponentCellularAutomation = true;
        e.AddComponentFire(300, 5);

        CellTools.SetCellColor(index, "fire");
    }
    
    private void TestGenerateBombFire()
    {
        var x = RandomTool.Range(0, _width);
        var y = RandomTool.Range(0, _height);
        
        // x,y as the center of the bomb, generate a circle of fire bomb
        // all fire bomb has a travel time of 40, direction is a circle (vector2) 
        var l = GetCirclePoints(x, y, 3);
        foreach (var (i, j) in l)
        {
            var index = CellTools.ComputeIndex(i, j);
            var dir = new Vector2(i - x, j - y);
            if (index != -1)
            {
                var e = _cellEntities[index];
                e.isComponentCellularAutomation = true;
                if (!e.hasComponentExplodeFire)
                {
                    e.AddComponentExplodeFire(dir, 40);
                }
                CellTools.SetCellColor(index, "bombFire");
            }
            
        }
        
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
    
    List<(int, int)> GetCirclePoints(int centerX, int centerY, int radius)
    {
        List<(int, int)> points = new List<(int, int)>();
        int x = 0, y = radius;
        int d = 1 - radius;

        while (x <= y)
        {
            // 利用对称性绘制八个点
            points.Add((centerX + x, centerY + y));
            points.Add((centerX + y, centerY + x));
            points.Add((centerX - x, centerY + y));
            points.Add((centerX - y, centerY + x));
            points.Add((centerX + x, centerY - y));
            points.Add((centerX + y, centerY - x));
            points.Add((centerX - x, centerY - y));
            points.Add((centerX - y, centerY - x));

            if (d < 0)
            {
                d += 2 * x + 3;
            }
            else
            {
                d += 2 * (x - y) + 5;
                y--;
            }
            x++;
        }
        
        return points;
    }
}