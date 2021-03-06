//
// mTouch-PDFReader library
// OptionsTableViewController.cs (Options table view controller)
//
//  Author:
//       Alexander Matsibarov (macasun) <amatsibarov@gmail.com>
//
//  Copyright (c) 2012 Alexander Matsibarov
//
//  This program is free software: you can redistribute it and/or modify
//  it under the terms of the GNU General Public License as published by
//  the Free Software Foundation, either version 3 of the License, or
//  (at your option) any later version.
//
//  This program is distributed in the hope that it will be useful,
//  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  GNU General Public License for more details.
//
//  You should have received a copy of the GNU General Public License
//  along with this program.  If not, see <http://www.gnu.org/licenses/>.
//

using System;
using System.Drawing;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using mTouchPDFReader.Library.Managers;
using mTouchPDFReader.Library.Data.Objects;
using mTouchPDFReader.Library.Data.Enums;

namespace mTouchPDFReader.Library.Views.Management
{
	public class OptionsTableViewController : UITableViewController
	{
		#region Constants		
		private const int DefaultCellWidth = 600;
		private const int DefaultCellHeight = 50;		
		#endregion
		
		#region Fields		
		private UITableViewCell _PageTransitionStyleCell;
		private UITableViewCell _PageNavigationOrientationCell;
		private UITableViewCell _AutoScaleMode;
		private UITableViewCell _ToolbarVisibilityCell;
		private UITableViewCell _SliderVisibility;
		private UITableViewCell _PageNumberVisibilityCell;
		private UITableViewCell _ZoomScaleLevelsCell;
		private UITableViewCell _ZoomByDoubleTouchCell;
		private UITableViewCell _LibraryReleaseDateCell;
		private UITableViewCell _LibraryVersionCell;		
		#endregion
		
		#region Constructors		
		public OptionsTableViewController(IntPtr handle) : base(handle) { }

		[Export("initWithCoder:")]
		public OptionsTableViewController(NSCoder coder) : base(coder) { }

		public OptionsTableViewController() : base(null, null) { }
		#endregion	

		#region UIViewController members
		public override void ViewDidLoad()
		{
			base.ViewDidLoad();
			View.AutoresizingMask = UIViewAutoresizing.FlexibleWidth | UIViewAutoresizing.FlexibleHeight;
   
			_PageTransitionStyleCell = _CreatePageTransitionStyleCell();
			_PageNavigationOrientationCell = _CreatePageNavigationOrientationCell();
			_AutoScaleMode = _CreateAutoScaleModeCell();

			_ToolbarVisibilityCell = _CreateToolbarVisibilityCell();
			_SliderVisibility = _CreateSliderVisibilityCell();
			_PageNumberVisibilityCell = _CreatePageNumberVisibilityCell();

			_ZoomScaleLevelsCell = _CreateZoomScaleLevelsCell();
			_ZoomByDoubleTouchCell = _CreatemZoomByDoubleTouchCell();

			_LibraryReleaseDateCell = _CreateLibraryReleaseDateCell();
			_LibraryVersionCell = _CreateLibraryVersionCell();

			TableView = new UITableView(View.Bounds, UITableViewStyle.Grouped)
			            {
				            BackgroundView = null, 
							AutoresizingMask = UIViewAutoresizing.FlexibleWidth | UIViewAutoresizing.FlexibleHeight, 
							Source = new DataSource(this)
			            };
		}

		public override bool ShouldAutorotateToInterfaceOrientation(UIInterfaceOrientation toInterfaceOrientation)
		{
			return true;
		}
		#endregion
		
		#region Helpers		
		private UITableViewCell CreateCell(string id)
		{
			var cell = new UITableViewCell(UITableViewCellStyle.Default, id)
			           {
				           Frame = new RectangleF(0, 0, DefaultCellWidth, DefaultCellHeight),
				           BackgroundColor = UIColor.White,
				           SelectionStyle = UITableViewCellSelectionStyle.None
			           };
			return cell;
		}
		
		private UILabel CreateTitleLabelControl(string title)
		{
			var label = new UILabel(new RectangleF(60, 15, 400, 20))
			            {
				            AutoresizingMask = UIViewAutoresizing.FlexibleRightMargin, 
							BackgroundColor = UIColor.Clear, 
							Text = title, 
							Font = UIFont.BoldSystemFontOfSize(18.0f)
			            };
			return label;
		}
		
		private UILabel CreateValueLabelControl(string title)
		{
			const int width = 250;
			var label = new UILabel(new RectangleF(DefaultCellWidth - width - 60, 15, width, 20))
			            {
				            AutoresizingMask = UIViewAutoresizing.FlexibleLeftMargin, 
							BackgroundColor = UIColor.Clear, 
							TextAlignment = UITextAlignment.Right, 
							Text = title
			            };
			return label;
		}
		
		private UISegmentedControl CreateSegmentControl(string[] values, int width)
		{
			var seg = new UISegmentedControl(new RectangleF(DefaultCellWidth - width - 60, 10, width, 30))
			          {
				          AutoresizingMask = UIViewAutoresizing.FlexibleLeftMargin
			          };
			for (int i = 0; i < values.Length; i++) {
				seg.InsertSegment(values [i], i, false);
			}
			return seg;
		}
		
		private UISwitch CreateSwitchControl(string[] values)
		{
			const int width = 90;
			var ctrl = new UISwitch(new RectangleF(DefaultCellWidth - width - 45, 10, width, 30))
			           {
				           AutoresizingMask = UIViewAutoresizing.FlexibleLeftMargin
			           };
			return ctrl;
		}
		
		private UISlider CreateSliderControl(int minValue, int maxValue)
		{
			const int width = 200;
			var slider = new UISlider(new RectangleF(DefaultCellWidth - width - 55, 10, width, 30))
			             {
				             AutoresizingMask = UIViewAutoresizing.FlexibleLeftMargin, 
							 MinValue = minValue, 
							 MaxValue = maxValue
			             };
			return slider;
		}
		#endregion

		#region Creating cells
		private UITableViewCell _CreatePageTransitionStyleCell()
		{
			var cell = CreateCell("PageTransitionStyleCell");
			var label = CreateTitleLabelControl("Transition style".t());
			var seg = CreateSegmentControl(new[] { "Curl".t(), "Scroll".t() }, 300);
			seg.SelectedSegment = (int)MgrAccessor.OptionsMgr.Options.PageTransitionStyle;
			seg.ValueChanged += delegate {
				MgrAccessor.OptionsMgr.Options.PageTransitionStyle = (UIPageViewControllerTransitionStyle)seg.SelectedSegment;
				MgrAccessor.OptionsMgr.Save();
			};
			cell.AddSubview(label);
			cell.AddSubview(seg);
			return cell;
		}

		private UITableViewCell _CreatePageNavigationOrientationCell()
		{
			var cell = CreateCell("PageNavigationOrientationCell");
			var label = CreateTitleLabelControl("Navigation orientation".t());
			var seg = CreateSegmentControl(new[] { "Horizontal".t(), "Vertical".t() }, 300);
			seg.SelectedSegment = (int)MgrAccessor.OptionsMgr.Options.PageTransitionStyle;
			seg.ValueChanged += delegate {
				MgrAccessor.OptionsMgr.Options.PageNavigationOrientation = (UIPageViewControllerNavigationOrientation)seg.SelectedSegment;
				MgrAccessor.OptionsMgr.Save();
			};
			cell.AddSubview(label);
			cell.AddSubview(seg);
			return cell;
		}
		
		private UITableViewCell _CreateToolbarVisibilityCell()
		{
			var cell = CreateCell("ToolbarVisibilityCell");
			var label = CreateTitleLabelControl("Toolbar".t());
			var switchCtrl = CreateSwitchControl(new[] { "Yes".t(), "No".t() });
			switchCtrl.SetState(MgrAccessor.OptionsMgr.Options.ToolbarVisible, false);
			switchCtrl.ValueChanged += delegate {
				MgrAccessor.OptionsMgr.Options.ToolbarVisible = switchCtrl.On;
				MgrAccessor.OptionsMgr.Save();
			};
			cell.AddSubview(label);
			cell.AddSubview(switchCtrl);
			return cell;
		}
		
		private UITableViewCell _CreateSliderVisibilityCell()
		{
			var cell = CreateCell("SliderVisibilityCell");
			var label = CreateTitleLabelControl("Slider".t());
			var switchCtrl = CreateSwitchControl(new[] { "Yes".t(), "No".t() });
			switchCtrl.SetState(MgrAccessor.OptionsMgr.Options.SliderVisible, false);
			switchCtrl.ValueChanged += delegate {
				MgrAccessor.OptionsMgr.Options.SliderVisible = switchCtrl.On;
				MgrAccessor.OptionsMgr.Save();
			};
			cell.AddSubview(label);
			cell.AddSubview(switchCtrl);
			return cell;
		}
		
		private UITableViewCell _CreatePageNumberVisibilityCell()
		{
			var cell = CreateCell("PageNumberVisibilityCell");
			var label = CreateTitleLabelControl("Page number".t());
			var switchCtrl = CreateSwitchControl(new[] { "Yes".t(), "No".t() });
			switchCtrl.SetState(MgrAccessor.OptionsMgr.Options.PageNumberVisible, false);
			switchCtrl.ValueChanged += delegate {
				MgrAccessor.OptionsMgr.Options.PageNumberVisible = switchCtrl.On;
				MgrAccessor.OptionsMgr.Save();
			};
			cell.AddSubview(label);
			cell.AddSubview(switchCtrl);
			return cell;
		}

		private UITableViewCell _CreateAutoScaleModeCell()
		{
			var cell = CreateCell("AutoScaleModelCell");
			var label = CreateTitleLabelControl("Auto scale mode".t());
			var seg = CreateSegmentControl(new[] { "Auto width".t(), "Auto height".t() }, 250);
			seg.SelectedSegment = (int)MgrAccessor.OptionsMgr.Options.AutoScaleMode;
			seg.ValueChanged += delegate {
				MgrAccessor.OptionsMgr.Options.AutoScaleMode = (AutoScaleModes)seg.SelectedSegment;
				MgrAccessor.OptionsMgr.Save();
			};
			cell.AddSubview(label);
			cell.AddSubview(seg);
			return cell;
		}

		private UITableViewCell _CreateZoomScaleLevelsCell()
		{
			var cell = CreateCell("ZoomScaleLevelsCell");
			var label = CreateTitleLabelControl("Zoom scale levels".t());
			var slider = CreateSliderControl(Options.MinZoomScaleLevels, Options.MaxZoomScaleLevels);
			slider.SetValue(MgrAccessor.OptionsMgr.Options.ZoomScaleLevels, false);
			slider.ValueChanged += delegate {
				MgrAccessor.OptionsMgr.Options.ZoomScaleLevels = (int)slider.Value;
				MgrAccessor.OptionsMgr.Save();
			};
			cell.AddSubview(label);
			cell.AddSubview(slider);
			return cell;
		}
		
		private UITableViewCell _CreatemZoomByDoubleTouchCell()
		{
			var cell = CreateCell("ZoomByDoubleTouchCell");
			var label = CreateTitleLabelControl("Scale by double click".t());
			var switchCtrl = CreateSwitchControl(new[] { "Yes".t(), "No".t() });
			switchCtrl.SetState(MgrAccessor.OptionsMgr.Options.AllowZoomByDoubleTouch, false);
			switchCtrl.ValueChanged += delegate {
				MgrAccessor.OptionsMgr.Options.AllowZoomByDoubleTouch = switchCtrl.On;
				MgrAccessor.OptionsMgr.Save();
			};
			cell.AddSubview(label);
			cell.AddSubview(switchCtrl);
			return cell;
		}
		
		private UITableViewCell _CreateLibraryReleaseDateCell()
		{
			var cell = CreateCell("LibraryReleaseDateCell");
			var label = CreateTitleLabelControl("Release date".t());
			var labelInfo = CreateValueLabelControl(MgrAccessor.OptionsMgr.Options.LibraryReleaseDate.ToShortDateString());			
			cell.AddSubview(label);
			cell.AddSubview(labelInfo);
			return cell;
		}
		
		private UITableViewCell _CreateLibraryVersionCell()
		{
			var cell = CreateCell("LibraryVersionCell");
			var label = CreateTitleLabelControl("Version".t());
			var labelInfo = CreateValueLabelControl(MgrAccessor.OptionsMgr.Options.LibraryVersion);
			cell.AddSubview(label);
			cell.AddSubview(labelInfo);
			return cell;
		}		
		#endregion
		
		protected class DataSource : UITableViewSource
		{
			private const int SectionsCount = 4;
			private readonly int[] _RowsInSections = new[] { 2, 3, 3, 2 };
			private readonly string[] _SectionTitles = new[] { "Transition style".t(), "Visibility".t(), "Scale".t(), "Library information".t() };		
			private readonly OptionsTableViewController _Controller;

			public DataSource(OptionsTableViewController controller)
			{
				_Controller = controller;
			}
		
			public override int NumberOfSections(UITableView tableView)
			{
				return SectionsCount;
			}
			
			public override int RowsInSection(UITableView tableview, int section)
			{
				return _RowsInSections[section];
			}

			public override string TitleForHeader(UITableView tableView, int section)
			{
				return _SectionTitles[section];
			}

			public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
			{
				switch (indexPath.Section) {
					case 0:
						switch (indexPath.Row) {
							case 0:
								return _Controller._PageTransitionStyleCell;
							case 1:
								return _Controller._PageNavigationOrientationCell;
						}
						break;
					case 1:
						switch (indexPath.Row) {
							case 0:
								return _Controller._ToolbarVisibilityCell;
							case 1:
								return _Controller._SliderVisibility;
							case 2:
								return _Controller._PageNumberVisibilityCell;
						}
						break;
					case 2:
						switch (indexPath.Row) {
							case 0:
								return _Controller._AutoScaleMode;
							case 1:
								return _Controller._ZoomScaleLevelsCell;
							case 2:
								return _Controller._ZoomByDoubleTouchCell;
						}
						break;
					case 3:
						switch (indexPath.Row) {
							case 0:
								return _Controller._LibraryReleaseDateCell;
							case 1:
								return _Controller._LibraryVersionCell;
						}
						break;
				}
				return null;
			}
		}
	}
}

