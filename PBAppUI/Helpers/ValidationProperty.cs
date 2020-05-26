using System;

namespace PasswordBoss.Helpers
{
	public class ValidationProperty<TViewModel>
	{
		public string Name { get; private set; }

		private bool isValid = false;
		public bool IsValid
		{
			get
			{
				if (autoUpdate)
				{
					Refresh();
				}

				return isValid;
			}

			// HACK:
			set
			{
				isValid = value;
			}
		}

		private readonly bool autoUpdate;
		private readonly TViewModel viewModel;
		private readonly Func<TViewModel, bool> property;

		public ValidationProperty(TViewModel viewModel, string name, Func<TViewModel, bool> property, bool autoUpdate = false)
		{
			this.autoUpdate = autoUpdate;
			this.viewModel = viewModel;
			this.Name = name;
			this.property = property;
		}
		public void Refresh()
		{
			isValid = property(viewModel);
		}
	}
}
