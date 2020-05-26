using PasswordBoss.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PasswordBoss.Helpers
{
	// ViewModel helper for validation properties. Also raises vm property changed on update
	public class Validator<TViewModel> where TViewModel : ViewModelBase
	{
		private readonly List<ValidationProperty<TViewModel>> properties = new List<ValidationProperty<TViewModel>>();
		private readonly TViewModel target;

		public Validator(TViewModel target)
		{
			this.target = target;
		}

		public void AddProperty(string name, Func<TViewModel, bool> predicate, bool autoUpdate = false)
		{
			if (FindPropertyByName(name) != null)
				throw new ArgumentException(string.Format("property {0} already added", name));

			properties.Add(new ValidationProperty<TViewModel>(target, name, predicate, autoUpdate));
		}


		public void RemoveProperty(string name)
		{
			var item = FindPropertyByName(name);
			if (item != null)
				properties.Remove(item);
		}

		public void SetProperty(string name, bool value)
		{
			var item = FindPropertyByName(name);
			if (item != null)
			{
				bool notEq = item.IsValid != value;
				item.IsValid = value;

				if (notEq)
					target.RaisePropertyChanged(name);
			}
			else
			{
				var property = new ValidationProperty<TViewModel>(target, name, (w) => false, false);
				property.IsValid = value;
				properties.Add(property);
			}
		}

		public bool IsValid(string name)
		{
			var item = FindPropertyByName(name);
			bool res = false;
			if (item != null)
			{
				res = item.IsValid;
			}

			return res;
		}

		public void SetValid(string name)
		{
			var item = FindPropertyByName(name);
			if (item != null)
			{
				item.IsValid = true;
			}
		}

		public void SetValidAll(bool raisePropertyChanged)
		{
			foreach (var property in properties)
			{
				property.IsValid = true;
				if (raisePropertyChanged)
					target.RaisePropertyChanged(property.Name);
			}
		}

		public bool Validate(string name)
		{
			var item = FindPropertyByName(name);
			if (item != null)
			{
				item.Refresh();
				target.RaisePropertyChanged(item.Name);
				return item.IsValid;
            }

			return false;
				
		}

		public bool IsAnyError()
		{
			return properties.Any(v => !v.IsValid);
		}

		public void Refresh(bool raisePropertyChanged)
		{
			foreach (var property in properties)
			{
				property.Refresh();
				if (raisePropertyChanged)
					target.RaisePropertyChanged(property.Name);
			}
		}

		private ValidationProperty<TViewModel> FindPropertyByName(string name)
		{
			return properties.SingleOrDefault(p => p.Name == name);
		}
	}
}
