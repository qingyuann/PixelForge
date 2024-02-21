namespace pp;

public interface ILightComponent {
	public bool Enabled{get;set;}
	public int[] Layers {get;set;}
	//render from small to large
	public int LightOrder {get;set;}
}