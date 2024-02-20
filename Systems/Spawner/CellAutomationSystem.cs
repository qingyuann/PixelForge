using System.Drawing;
using Entitas;
using Silk.NET.Input;
using Render;


namespace PixelForge;

public class CellAutomationSystem : IInitializeSystem, IExecuteSystem
{
    private	readonly Contexts _contexts;
    IGroup<GameEntity> _cellRenderGroup;
    private uint _width;
    private uint _height;
    private GameEntity[] _cellEntities;
    private byte[] _cellColors;
    
    
    public CellAutomationSystem( Contexts contexts )
    {
        _contexts = contexts;
    }

    public void Initialize()
    {
        _width = 500;
        _height = 500;
        
        _cellRenderGroup = _contexts.game.GetGroup( GameMatcher.AllOf( GameMatcher.ComponentCellAutoTexture, GameMatcher.MatRenderSingle ) );
        
        
        
        _cellColors = new byte[_width * _height * 4];
        _cellEntities = new GameEntity[_width * _height];
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
        //InputSystem.AddMouseDownEvent( OnMouseDown );
    }
    
    void SetTexture( GameEntity e ) {
        //这里新建纹理，添加到entity的matPara中，render system识别到para后将纹理添加到shader
        //shader中会储存纹理的引用，所以这里不需要储存纹理的引用
        Texture tempTexture = new Texture( GlobalVariable.GL, _cellColors, _width, _height );
        if( !e.hasMatPara ) {
            //不添加属性参数，添加纹理参数
            e.AddMatPara( null, new Dictionary<string, object>(){
                {
                    "MainTex", tempTexture
                }
            } );
        }
    }
    
    public void Execute()
    {
        if (InputSystem.GetKeyDown(Key.Space))
        {
            TestGenerateSand();
        }

        for (int i = 0; i < _width; i++)
            for(int j = (int)_height-1; j >= 0; j--)
            {
                var e = _cellEntities[ComputeIndex(i, j)];
                if (e.isComponentSand)
                {
                    SandBehavior(i, j, e);
                }
            }
        
        //update the color of the texture
        foreach(var e in _cellRenderGroup.GetEntities())
        {
            if( e.matRenderSingle.Renderer is not null )
            {
                //Debug.Log("update texture");
                e.matRenderSingle.Renderer.Textures["MainTex"].UpdateImageContent( _cellColors, _width, _height );
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

    
    private void OnMouseDown(IMouse mouse, MouseButton button) {
        
        if (button == MouseButton.Left)
        {
            var x = (int) (mouse.Position.X * _width);
            var y = (int) (mouse.Position.Y * _height);
            
            var index = ComputeIndex(x, y);

            var e = _cellEntities[index];
            e.isComponentCellularAutomation = true;
            e.isComponentSand = true;

            SetCellColor(index, "sand");
        }
    }

    private void TestGenerateSand()
    {
        
        var x = (int) (0.5 * _width);
        var y = (int) (0.1 * _height);
        
        var index = ComputeIndex(x, y);
    
        //Debug.Log("index" + index);
        var e = _cellEntities[index];
        e.isComponentCellularAutomation = true;
        e.isComponentSand = true;

        SetCellColor(index, "sand");
        
    }
    
    private int ComputeIndex(int i, int j)
    {
        var index = (int)(j * _width + i);
        return index;
    }
    
    private void SetCellColor(int index, string colorType)
    {
       
        if (colorType == "sand")
        {
            var offset = index * 4;
            //Debug.Log("set sand color");
            _cellColors[offset] = GlobalVariable.SandColor[0];
            _cellColors[offset + 1] = GlobalVariable.SandColor[1];
            _cellColors[offset + 2] = GlobalVariable.SandColor[2];
            _cellColors[offset + 3] = GlobalVariable.SandColor[3];
        }

        if (colorType == "none")
        {
            var offset = index * 4;
            _cellColors[offset] = 0;
            _cellColors[offset + 1] = 0;
            _cellColors[offset + 2] = 0;
            _cellColors[offset + 3] = 0;
        }
    }
    
    
    
    private void SandBehavior(int i, int j, GameEntity e)
    {
        var id = ComputeIndex(i, j);
        var idDown = ComputeIndex(i, j + 1);
        var idDownLeft = ComputeIndex(i - 1, j + 1);
        var idDownRight = ComputeIndex(i + 1, j + 1);
        
        if (j == _height - 1 || i == 0 || i == _width - 1)
        {
            return;
        }

        else
        {
            if(!_cellEntities[idDown].isComponentCellularAutomation)
            {
                //move down: just let down cell get the reference of this entity
                //entity didn't change, just change the reference in the _cellEntities list
                _cellEntities[idDown].isComponentCellularAutomation = true;
                _cellEntities[idDown].isComponentSand = true;
                
                _cellEntities[id].isComponentCellularAutomation = false;
                _cellEntities[id].isComponentSand = false;
                
                SetCellColor(idDown, "sand");
                SetCellColor(id, "none");
                
                return;
            }
        
        
            else if(!_cellEntities[idDownLeft].isComponentCellularAutomation)
            {
                _cellEntities[idDownLeft].isComponentCellularAutomation = true;
                _cellEntities[idDownLeft].isComponentSand = true;
                
                _cellEntities[id].isComponentCellularAutomation = false;
                _cellEntities[id].isComponentSand = false;
                SetCellColor(idDownLeft, "sand");
                SetCellColor(id, "none");
                
                return;
            }
        
        
            else if(!_cellEntities[idDownRight].isComponentCellularAutomation)
            {
                _cellEntities[idDownLeft].isComponentCellularAutomation = true;
                _cellEntities[idDownLeft].isComponentSand = true;
                
                _cellEntities[id].isComponentCellularAutomation = false;
                _cellEntities[id].isComponentSand = false;
                SetCellColor(idDownRight, "sand");
                SetCellColor(id, "none");
                
                return;
            }
        }
            
        
        
        
    }
    
}