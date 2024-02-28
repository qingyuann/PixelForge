using Entitas;


namespace Component;

[Game]
public class LiquidComponent: IComponent
{
    public string ColorType;
    
    // oil = 0, water = 1, acid = 2, lava = 3
    public int Density;
    
    // when it comes to fire
    // not flammable = 0, flammable = 1, explosive = 2
    public int Flammability;
}