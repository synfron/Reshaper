namespace ReshaperUI.Display.Interfaces
{
	public interface IModelPresenter<T> : IModelPresenter
	{
	}

	public interface IModelIndependentPresenter<T> : IModelPresenter<T>, IModelIndependentPresenter
	{

	}

	public interface IModelPresenter
	{
		void SetModel(object model);
	}

	public interface IModelIndependentPresenter : IModelPresenter
	{
		void Show();
	}
}
