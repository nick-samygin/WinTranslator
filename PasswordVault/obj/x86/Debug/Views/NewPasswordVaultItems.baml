   M S B A M L   `   `   `  ����H  DPasswordVault, Version=1.2.1.0, Culture=neutral, PublicKeyToken=null7 clr-namespace:PasswordBoss.ViewsPasswordBoss.Views  5      @ <theme, Version=1.2.1.0, Culture=neutral, PublicKeyToken=nullL2clr-namespace:QuickZip.UserControls;assembly=themeQuickZip.UserControls 5	      J1clr-namespace:PasswordBoss.Helpers;assembly=themePasswordBoss.Helpers 5
      N3clr-namespace:PasswordBoss.Behaviors;assembly=themePasswordBoss.Behaviors 5      L HSecureItemsCommon, Version=1.2.1.0, Culture=neutral, PublicKeyToken=null`Bclr-namespace:SecureItemsCommon.Helpers;assembly=SecureItemsCommonSecureItemsCommon.Helpers 5      H0clr-namespace:QuickZip.Converters;assembly=themeQuickZip.Converters 5      .    (PasswordBoss.Views.NewPasswordVaultItems   5      R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      2local clr-namespace:PasswordBoss.Views     5      a ]Telerik.Windows.Data, Version=2016.1.217.40, Culture=neutral, PublicKeyToken=5803cfa389c90ce7e aTelerik.Windows.Controls, Version=2016.1.217.40, Culture=neutral, PublicKeyToken=5803cfa389c90ce7p	 lTelerik.Windows.Controls.Navigation, Version=2016.1.217.40, Culture=neutral, PublicKeyToken=5803cfa389c90ce7Ctelerik1http://schemas.telerik.com/2008/xaml/presentation   	 5      c
 _System.Windows.Interactivity, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35Bi:http://schemas.microsoft.com/expression/2010/interactivity 
 5      ;uc2clr-namespace:QuickZip.UserControls;assembly=theme 
 5	      ?helpers1clr-namespace:PasswordBoss.Helpers;assembly=theme 
 5
      C	behaviors3clr-namespace:PasswordBoss.Behaviors;assembly=theme 
 5      [secureItemsHelpersBclr-namespace:SecureItemsCommon.Helpers;assembly=SecureItemsCommon 
 5      A
converters0clr-namespace:QuickZip.Converters;assembly=theme 
 5      -   5          >�Name$  
LayoutRoot��5       � 	Resources 5         BoolToVisibility  ListOfSecureItems �� Orientation.  (Telerik.Windows.DragDrop.DragDropManager   AllowCapturedDrag �� RelativeSource �� AncestorType. 	 (Telerik.Windows.Controls.RadTreeViewItem  NodeLevelToMargine �� 	Converter   LightGrayTextForegroundColor ��Name  SecureItemsListBoxStyle4  .PasswordBoss.Behaviors.ListBoxDragDropBehavior   	IsEnabled(  "PasswordBoss.Helpers.ListBoxHelper	   SelectedItems
 �� Mode
 � Tag* 	 $Telerik.Windows.Controls.RadTreeView- 	 'Telerik.Windows.Controls.RadContextMenu   ContextMenu  Name  RadContextMenuStyle  RadMenuItemStyle �� Path  Key �� Padding � RowDefinitions J� Opacity ��Name  InvertedBooleanToVisibility 	 SettingsGreenColor $
  PasswordBossBlackBackgroundColor  ProximaRegular   SettingsLightGrayBorderColor �� CornerRadius  HierarchicalTemplate �� ItemsSource  StringToUppercase8  2SecureItemsCommon.Helpers.TreeDataTemplateSelector  FolderTemplateSelector   FolderTemplate   SecureItemsListTemplate��%�
  &	        &	      7  7   7  7   7  7   &	 �    7  &	 +
    7  7  �� 5   
   6'   �� 5   
   .R�5      �� $ Vertical=�6    .  ��6c   ��  �� "  8   .+�5      �� #���  6   $��1��6R   $��0 0 0 5q�6]   $��Stretch=�6n   �� 5      $ secureItemsList��6   #���  5       . ��5   3   �� *
SecureList+	 5      �� *SelectedItems+$
 TwoWay=� 5      �� *DataContext.Actions+ �� "   5        5      $ radContextMenu��61   #���  5!   )   #���  5"   )   ��5    )   �� $ PlacementTarget.Tag� �� *Self+5"   )    5$      ���� 5%      DraggedItemTemplate .R�5&      �� 
������ .+�5'   "   �� ���	#3F008769$��1q�6A   ���	#3F00A785$
��Left=�6m   $ 10q�6�   ._�5(   &   � $
��Left=�6+   .+�5)   *   � $
��Left=�60   y�6K   �� *HasOne+8   5*   .   �� 5+   2   $
��auto �6@   �� 5,   2   $
��auto �6@   .+�5.   .   � $
 0.75Q�64   $��60��6C   $��30��6N   ��6Z   �� *" CurrentDraggedItem[0].Background+.+�5/   2   �� $ dataImg��68   $
��Fill=�50   T   y�5/   J   �� *&$CurrentDraggedItem[0].IsDefaultImage+8  ��50   %   �� *CurrentDraggedItem[0].Image+51   3   �� 52   2   $��25��6:   $��Center=�6I   $��Uniform=�53   T   y�52   d   �� *&$CurrentDraggedItem[0].IsDefaultImage+8  ��53   %   �� *CurrentDraggedItem[0].Image+54   3   55   /   56   +   � 57   *   $
��Left=�6/   $
 0.75Q�6J   $��60��6Y   $��30��6d   #��� 	 6q   y�6�   �� *HasOne+8  .+�58   .   �� $��Center=�6b   $��Center=�6   #��� 
 6�   $��15(�59   *   $
��Bold"�68   #���  5:   )   ��58   8   �� *CurrentDraggedItem.Count+5:   )   5;   +   5<   '   5=   #   �� 5?   "   #���  6)   $��1q�6f   $��0 2 0 0q�6z   $ 1��6�   
�������y�6�   �� *IsValidDrop+8  ._�5@   &   �� $ 
Horizontal=�61   .+�5A   *   �� $��Move To ��64   #��� 
 6F   $��11(�5B      $��Normal"�6,   $��5 5 0 5q�6A   #���  5C      �� 5D   *   #���  6h   $��11(�5E      $��Normal"�6,   $
��Wrap=�6B   $��0 5 5 5q�6V   #���  5F      ��5D   4   �� *#!CurrentDraggedOverItem.FolderName+5F      5G   '   5H   #   5I      5J      5K      5L      5M      5O      �� 5Q   
    5R      �� *	ChildList+.R�5S      �� $ 
Horizontal=�6   .+�5T      �� $��12(�6   $
��Bold"�6*   ��6=   �� *
FolderName+8   �� 5U      $��12(�6   $
��Bold"�6*   $�� (��6=   �� 5V      $��12(�6   $
��Bold"�6*   ��6=   �� *Count+�� 5W      $��12(�6   $
��Bold"�6*   $��)��6=   5X      5Z        5\   
   8   8  6�   5^      .��5_      � $��Stretch=�6    5`   
   �� 5a      $
��Auto �6   �� 5b      .+�5d   
   �� -   5         ��Name$ RecommendedHeaderText��5d      $��0��63     RecommendedItemsHeader#���  6@   $��18(�6p   $��	15,15,0,0q�6~   y�5e      �� *$"AddRecommendedItemHeaderVisibility+# [  $
 TwoWay=� �� UpdateSourceTrigger$ PropertyChanged=��� 5f   
   ��. $��1��6/   .��5g        -   5          Name$ treeView��5g   &     CategoryItemStyle#���  6d   #��[ 5h       J� 	AllowDrop . $��Disabled=�6i   $��5q�5i         IsDropPreviewLineEnabled .  ��5g   8   �� *HierarchyFolderList+5i   $   5j      5k      5l      