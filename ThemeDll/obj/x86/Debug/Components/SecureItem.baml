   M S B A M L   `   `   `  ����@  <theme, Version=1.2.1.0, Culture=neutral, PublicKeyToken=null9!clr-namespace:QuickZip.ConvertersQuickZip.Converters  5      ;"clr-namespace:PasswordBoss.HelpersPasswordBoss.Helpers  5      ?$clr-namespace:PasswordBoss.BehaviorsPasswordBoss.Behaviors  5      W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089c _System.Windows.Interactivity, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35a ]Telerik.Windows.Data, Version=2016.1.217.40, Culture=neutral, PublicKeyToken=5803cfa389c90ce7e aTelerik.Windows.Controls, Version=2016.1.217.40, Culture=neutral, PublicKeyToken=5803cfa389c90ce7n jTelerik.Windows.Controls.GridView, Version=2016.1.217.40, Culture=neutral, PublicKeyToken=5803cfa389c90ce7p	 lTelerik.Windows.Controls.Navigation, Version=2016.1.217.40, Culture=neutral, PublicKeyToken=5803cfa389c90ce7k
 gTelerik.Windows.Controls.Input, Version=2016.1.217.40, Culture=neutral, PublicKeyToken=5803cfa389c90ce7   BoolToVisibility4    .QuickZip.Converters.StringToUppercaseConverter  StringToUppercase5   /QuickZip.Converters.NodeLevelToMargineConverter  NodeLevelToMargineC  =Telerik.Windows.Controls.InvertedBooleanToVisibilityConverter  InvertedBooleanToVisibility  ListSecureItemTemplate  ��Name �� Padding �� CornerRadius  
WhiteColor �� Property �� Value �� Triggers P� Property P� Value  PasswordBossGreenColor �Name	 � UseLayoutRounding
 � ContextMenu � ColumnDefinitions ��Name �� 	Converter0  *Telerik.Windows.Controls.RadDropDownButton  Name " RadDropDownButtonWithArowStyle �� BitmapScalingMode   IsOpen �� Mode �� 
TargetType  	menu_grey 	 
menu_green   DropDownContent 
 SecureItemsListBoxItemStyle* 	 $Telerik.Windows.Controls.RadMenuItem ��Name ��Name $  PasswordBossBlackBackgroundColor �� TextAlignment �� Padding  ProximaRegular ��Name �� TextWrapping �� TextAlignment  TextBoxTemplate   LightGrayTextForegroundColor  LastUsedAndroidSort  MenuLastModified �� StringFormat  Sharing � ToolTip  imgSharePeopleHover  star_green_21x20  Favorite 	 Notes  imgNotes � Cursor.  (System.Windows.Interactivity.Interaction   Triggers/  )System.Windows.Interactivity.EventTrigger   	EventName.  (System.Windows.Interactivity.TriggerBase    Actions6  0System.Windows.Interactivity.InvokeCommandAction!   Command"   CommandParameter  imgTrashMedium 
 Delete# J� InputBindings$ d� MouseAction% �� Command& �� Triggers' �� Value( �� Binding) �� ElementName* �� 
TargetName+ �� RelativeSource, �� AncestorType- �� Mode  TileSecureItemTemplate. � RowDefinitions/ J� Opacity0 �� Orientation  
menu_white  RadMenuItemStyle
1 � Tag  open_site_green  open_site_white.	 	 (Telerik.Windows.Controls.RadTreeViewItem % !SIWithContextMenuListBoxItemStyle2 �� 
TargetType3 J� SnapsToDevicePixels  SecureItemsListBoxStyle*
 	 $Telerik.Windows.Controls.RadTreeView   TreeViewWrapPanelTemplate ! TreeViewPanelTemplate " SecureItemsDragTemplate # SettingsGreenColor  $ SettingsLightGrayBorderColor�� 5      D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    :x,http://schemas.microsoft.com/winfx/2006/xaml     5      Bi:http://schemas.microsoft.com/expression/2010/interactivity  5      Gtelerik1http://schemas.telerik.com/2008/xaml/presentation    	 
 5      0
converters!clr-namespace:QuickZip.Converters  5      .helpers"clr-namespace:PasswordBoss.Helpers  5      2	behaviors$clr-namespace:PasswordBoss.Behaviors  5      %�2  &	        &	      &	 &     &	 9     &	 L     7  7   7  7   7   7   7   7   7   &	 �    7  7   7   7   &	 R&    &	 ^(    7  &	" �+    7   7  7   7  7   �� 5      6#      5	      6,     5
      6-     5      63   �� 5      .R�5   
   �� $  
mainBorder��6   $��0q�6%   $ 5 0 14 0q�60   $��51��6C   $ 2��5      #���  6"   $��Stretch=�6L   ��5      �� 5      .�5      ��  � ��$ 0q�6?   �� 5       � ��
 ���� 6;    5      P� 5       �@J�IsMouseOver . . �5      ��  � ��# �  6C   �� 5       � ��$ 1q�6G   5      5      ._�5      � $ gridlist��6   $��Stretch=�6=   $��Stretch=�6[   #���  6{   	 . 
 6%   Z�  5       �� 5!      $
��auto �6'   �� 5"      $
��auto �6'   �� 5#      $	��280��6'   $��* �66   �� 5$      $	��130 �6'   �� 5%      $��50 �6'   �� 5&      $��50 �6(   �� 5'      $��50 �6(   �� 5(      $��50 �6(   .+�5+      � $��0��69   $��0 0 12 0q�6J   $��Center=�6\   $��90��6y   $��45��6�   $��Center=�6�   ��6   �� *
Background+.+�5,      �� $ dataImg��6   $
��Fill=�5-   6   y�5,   .   �� *IsDefaultImage+8   ��5-      �� *Image+5.      �� 5/      $��35��6   $��90��6-   $��Uniform=�50   6   $��Center=�6H   y�5/   8   �� *IsDefaultImage+8  ��50      �� *Image+51      52        53      $ settingsButton��6,   #���  6D   $��1��6}   $��30��6�   $��30��6�   $
��12 0q�6�   $��Center=�6�   $��Center=�6�   $ HighQuality=�54      $y�Hidden=�6J   
������ $��0q�55       60   �� *ShowItemMenu+$ TwoWay=���56      �� 57      $��30��6    $��30��6,   $��Center=�6O   $ HighQuality=�6j   
 68   Z� ��58      �� 59   "   " ��6(   .�5:   &   ��  � ��# �  6?    5;   &   P� 5<   *    �@J�IsMouseOver . . �5=   .   ��  � ��# � 	 6G   5>   +   5@   #   5B       5D      �� 5E      #��� 
 6N   $��0q�6�   ��6"   �� *	Actions+��5F      �� 5G   "   .R�5H   &     $��Stretch=�6;   5I   #   5K      5M      �� 5P      $ LeftPartOfItemGrid��6   $��2��69   
������ $��Center=�6b   $��0 0 12 0q�6}   .+�5R      � $��3 0 0 0q�6   .+�5S      �� $ dataTb��6$   $
��Left=�64   $	��260��6O   #���  5T   !   $
��Wrap=�6a   $
 Left=�6u   $ 0,0,0,0q�5U   !   $��Bottom=�63   $
��11pt(�6N   $
��Bold"�6^   #���  5V   !   ��6O   �� *Name+�� 5W      $ 
editDataTb��6"   $
��Left=�66   $	��260��6Q   $��-4 0 0 0q�6]   #���  5X   !   $
 Wrap=�6a   $
 Left=�6u   #���  6�   $��2��6�   $��0q�5Y   !   $��Bottom=�6-   $
��11pt(�6H   $
��Bold"�6X   #���  5Z   !   $y�	Collapsed=�6e   ��6O   �� *Name+6e   5[      �� 5\      $ UsernameTextBlock��6    $
��Left=�6;   $
 Left=�6V   $��3 2 0 0q�6k   #���  5]      $��0��6Y   $��2��6i   $ 0,0,0,0q�5^      $	��Top=�6/   $
��10pt(�6G   $��Normal"�6W   $
��Wrap=�6m   #���  5_      ��6K   �� *ListViewSecondName+5a      � 5c      $ lastUsedGrid��6   $
��Left=�6.   $��3��6I   $��0 0 12 0q�6Y   
������ $y�	Collapsed=�6�   .+�5d      �� $��Center=�6!   y�6<   �� *ShowLastModifiedDate+8  .+�5f      �� $
��Left=�6$   #���  6�   $
��Wrap=�6�   $
 Left=�6�   $ 0,0,0,0q�5g   !   $��Bottom=�63   $
��10pt(�6N   $
��Thin"�6^   #���  5h   !   #���  6O   y�5f   ?   �� *	IsWebSite+8  5h   O   �� 5l      $
��Left=�6r   $
 Left=�6�   #���  5m      $��0��6Y   $��2��6i   $ 0,2,0,0q�5n      $	��Top=�6/   $
��10pt(�6G   $
��Wrap=�6W   $
��Thin"�6k   #���  5o      y�5l   %   �� *	IsWebSite+8  ��5o   K   �� *LastAcceessTime+5q      �� 5r      $��Center=�6!   y�6=   �� *ShowLastModifiedDate+8  .+�5t      �� $
��Left=�6$   #���  6?   $
��Wrap=�6   $
 Left=�6�   $ 0,0,0,0q�5u   !   $��Bottom=�63   $
��10pt(�6N   $
��Thin"�6^   #���  5v   !   #���  6O   �� 5x      $
��Left=�5y      $
 Left=�63   #���  5z      $��0��6Y   $��2��6i   $ 0,2,0,0q�5{      $	��Top=�6/   $
��10pt(�6G   $
��Wrap=�6W   $
��Thin"�6k   #���  5|      ��6K   �� *LastModifiedDate+$ {0:MMM yyyy}��5}      5~      �� 5�      $ listBoxShare��6c   $
��12 0q�6z   $��4��6�   $��22��6�   $��18��6�   $��Uniform=�6�   $��Center=�6�   $��Center=�6�   $ HighQuality=�6�   # �  6*  #���  6N  y�6   �� *Shared+8  6N  �� 5�      $ favoritesGridImage��6�   $��5��6f   $
��12 0q�6v   #���  6�   $��21��6�   $��20��6�   $��Center=�6�   $��Center=�6�   # �  6  y�6   �� *
Favorite+8  6  �� 5�      $ notesGridImage��6d   $
��12 0q�6|   $��6��6�   # �  6�   #���  6�   $��17��6�   $��20��6�   y�6   �� *	HasNote+8  6�   �� 5�      $  DeleteImageBorder��6   $y�Hidden=�64   $
��12 0q�6H   $��7��6V   
������ $
 Hand��6   ._�5�      �� $��26��6   $��26��6)   #���  64    5�        5�      $ MouseLeftButtonUp��6-   .  5�   "     ! 68   �� *DeleteItemCommand+" 6^   �� *Id+5�      ��5�      �� 5�      .�5�   "   ��  � ��$ 0q�6K    5�   "   P� 5�   &    �@J�IsMouseOver . . �5�   *   ��  � ��$ 1q�6S   5�   '   5�      ._�5�      �� $��Center=�6    $��Center=�6=   $��20��6X   $��20��6d   #���  6o   $ HighQuality=�6�   # �  6�   5�      5�      5�      # 5�      d� 5�      $$ LeftDoubleClicke�6   % 6=   �� *DoubleClickCommand+5�      & 5�   
   �� 5�      $
' True��6R   ( 6   �� *IsMouseOver+$) 
mainBorder��.a�5�      �� $* settingsButton��6/    � y�$ Visible=�6K   �� 5�      $* DeleteImageBorder��6/    � y�$ Visible=�6N   �� 5�      $* settingsButton��6/    � y�$ Visible=�6K   �� 5�      $* lastUsedGrid��6/    � y�$ Visible=�6I   5�      �� 5�      $
' True��6N   ( 6   �� *IsMouseOver+$) dataTb��.a�5�      �� $* 
editDataTb��6/    � y�$ Visible=�6G   �� 5�      $* dataTb��6/    � y�$ 	Collapsed=�6C   5�      �� 5�      $
' True��6R   ( 6   �� *IsMouseOver+$) 
editDataTb��.a�5�      �� $* dataTb��6/    � y�$ 	Collapsed=�6C   �� 5�      $* 
editDataTb��6/    � y�$ Visible=�6G   5�      �� 5�      $
' True��6�   ( 6   �� *
IsSelected++ �� ", ��$- FindAncestor=�.a�5�      �� $* 
mainBorder��6   # �  6H    � ���� 5�      $* 
mainBorder��6    � ��$ 1q�6L   5�      5�      �� 5�      .R�5�   
   �� $  
mainBorder��6   $	��142��6%   
������ $��1q�6K   $��	0,0,15,10q�6_   $��Stretch=�6r   $��Stretch=�6�   ._�5�      � $��Stretch=�6   $��Stretch=�62   . 5�      �� 5�      $��75 �6$   �� 5�      $��23 �6$   .+�5�      � #���  6   $��Stretch=�6A   $��Stretch=�6_   .+�5�      � $��0��6=   $��5q�6N   $��Stretch=�6Y   $��Stretch=�6w   ��6   �� *
Background+.+�5�      �� $ dataImg��6    $
��Fill=�5�   6   y�5�   2   �� *IsDefaultImage+8   ��5�      �� *Image+5�      �� 5�      $��50��6"   $��Center=�61   $��Uniform=�5�   6   y�5�   L   �� *IsDefaultImage+8  ��5�      �� *Image+5�      5�      �� 5�      $��1q�6   $
��Left=�6t   $	��Top=�6�   #���  5�      $��17��6J   $��16��6U   # �  6b   y�5�   '   �� *
Favorite+8  5�   b   �� 5�      $��1q�6   $��4��6)   $��20��6:   $��16��6E   $��Uniform=�6Q   $	��Top=�6c   $��Right=�6{   $ HighQuality=�6�   # �  6�   #���  6�   5�      �� 5�      $  MouseOverBorder��6   $��2��62   $y�	Collapsed=�6C   $
/ 0.45Q�6Z   #���  6i   �� 5�      $ txtName��6   $	��Top=�6-   $��Center=�6E   $��1��6b   $��0 5 0 0q�6o   #���  5�   !   $
��Wrap=�6]   $ Center=�6q   $
��10pt(�5�   !   $��Normal"�61   #���  5�   !   ��6O   �� *Name+�� 5�      $ MouseOverPanel��6   $y�	Collapsed=�65   $0 
Horizontal=�6L   $��Center=�6e   $	��Top=�6�   $��0 25 0 0q�6�   .+�5�        $ settingsButton��60   #���  6H   $��1��5�   4   $
��12 0q�6E   $��0q�5�       5�   S   �� *ShowItemMenu+$ TwoWay=���5�      �� 5�      $��35��6$   $��35��60   $��Center=�6S   $ HighQuality=�6n   
 6<   Z� ��5�   "   �� 5�   &   " ��6,   .�5�   *   ��  � ��# � 	 6C    5�   *   P� 5�   .    �@J�IsMouseOver . . �5�   2   ��  � ��# �  6K   5�   /   5�   '   5�       5�      �� 5�      #��� 
 6H   $��0q�6�   ��6&   �� *	Actions+��5�   "   �� 5�   &   .R�5�   *     #���  6>   $	��200��6i   5�   '   5�      5�      �� 5�      $
��12 0q�6   $��Center=�6-   $��Center=�6H   $ HighQuality=�5�      
������ $��0q�5      y�5�   c   �� *	IsWebSite+8  1 5   0   �� *DataContext++ �� $
- Self=�._�5     �� $��41��6    $��41��6,   $��Center=�67   $ HighQuality=�6R   ��5     �� 5  "   " ��6(   .�5  &   ��  � ��# �  6?    5  &   P� 5  *    �@J�IsMouseOver . . �5  .   ��  � ��# �  6G   5  +   5
  #   5     5     5     5     # 5     d� 5     $$ LeftDoubleClicke�6   % 6=   �� *DoubleClickCommand+5     & 5  
   �� 5     $
' True��6�   ( 6   �� *
IsSelected++ �� ", 	 $- FindAncestor=�.a�5     �� $* 
mainBorder��6   # �  6H    � ���� 5     $* 
mainBorder��6    � ��$ 1q�6L   5     P� 5      �@J�IsMouseOver . . �5     �� $* MouseOverPanel��6    � y�$ Visible=�6K   �� 5     $* MouseOverBorder��6    � y�$ Visible=�6L   �� 5     $* txtName��6    � ��
 �����6D   5     5     �� 55     " ��66   .�56  
   ��  �@J�SnapsToDevicePixels . 60   �� 57  
    � ��$ 0,3,0,3q�6$   �� 58  
    � ��$ Stretch=�67   �� 59  
    � ��$ Center=�65   �� 5:  
    � ��
 ���� 6'   �� 5;  
    � ��
 ���� 6(   �� 5<  
    � ��$ 0q�6,   �� 5=  
    � �� 5>     �� 5?     "2 ��6"   .R�5@     �� $  Bd��6   #��z��6)   #��z��6U   #��z��6�   # z��6�   3 . ._�5A     �� #3 z3 6,   5B     5C     5E     5F     �� 5I     " ��6,   .�5J  
   ��  �@�ContextMenu 6(   Z� �� 5K  
    � ��
 ���� 6'   �� 5L  
    � ��$ 0q�6,   �� 5M  
    �@��SelectionMode$ Multiple=�6*   �� 5N  
    � ��$ Disabled=�6G   �� 5O  
    � ��8   6/    5P  
   �� 5Q     ( 6   �� *DataContext.IsTileView++ �� *FindAncestor+", 
 ' True6�   .a�5R     ��  � ��# �   5S     5T     �� 5U     ( 6   �� *DataContext.IsTileView++ �� *FindAncestor+", 
 	' False6�   .a�5V     ��  � ��# � ! 5W     5X     �� 5Z     ( 6   �� *DataContext.IsTileView++ �� *FindAncestor+", 
 ' True6�   .a�5[     ��  � ��# �  5\     5]     �� 5^     ( 6   �� *DataContext.IsTileView++ �� *FindAncestor+", 
 	' False6�   .a�5_     ��  � ��# �  5`     5a     5c     �� 5e     .R�5f  
   �� 
������ .+�5g     �� ���	#3F008769$��1q�6-   ���	#3F00A785$
��Left=�6Y   $ 10q�6t   ._�5h     � $
��Left=�6   .+�5i     � $
��Left=�6   y�67   �� *HasOne+8   . 5j     �� 5k     $
��auto �6,   �� 5l     $
��auto �6,   .+�5n     � $
/ 0.75Q�6    $��60��6/   $��30��6:   ��6F   �� *" CurrentDraggedItem[0].Background+.+�5o     �� $ dataImg��6$   $
��Fill=�5p  T   y�5o  6   �� *&$CurrentDraggedItem[0].IsDefaultImage+8  ��5p  %   �� *CurrentDraggedItem[0].Image+5q     �� 5r     $��25��6&   $��Center=�65   $��Uniform=�5s  T   y�5r  P   �� *&$CurrentDraggedItem[0].IsDefaultImage+8  ��5s  %   �� *CurrentDraggedItem[0].Image+5t     5u     5v     � 5w     $
��Left=�6   $
/ 0.75Q�66   $��60��6E   $��30��6P   #��� # 6]   y�6�   �� *HasOne+8  .+�5x     �� $��Center=�6N   $��Center=�6k   #���  6�   $��15(�5y  *   $
��Bold"�68   #���  5z  )   ��5x  $   �� *CurrentDraggedItem.Count+5z  )   5{     5|     5}     �� 5     #��� $ 6   $��1q�6R   $��0 2 0 0q�6f   $ 1��6w   
�������y�6�   �� *IsValidDrop+8  ._�5�     �� $0 
Horizontal=�6   .+�5�     �� $��Move To ��6    #���  62   $��11(�5�     $��Normal"�6,   $��5 5 0 5q�6A   #���  5�     �� 5�     #��� $ 6T   $��11(�5�     $��Normal"�6,   $
��Wrap=�6B   $��0 5 5 5q�6V   #���  5�     ��5�      �� *#!CurrentDraggedOverItem.FolderName+5�     5�     5�     5�     5�     5�     