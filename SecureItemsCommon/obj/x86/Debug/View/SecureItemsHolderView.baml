   M S B A M L   `   `   `  ����L  HSecureItemsCommon, Version=1.2.1.0, Culture=neutral, PublicKeyToken=null?$clr-namespace:SecureItemsCommon.ViewSecureItemsCommon.View  5      @ <theme, Version=1.2.1.0, Culture=neutral, PublicKeyToken=nullJ1clr-namespace:PasswordBoss.Helpers;assembly=themePasswordBoss.Helpers 5      N3clr-namespace:PasswordBoss.Behaviors;assembly=themePasswordBoss.Behaviors 5	      E'clr-namespace:SecureItemsCommon.HelpersSecureItemsCommon.Helpers  5
      Q-clr-namespace:PasswordBoss.Views.UserControlsPasswordBoss.Views.UserControls  5      H0clr-namespace:QuickZip.Converters;assembly=themeQuickZip.Converters 5      N3clr-namespace:PasswordBoss.Behaviors;assembly=themePasswordBoss.Behaviors 5      5clr-namespace:SecureItemsCommonSecureItemsCommon  5      2    ,SecureItemsCommon.View.SecureItemsHolderView   5      R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      6local$clr-namespace:SecureItemsCommon.View     5      a ]Telerik.Windows.Data, Version=2016.1.217.40, Culture=neutral, PublicKeyToken=5803cfa389c90ce7k gTelerik.Windows.Controls.Input, Version=2016.1.217.40, Culture=neutral, PublicKeyToken=5803cfa389c90ce7e aTelerik.Windows.Controls, Version=2016.1.217.40, Culture=neutral, PublicKeyToken=5803cfa389c90ce7n	 jTelerik.Windows.Controls.GridView, Version=2016.1.217.40, Culture=neutral, PublicKeyToken=5803cfa389c90ce7p
 lTelerik.Windows.Controls.Navigation, Version=2016.1.217.40, Culture=neutral, PublicKeyToken=5803cfa389c90ce7Gtelerik1http://schemas.telerik.com/2008/xaml/presentation    	 
 5      Ghelpers1clr-namespace:PasswordBoss.Helpers;assembly=theme    	 
 5      K	behaviors3clr-namespace:PasswordBoss.Behaviors;assembly=theme    	 
 5	      HsecureItemsHelpers'clr-namespace:SecureItemsCommon.Helpers    	 
 5
      @view-clr-namespace:PasswordBoss.Views.UserControls    	 
 5      I
converters0clr-namespace:QuickZip.Converters;assembly=theme    	 
 5      Jbehavior3clr-namespace:PasswordBoss.Behaviors;assembly=theme    	 
 5      7	viewmodelclr-namespace:SecureItemsCommon    	 
 5        � 	Resources  5      .  (PasswordBoss.Behaviors.PropertyConverter   PropertyConverter  SubMenuItemStyle �� 
TargetType* 
 $Telerik.Windows.Controls.RadMenuItem �� Property ! RadSubMenuItemControlTemplate �� Value   LightGrayTextForegroundColor �� BasedOn �� Path �� RelativeSource �� AncestorType. 
 (Telerik.Windows.Controls.RadTreeViewItem `� 	Converter	 �� Mode
 �� Triggers P� Property P� Value  SettingsGreenColor5   /SecureItemsCommon.Helpers.MenuItemStyleSelector  MenuItemStyleSelector   SubItemStyle  BoolToVisibility5   /SecureItemsCommon.Helpers.BindingCheckConverter  BindingCheck4  .QuickZip.Converters.StringToUppercaseConverter  StringToUppercase5  /QuickZip.Converters.NodeLevelToMargineConverter 	 NodeLevelToMargineC  =Telerik.Windows.Controls.InvertedBooleanToVisibilityConverter 
 InvertedBooleanToVisibility  ListOfSecureItems �� Orientation �� 	Converter ��Name  SecureItemsListBoxStyle4	  .PasswordBoss.Behaviors.ListBoxDragDropBehavior 	  	IsEnabled(
  "PasswordBoss.Helpers.ListBoxHelper 
  SelectedItems �� Mode  HierarchicalTemplate �� ItemsSource
 � Tag* 
 $Telerik.Windows.Controls.RadTreeView8   2SecureItemsCommon.Helpers.TreeDataTemplateSelector  FolderTemplateSelector   FolderTemplate   SecureItemsListTemplate��%/
  &	        &	      7   &	     7  &	     &	 2    &	 E    &		 X    &	
 k    &	 ~    7 	 &	 �    7  &	 �	    7  7    5   
   6&   �� 5   
   "  6*   .�5      �� ! �@ TopLevelHeaderTemplateKey# �  6:   �� 5       �@ TopLevelItemTemplateKey# �  68   �� 5        �@ SubmenuHeaderTemplateKey# �  69   �� 5       �@ SubmenuItemTemplateKey# �  67   �� 5       � ��$ 12(�6)   �� 5       � ��# �  6+    5      �� 5      "  6   .�5      ��  � ��
 ���� 63   �� 5       � �� �	#FF84848464   �� 5       � ��$ 0q�68   �� 5        � ��$
 Left=�6C   �� 5!       � ��$ Center=�6A   �� 5"       �@J�SnapsToDevicePixels . 6<   �� 5#       �@�FocusVisualStyle 69   Z� �� 5$       � y� 63   �� *
Visibility+�� 5%       �@ Icon 6-   �� *Icon+�� 5&       �@�Tag 6,   �� $ DataContext� �� "  �� 5'       �@ IconTemplate 5(      �� 5)      .R�5*   "   �� $��16��6;   $��Uniform=�6G   $��Center=�6Y   $��Center=�6v   ��6(   �� 6v   5+      5-      �� 5.       � �� 6/   �� *Name+�� 5/       � �� 64   �� *
SubItems+�� 50       �@ Command 60   �� *Action+�� 51       �@ CommandParameter 52      `� 53      8   .1�54   "   �� $ DataContext�6*    6=   �� "  �� 55   "   $! DataContext.ActionParameter�6*    6M   �� $
	 Self=�56      58      59      
 5;      P� 5<       �@J�IsMouseOver . . �5=      ��  � ��# �  63   5>      5@        5B   
   8   6Q   �� 5D   
   6'     5E   
   63     5F   
   60     5G   
   61     5H   
   67   �� 5K   
   .R�5L      �� $ Vertical=�6   ��5M      ��  �� "  8   .+�5O      �� #���  6   $��1��5P      $��0 0 0 5q�5Q      $��Stretch=�5R      �� 5T      $ secureItemsList��6   #���  5V       . ��5U      �� *
SecureList+ 5X      �� *SelectedItems+$ TwoWay=�5Z      5[      5]      �� 5_   
    5`      �� *	ChildList+.R�5a      �� $ 
Horizontal=�6    5b      �� *DataContext.FolderActions+ �� "  .+�5c      �� $��12(�6   $
��Bold"�6*   ��6=   �� *
FolderName+8   �� 5d      $��12(�6   $
��Bold"�6*   $�� (��6=   �� 5e      $��12(�6   $
��Bold"�6*   ��6=   �� *Count+�� 5f      $��12(�6   $
��Bold"�6*   $��)��6=   5g      5h        5j   
   8   8  6�   5l      .��5m      � .+�5n   
   �� ��. $��1��6/   .��5p        -   5          Name$ treeView��5p   "   #��[ 5q       J� 	AllowDrop . $��Disabled=�6i   $��5q�5r         IsDropPreviewLineEnabled .  ��5p   4   �� *HierarchyFolderList+��5s      �� 5t      "  6   .�5u      ��  �@�FocusVisualStyle 5v      �� 5w   "   .�5x   &   ��  � �� 5y   *   �� 5z   .   .R�5{   2   �� $��0q�6<   
������ $��0��6\   5|   /   5~   '   5   #   5�      �� 5�      .  (Telerik.Windows.DragDrop.DragDropManager �@ AllowCapturedDrag .  6V   �� 5�       �@ 	AllowDrag .  6N   �� 5�       � ��$ Stretch=�6G   �� 5�       � ��$ Center=�6E   �� 5�       � ��$ 1q�6<   �� 5�       � ��$ 1,4,5,4q�64   �� 5�       � ��$ 0q�63   �� 5�       �@ IsDropAllowed . 6:   �� 5�       � {� . 66   �� 5�       �@ 
IsExpanded 67   �� $ 
IsExpanded�$ TwoWay=��� 5�       � �� .  66   �� 5�      -  'System.Windows.Input.KeyboardNavigation �@ TabNavigation$ Local=�6M   �� 5�       � �� 5�      �� 5�   "    �� 
TargetType"  62   .R�5�   &   �  �Name$ RootElement��6+    � UseLayoutRounding .  � RowDefinitions 5�   *   �� 5�   .   $
��Auto �6<   �� 5�   .   '  !System.Windows.VisualStateManager   VisualStateGroups 5�   *   %  System.Windows.VisualStateGroup  5�   .   
EditStates��    States.  5�   2      System.Windows.VisualState  Display��6>     5�   2   Edit��!   
Storyboard.! 5�   6   �� .��5�   :   T� " g� Duration$" 0D�6X   # �� TargetProperty$# 
Visibility�6e   $ �� 
TargetName$$ EditHeaderElement��6�   ..�5�   >   i� % R� KeyTime$% 0��6U   & R� Value& 5�   B     System.Windows.Visibility 5�   F   Visible=�6Z   5�   ?   5�   ;   T� 5�   :   $" 0D�6X   $# 
Visibility�6e   $$ Header��6�   ..�5�   >   i� $% 0��6U   & 5�   B    5�   F   	Collapsed=�6\   5�   ?   5�   ;   5�   7   5�   3   5�   /     5�   .   CommonStates��.  5�   2     
Normal��6>     5�   2   Disabled��.! 5�   6   �� .��5�   :   W� $" 0D�6J   	' W� To$
' 0.35X�6W   $# Opacity�6a   $$ Header��6�   W� 5�   :   $" 0D�6J   $
' 0.35X�6W   $# Opacity�6a   $$ Image��6�   5�   7   5�   3     5�   2   	MouseOver��.! 5�   6   �� .��5�   :   T� ( g� 	BeginTime$( 0X�6X   $" 	0:0:0.001D�6f   $# 
Foreground�6{   $$ Header��6�   ..�5�   >   i� $% 0��6U   & 5�   B   �� 5�   F   ) �� Color$) #23B194��6V   5�   ?   5�   ;   T� 5�   :   $( 0X�6X   $" 	0:0:0.001D�6f   $# 
Foreground�6{   $$ Expander��6�   ..�5�   >   i� $% 0��6U   & 5�   B   �� 5�   F   $) #23B194��6V   5�   ?   5�   ;   5�   7   5�   3   5�   /     5�   .   SelectionStates��.  5�   2     
Unselected��6>     5�   2   Selected��6>     5�   2   SelectedUnfocused��6>   5�   /     5�   .   LoadingOnDemandStates��.  5�   2     LoadingOnDemand��.! 5�   6   �� .��5�   :   T� $" 0D�6X   $# 
Visibility�6e   $$ LoadingVisual��6�   ..�5�   >   i� $% 0��6U   & 5�   B    5�   F   Visible=�6Z   5�   ?   5�   ;   T� 5�   :   $" 0D�6X   $# 
Visibility�6e   $$ Expander��6�   ..�5�   >   i� $% 0��6U   & 5�   B    5�   F   	Collapsed=�6\   5�   ?   5�   ;   W� 5�   :   $" 0:0:1D�6J   * W� From$* 0X�6[   + g� RepeatBehavior$+ Forever��6d   $	' 359X�6}   $# Angle�6�   $!$ LoadingVisualAngleTransform��6�   5�   7   5�   3     5�   2   LoadingOnDemandReverse��6>   5�   /     5�   .   FocusStates��.  5�   2     Focused��6>     5�   2   	Unfocused��6>   5�   /     5�   .   ExpandStates��.  5�   2     Expanded��.! 5�   6   �� .��5�   :   T� $" 0D�6X   $# 
Visibility�6e   $$ ItemsHostBorder��6�   ..�5�   >   i� $% 0��6U   & 5�   B    5�   F   Visible=�6Z   5�   ?   5�   ;   5�   7   5�   3     5�   2   	Collapsed��6>   5�   /   .+�5�   *   � $ 	HeaderRow��6/   $��0q�6B   #��z��6N   , J� SnapsToDevicePixels, . - � ColumnDefinitions- 5�   .   �� 5�   2   $
��auto �6C   �� 5�   2   $
��Auto �6C   �� 5�   2   $
��Auto �6C   �� 5�   2   $
��Auto �6C   �� 5�   2   $
��Auto �6C   �� 5�   2   $��* �6C   .+�5�   .   �� #��z��65   #��z��6a   #��z��6�   $��6��6�   $��2��6�   . �� CornerRadius$. 2��6�   �� 5�   .   / ��Name$/ MouseOverVisual��65   ���	#FFFFC92B$��1q�6f   $��6��6z   $��2��6�   $. 1��6�   0 J� Opacity$0 0Q�6�   ��5   2   �� 5  6   1 �� EndPoint$1 0.5,1&�6J   2 �� 
StartPoint$2 0.5,0&�6[   .��5  :   � 3 � Color$3 	#FFFFFBA3��6G   4 � Offset$4 1Q�6Y   � 5  :   $3 	#FFFFFBDA��6G   $4 0Q�6Y   5  7   ._�5  2   �� 
�������$��1q�6M   $. 0��6w   ��6a   Z� 6w   5  /   �� 5  .   $/ SelectionUnfocusedVisual��65   ���	#FFDBDBDB$��1q�6o   $��6��6�   $��2��6�   $. 1��6�   $y�	Collapsed=�6�   ��5	  2   �� 5
  6   $	1 0,1&�6J   .��5  :   � $3 	#FFF8F6F9��6G   $4 0Q�6Y   � 5  :   $3 	#FFF0F0F0��6G   $4 1Q�6Y   5  7   ._�5  2   �� 
������ $��1q�6S   $. 0��6}   ��6g   Z� 6}   5  /   �� 5  .   $/ SelectionVisual��65   ���	#FFFFC92B$��1q�6f   $��6��6z   $��2��6�   $. 1��6�   $y�	Collapsed=�6�   ��5  2   �� 5  6   $1 0.5,1&�6J   $2 0.5,0&�6[   .��5  :   � $3 	#FFFCE79F��6G   $4 1Q�6Y   � 5  :   $3 	#FFFDD3A8��6G   5  7   ._�5  2   �� 
�������$��1q�6M   $. 0��6w   ��6a   Z� 6w   5  /   �� 5  .   5 ��Name$5 IndentContainer��69   $ 
Horizontal=�6R   .+�5  2   �� 6 ��Name$6 IndentFirstVerticalLine��6<   ���	#FFCCCCCC$y�	Collapsed=�6p   $	��Top=�6�   5  /   � 5  .   $ ListRootContainer��63   $��1��6P   $��Center=�6`   $��21��6}   .+�5  2   �� $6 HorizontalLine��6<   $��Right=�6T   $��1��6p   ���	#FFCCCCCC$��Center=�6�   �� 5  2   $6 VerticalLine��6<   $��Center=�6R   ���	#FFCCCCCC$	��Top=�6�   $��1��6�   d� 5   2   7 d�Name$7 Expander��6?    - )SimpleExpanderToggleButtonControlTemplate#���  6Q   #��z��6�   ��.  6�   � 5!  2   $ LoadingVisual��67   $��Center=�6N   8 J� RenderTransformOrigin$8 0.5,0.5&�6k   $y�	Collapsed=�6�   $��Center=�6�   z�5"  6   V� 5#  :   .~�5$  >   �� 9 �� Angle$9 0Q�6N   : �� CenterY$	: 0.5Q�6X   ; �� CenterX$	; 0.5Q�6f   5%  ;   .+�5'  6   F� !����@B     �    ������J]	$��10��6W   < �� StrokeStartLineCap$< Round=�6c   = �� Stretch$
= Fill=�6~   #��z��6�   $��1��6�   $��10��6�   F� 5(  6   "����     7��Ѡ� �����    `D��	#��z��6X   $
��Left=�6|   $��4��6�   $��
5,-1.5,0,0q�6�   $
= Fill=�6�   $��1��6�   $	��Top=�6�   $��4��6�   5)  3   5*  /   �� 5+  .   > ��Name$> Image��64   $��3��6C   $��Center=�6S   $��16��6p   $��16��6~   $��2q�6�   $��Center=�6�   �� 5,  .   $6 FocusVisual��68   $��6��6M   $��2��6a   ? J� IsHitTestVisible? .  @ �� RadiusY$@ 3��6�   A �� RadiusX$A 3��6�   
���   �$��1��6�   B �� StrokeDashArray$	B 1 2R�6�   $y�	Collapsed=�6�   � 5.  .   $ HeaderContentPanel��63   
������ $��2��6h   $��4��6|   .+�5/  2   �� C ��Name$C Header��6A   #��z��6R   #��z��6�   #��z��6�   #��z��6K  ��.  #��z��6�  #��z��6�  ��6�   �� *
Foreground+$ OneWay=� �� *TemplatedParent+6�  �� 50  2   D ��Name$D EditHeaderElement��6C   D 
 >Telerik.Windows.Controls.TreeView.EditableHeaderedItemsControlE   HeaderEditTemplate#��zE 6^   #��z��6�   #��z��6�   $y�	Collapsed=�6�   #��z��6  51  /   �� 52  .   F ��Name$F CheckBoxElement��67   $��2��6P   ��.  $��5,0,0,0q�6r   $y�	Collapsed=�6�   $��Center=�6�   +  %Telerik.Windows.Controls.StyleManagerG   ThemeG 53  2   0  *Telerik.Windows.Controls.Office_BlackTheme  54  6   56  /   	� 57  .   H 	�Name$H RadioButtonElement��6:   $��2��6V   ��.  $��5,0,0,0q�6x   $y�	Collapsed=�6�   $��Center=�6�   G 58  2     59  6   5;  /   5<  +   �� 5=  *   $/ ItemsHostBorder��61   $	��5 0q�6J   $��0q�6X   $��1��6n   $y�	Collapsed=�6{   $��Stretch=�6�   ._�5>  .   �� I ��Name$I 	ItemsHost��6=   $��1��6P   5?  +   5@  '   5A  #   5C     -   5        5F     5I     5J     A   ;PasswordBoss.Views.UserControls.CustomMessageBoxUserControl  5K  
   J   CustomMessageBoxType$J Confirmation=�5M      
 Cancel'K    MessageBoxDialogCancelButtonText#K �  5N      
 Delete(L   !MessageBoxDialogConfirmButtonText#L �  5O       DeleteSecureItemHeader!M   MessageBoxDialogHeaderText#M �  5P     !N   MessageBoxDialogVisibilityN 5L     �� *)'DeleteSelectedItemsMessageBoxVisibility+O   MessageBoxDialogTextO 5Q     �� * DeleteSelectedItemsMessageText+P �� UpdateSourceTrigger$P PropertyChanged=�%Q   MessageBoxDialogConfirmCommandQ 5R     �� *%#ConfirmedDeleteSelectedItemsCommand+$R   MessageBoxDialogCancelCommandR 5S     �� *$"CanceledDeleteSelectedItemsCommand+5U     5V     