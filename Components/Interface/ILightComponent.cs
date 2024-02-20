namespace pp;

public interface ILightComponent {
	public bool Enabled{get;set;}
	public int[] Layers {get;set;}
	public int LightOrder {get;set;}
}