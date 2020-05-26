using PasswordBoss.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PasswordBoss.Helpers
{
	public class PasswordBoxViewModel
	{
		public event EventHandler Updated;
		public bool IsPresentNumber { get; set; }
		public bool IsPresentCharacterUpper { get; set; }
		public bool IsPresentCharacterLower { get; set; }
		public bool IsPresentSymbol { get; set; }
		public bool IsPresent8Chars { get; set; }
		public bool IsPasswordValid
		{
			get
			{
				return IsPresentCharacterUpper
						&& IsPresentNumber
						&& IsPresentCharacterLower
						&& IsPresentSymbol
						&& IsPresent8Chars;
			}
		}

		public string _password = "";
		public string Password
		{
			get { return _password; }
			set { SetPasswordAndValidate(value); }
		}

		public string PlaceHolderText
		{
			get
			{
				//return string.IsNullOrEmpty(_password) ?
				//    (string)System.Windows.Application.Current.FindResource("MasterPassword")
				//    : _password;
				return _password;
			}
		}

		public bool EyeImageVisibility
		{
			get { return !string.IsNullOrEmpty(_password); }
		}

		private readonly string propertyName = "";
		private readonly ViewModelBase host;

		public PasswordBoxViewModel(ViewModelBase host, string propertyName)
		{
			this.propertyName = propertyName;
			this.host = host;
		}

		private void SetPasswordAndValidate(string password)
		{
			_password = password == null ? "" : password;

			Action<string> raise = (prop) =>
			{
				host.RaisePropertyChanged(string.Format("{0}.{1}", propertyName, prop));
			};

			IsPresentCharacterLower = password.Any(c => char.IsLower(c));
			raise("IsPresentCharacterLower");

			IsPresentCharacterUpper = password.Any(c => char.IsUpper(c));
			raise("IsPresentCharacterUpper");

			IsPresentNumber = password.Any(c => char.IsDigit(c));
			raise("IsPresentNumber");

			IsPresentSymbol = password.Any(c => !char.IsLetterOrDigit(c));
			raise("IsPresentSymbol");

			IsPresent8Chars = password.Length >= 8;
			raise("IsPresent8Chars");
			raise("EyeImageVisibility");
			raise("PlaceHolderText");
			raise("AllChecked");
			host.RaisePropertyChanged(propertyName);

			if (Updated != null)
			{
				Updated(this, EventArgs.Empty);
			}
		}
	}
}
