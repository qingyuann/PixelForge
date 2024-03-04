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
    public static byte[] _cellColorsFire;
    public static byte[] _cellColorsWater;
    public static byte[] _cellColorsSand;
    public static byte[] _cellColorsOli;
    public static byte[] _cellColorsAcid;
    public static byte[] _cellColorsLava;
    public static byte[] _cellColorsStone;
    public static byte[] _cellColorsSteam;
    public static byte[] _cellColorsSmoke;
    
    public static List<byte[]> _cellColorsList;
    
    
    public CellAutomationSystem( Contexts contexts )
    {
        _contexts = contexts;
        _width = 256;
        _height = 256;
        
        _cellColors = new byte[_width * _height * 4];
        _cellColorsFire = new byte[_width * _height * 4];
        _cellColorsWater = new byte[_width * _height * 4];
        _cellColorsSand = new byte[_width * _height * 4];
        _cellColorsOli = new byte[_width * _height * 4];
        _cellColorsAcid = new byte[_width * _height * 4];
        _cellColorsLava = new byte[_width * _height * 4];
        _cellColorsStone = new byte[_width * _height * 4];
        _cellColorsSteam = new byte[_width * _height * 4];
        _cellColorsSmoke = new byte[_width * _height * 4];
        
        
        _cellColorsList = new List<byte[]>{_cellColors, _cellColorsFire, _cellColorsWater, _cellColorsSand, _cellColorsOli, _cellColorsAcid, _cellColorsLava, _cellColorsStone};
        
        
        _cellEntities = new GameEntity[_width * _height];
        _cellRenderGroup = _contexts.game.GetGroup( GameMatcher.AllOf( GameMatcher.ComponentCellAutoTexture, GameMatcher.MatRenderSingle ) );
    }

    public void Initialize()
    {
        
        InitCellEntities();
        
        foreach(var e in _cellRenderGroup.GetEntities())
        {
            if (e.componentName.Name == "quad3")
            {
                if (e.hasMatRenderSingle && e.matRenderSingle.Renderer is not null)
                {
                    SetTexture(e);
                }
            }
        }
        
        //TestGenerateStone();
        
        //generate a sand on click position
        //InputSystem.AddMouseDownEvent( OnMouseDown );
    }
    
    
    
    public void Execute()
    {
        /*
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
        */
        
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
                    if (e.isComponentSand)
                    {
                        SandBehaviour.Act(i, j);
                        continue;
                    }
                    if (e.hasComponentLiquid)
                    {
                        LiquidBehaviour.Act(i, j);
                        continue;
                    }
                    
                    if (e.hasComponentExplodeFire)
                    {
                        ExplodeFireBehaviour.Act(i, j);
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
            if (e.componentName.Name == "quad3")
            {
                if (e.matRenderSingle.Renderer is not null)
                {
                    //Debug.Log("update texture");
                    e.matRenderSingle.Renderer.Textures["MainTex"]
                        .UpdateImageContent(_cellColorsStone, (uint)_width, (uint)_height);
                    e.matRenderSingle.Renderer.Textures["fire"]
                        .UpdateImageContent(_cellColorsFire, (uint)_width, (uint)_height);
                    e.matRenderSingle.Renderer.Textures["water"]
                        .UpdateImageContent(_cellColorsWater, (uint)_width, (uint)_height);
                    e.matRenderSingle.Renderer.Textures["sand"]
                        .UpdateImageContent(_cellColorsSand, (uint)_width, (uint)_height);
                    e.matRenderSingle.Renderer.Textures["oli"]
                        .UpdateImageContent(_cellColorsOli, (uint)_width, (uint)_height);
                    e.matRenderSingle.Renderer.Textures["acid"]
                        .UpdateImageContent(_cellColorsAcid, (uint)_width, (uint)_height);
                    e.matRenderSingle.Renderer.Textures["lava"]
                        .UpdateImageContent( _cellColorsLava, (uint)_width, (uint)_height );
                    e.matRenderSingle.Renderer.Textures["stone"]
                        .UpdateImageContent( _cellColorsStone, (uint)_width, (uint)_height );
                    
                }
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
    

    void SetTexture( GameEntity e ) {
        Texture tempTexture = new Texture( GlobalVariable.GL, _cellColors, (uint)_width, (uint)_height );
        Texture fireTexture = new Texture( GlobalVariable.GL, _cellColorsFire, (uint)_width, (uint)_height );
        Texture waterTexture = new Texture( GlobalVariable.GL, _cellColorsWater, (uint)_width, (uint)_height );
        Texture sandTexture = new Texture( GlobalVariable.GL, _cellColorsSand, (uint)_width, (uint)_height );
        Texture oliTexture = new Texture( GlobalVariable.GL, _cellColorsOli, (uint)_width, (uint)_height );
        Texture acidTexture = new Texture( GlobalVariable.GL, _cellColorsAcid, (uint)_width, (uint)_height );
        Texture lavaTexture = new Texture( GlobalVariable.GL, _cellColorsLava, (uint)_width, (uint)_height );
        Texture stoneTexture = new Texture( GlobalVariable.GL, _cellColorsStone, (uint)_width, (uint)_height );
        
        if( !e.hasMatPara ) {
            e.AddMatPara( null, new (){
                {
                    "MainTex", (tempTexture,default)
                },
                {
                    "fire", (fireTexture, default)
                }
                ,
                {
                    "water", (waterTexture, default)
                }
                ,
                {
                    "sand", (sandTexture, default)
                }
                ,
                {
                    "oli", (oliTexture, default)
                }
                ,
                {
                    "acid", (acidTexture, default)
                }
                ,
                {
                    "lava", (lavaTexture, default)
                }
                ,
                {
                    "stone", (stoneTexture, default)
                }
                
               
            } );
        }
    }
    
    private void OnMouseDown(IMouse mouse, MouseButton button) {
        
        if (button == MouseButton.Left)
        {
            var x = (int) (mouse.Position.X / GameSetting.WindowWidth * _width);
            var y = (int)(mouse.Position.Y / GameSetting.WindowHeight * _height);
            
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
            var x = (int) (mouse.Position.X / GameSetting.WindowWidth * _width);
            var y = (int)(mouse.Position.Y / GameSetting.WindowHeight * _height);
            
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

    public static void TestGenerateSand(int x, int y)
    {
        int widthSize = 20;
        int heightSize = 10;
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

                    if (index == -1 || _cellEntities[index].isComponentCellularAutomation)
                    {
                        //Debug.Log("index out of range: " + currentX + ", " + currentY);
                        break;
                    }

                    var e = _cellEntities[index];
                    e.isComponentCellularAutomation = true;
                    e.isComponentSand = true;

                    CellTools.SetCellColor(index, "sand");
                }
            }
        }
    }
    
    public static void TestGenerateOli(int x, int y)
    {
        int widthSize = 20; 
        int heightSize = 10;
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

                    if (index == -1 || _cellEntities[index].isComponentCellularAutomation)
                    {
                        //Debug.Log("index out of range: " + currentX + ", " + currentY);
                        break;
                    }

                    var e = _cellEntities[index];
                    e.isComponentCellularAutomation = true;
                    //e.isComponentWater = true;
                    if(!e.hasComponentLiquid){e.AddComponentLiquid("oli", 0,1);}

                    CellTools.SetCellColor(index, "oli");
                }
            }
        }
        
    }
    
    public static void TestGenerateWater(int x, int y)
    {
        //var x = (int) (0.5 * _width);
        //var y = (int) (0.2 * _height);
        int widthSize = 20; 
        int heightSize = 10;
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

                    if (index == -1 || _cellEntities[index].isComponentCellularAutomation)
                    {
                        //Debug.Log("index out of range: " + currentX + ", " + currentY);
                        break;
                    }

                    var e = _cellEntities[index];
                    e.isComponentCellularAutomation = true;
                    if(!e.hasComponentLiquid){e.AddComponentLiquid("water", 1,0);}

                    CellTools.SetCellColor(index, "water");
                }
            }
        }
        

        
    }
    
    public static void TestGenerateAcid(int x, int y)
    {
        int widthSize = 20; 
        int heightSize = 10;
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

                    if (index == -1 || _cellEntities[index].isComponentCellularAutomation)
                    {
                        //Debug.Log("index out of range: " + currentX + ", " + currentY);
                        break;
                    }

                    var e = _cellEntities[index];
                    e.isComponentCellularAutomation = true;
                    //e.isComponentWater = true;
                    if(!e.hasComponentLiquid){e.AddComponentLiquid("acid", 2,1);}

                    CellTools.SetCellColor(index, "acid");
                }
            }
        }
        
        
        
        
    }




    public static void TestGenerateLava(int x, int y)
    {
        int widthSize = 20;
        int heightSize = 10;
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

                    if (index == -1 || _cellEntities[index].isComponentCellularAutomation)
                    {
                        //Debug.Log("index out of range: " + currentX + ", " + currentY);
                        break;
                    }

                    var e = _cellEntities[index];
                    e.isComponentCellularAutomation = true;
                    //e.isComponentWater = true;
                    if (!e.hasComponentLiquid)
                    {
                        e.AddComponentLiquid("lava", 3, 1);
                    }

                    CellTools.SetCellColor(index, "lava");
                }
            }
        }
    }


    public static void TestGenerateFire(int x, int y)
    {
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

                    if (index == -1 || _cellEntities[index].isComponentCellularAutomation)
                    {
                        //Debug.Log("index out of range: " + currentX + ", " + currentY);
                        break;
                    }


                    var e = _cellEntities[index];
                    e.isComponentCellularAutomation = true;
                    if(!e.hasComponentFire){e.AddComponentFire(60, 20);}

                    CellTools.SetCellColor(index, "fire");
                }
            }
        }
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

    public static void TestGenerateStone(int x, int y)
    {
        //var x = (int) (0.5 * _width);
        //var y = (int) (0.6 * _height);
        
        int widthSize = 10; 
        int heightSize = 5;
        
            
        for (int i = -widthSize / 2; i <= widthSize / 2; i++)
        {
            for (int j = -heightSize / 2; j <= heightSize / 2; j++)
            {
                var currentX = x + i;
                var currentY = y + j;
                var index = CellTools.ComputeIndex(currentX, currentY);

                if (index == -1)
                {
                    //Debug.Log("index out of range: " + currentX + ", " + currentY);
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