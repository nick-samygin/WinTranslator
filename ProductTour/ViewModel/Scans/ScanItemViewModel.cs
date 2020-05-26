using ProductTour.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows.Media;

namespace ProductTour.ViewModel.Scans
{
	public class ScanItemViewModel
	{
		private readonly ScanItem scanItem;

		public ScanItemViewModel()
		{
		}

		public ScanItemViewModel(ScanItem scanItem)
			: this()
		{
			this.scanItem = scanItem == null ? new ScanItem("", "", "") : scanItem;
		}

		public ScanItemViewModel(ScanItemViewModel copy)
			: this(copy.scanItem)
		{
		}

		public string Username { get { return scanItem.Username; } }
		public string Site { get { return scanItem.Site; } }
		public string Password
		{
			get
			{

				return scanItem.Password;

			}

		}

		public string RiskText
		{
			get
			{
				var plainText = scanItem.Risk.GetDefinition();
				return (string)System.Windows.Application.Current.FindResource(plainText);
			}
		}

		public string RiskImage
		{
			get
			{
				var res = scanItem.Risk.GetImage();
				return res;
			}
		}

		public SolidColorBrush RiskColor
		{
			get
			{
				var res = scanItem.Risk.GetColor();
				return (SolidColorBrush)System.Windows.Application.Current.FindResource(res);
			}
		}
	}
}
