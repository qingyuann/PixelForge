namespace pp;

public interface IPostProcessingComponent {
	public bool Enabled{get;set;}
	public int[] Layers {get;set;}
}