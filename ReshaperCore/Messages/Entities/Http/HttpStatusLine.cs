namespace ReshaperCore.Messages.Entities.Http
{
	/// <summary>
	/// Represents the status line of a HTTP message
	/// </summary>
	public abstract class HttpStatusLine : EntityContainer
	{
		private string _version;

		/// <summary>
		/// The HTTP version
		/// </summary>
		public virtual string Version
		{
			set
			{
				_version = value;
				OnPropertyChanged(nameof(Version));
			}
			get
			{
				return _version;
			}
		}
	}
}
