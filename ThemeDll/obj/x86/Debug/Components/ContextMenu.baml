   M S B A M L   `   `   `  ����@  <theme, Version=1.2.1.0, Culture=neutral, PublicKeyToken=null?$clr-namespace:PasswordBoss.BehaviorsPasswordBoss.Behaviors  5      W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089a ]Telerik.Windows.Data, Version=2016.1.217.40, Culture=neutral, PublicKeyToken=5803cfa389c90ce7e aTelerik.Windows.Controls, Version=2016.1.217.40, Culture=neutral, PublicKeyToken=5803cfa389c90ce7n jTelerik.Windows.Controls.GridView, Version=2016.1.217.40, Culture=neutral, PublicKeyToken=5803cfa389c90ce7p lTelerik.Windows.Controls.Navigation, Version=2016.1.217.40, Culture=neutral, PublicKeyToken=5803cfa389c90ce7k	 gTelerik.Windows.Controls.Input, Version=2016.1.217.40, Culture=neutral, PublicKeyToken=5803cfa389c90ce7-   'Telerik.Windows.Controls.RadContextMenu  �� 
TargetType �� Property �� Value    LightGrayTextForegroundColor  ProximaRegular   SettingsLightGrayBorderColor �� 
TargetType'  !System.Windows.VisualStateManager   VisualStateGroups%  System.Windows.VisualStateGroup   States   System.Windows.VisualState   
Storyboard �� TargetProperty �� 
TargetName	 R� KeyTime
 R� Value �Name ��Name  CommonCornerRadius �� CornerRadius �� RelativeSource ��Name9  3Telerik.Windows.Controls.Primitives.SharedSizeScope   IsSharedSizeScope J� SnapsToDevicePixels ��Name �� CanContentScroll �� CanHorizontallyScroll �� CanVerticallyScroll �� ContentStringFormat �� ContentStringFormat �� Color g� Duration	 W� To F�Name �� Stretch �� BasedOn `� ConverterParameter �� FallbackValue  `� UpdateSourceTrigger! `� 	Converter" �� Path  RadMenuItemControlTemplate*  $Telerik.Windows.Controls.RadMenuItem  System.Windows.Visibility# � ColumnDefinitions ! MainWindowGrayBackgroundColor$ J� Opacity%   IconColumnWidth& ��Name'   IconTemplate(   Icon) �� HeaderStringFormatD  >Telerik.Windows.Controls.Primitives.SharedSizeContentPresenter*  Name+   InputGestureText,   SharedSizeGroupWidth- �Name. � AllowsTransparency/ � HorizontalOffset0   IsSubmenuOpen1 � VerticalOffset2   DropDownHeight3   DropDownWidth ! RadSubMenuItemControlTemplate4 �� AncestorType.  (Telerik.Windows.Controls.RadTreeViewItem5 �� Triggers6 P� Property7 P� Value  SettingsGreenColor�� 5      D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    :x,http://schemas.microsoft.com/winfx/2006/xaml     5      Gtelerik1http://schemas.telerik.com/2008/xaml/presentation     	 5      1behavior$clr-namespace:PasswordBoss.Behaviors  5      %�T  '         &	 	    &	 �/    '  �E    7  7  7  7  7  �� 5      "    6   .�5   
   ��  � ��$ 0q�6$   �� 5   
    � ��$ 1q�6,   �� 5	   
    � ��# �   6'   �� 5
   
    � ��# �  6'   �� 5   
    � ��$
 10pt(�6%   �� 5   
    � ��$ Normal"�6'   �� 5   
    � ��
 �����6'   �� 5   
    � ��# �  6(   �� 5   
    �@J�SnapsToDevicePixels . 60   �� 5   
    �@�FocusVisualStyle 6-   Z� �� 5   
    �@��PanningMode$ VerticalFirst=�65   �� 5   
    � �� 5      �� 5      "   6"   .R�5      �  5        5      PopupHostStates��. 5   "     
Standalone��6.     5   "   
Hosted��. 5   &   �� .��5   *   T� $ BorderThickness�6H   $ TemplateBorder��6t   ..�5   .   i� $	 0��6E   
 5   2   u�5   6   0q�6C   5    /   5!   +   5"   '   5#   #   5$      .+�5&      � $ PopupContentElement��6   .+�5'      �� $ TemplateBorder��6%   # �  6=   #��z��6q   #��z��6�   ��6�   �� *BorderThickness+ �� *TemplatedParent+._�5(   "   � .+�5)   &   �� $ IconSeparatorBackground��6-   ���	#FF848484$��0q�6f   
������ $
��Left=�6�   ��6�   �� *IconColumnWidth+ �� *TemplatedParent+�� 5*   &   $ PART_ScrollViewer��63    . ��5+   *   �� 5,   .   "  ��64   .�5-   2   ��  � ��$ 0q�6L   �� 5.   2    � ��$ Hidden=�6b   �� 5/   2    � ��$
 Auto=�6`   �� 50   2    � �� . 6U   �� 51   2    � �� 52   6   �� 53   :   " ��6J   .R�54   >   �  . .+�55   B   �� $! PART_ScrollContentPresenter��6Y   # z��6~    .   .  #��z��6�   #��z��6"  # z 6F  #��z��6�  �� 56   B   $��ScrollBar.LineUpCommand��6�   |�.  $	��Top=�6�   ��6O   ��  �� *TemplatedParent+��57   F   �� 58   J   "  ��6P   .�59   N   ��  � ��$ 0,0,0,1q�6p   �� 5:   N    � �� 5;   R   �� 5<   V   " ��6f   .R�5=   Z   �� #��z��6a   #��z��6�   #��z��6�    5>   ^     5?   b   CommonStates��. 5@   f     
Normal��6r     5A   f   Pressed��. 5B   j   �� .��5C   n   T� $
 Fill�6�   $ ForegroundIcon��6�   ..�5D   r   i� $	 0��6�   
 5E   v   �� 5F   z   $ Black��6�   5H   s   5I   o   T� 5J   n   $
 Fill�6�   $ BackgroundIcon��6�   ..�5K   r   i� $	 0��6�   
 5L   v   �� 5M   z   $ White��6�   5O   s   5P   o   T� 5Q   n   $ Margin�6�   $ ContentContainer��6�   ..�5R   r   i� $	 0��6�   
 5S   v   u�5T   z   1,3,1,1q�6�   5V   s   5W   o   5X   k   5Y   g     5Z   f   Disabled��. 5[   j   �� .��5\   n   W� $ 0D�6~   $	 0.5X�6�   $ Opacity�6�   $ ContentContainer��6�   5]   k   5^   g   5_   c     5`   b   FocusStates��. 5a   f     Focused��6r     5b   f   	Unfocused��6r   5c   c   ._�5e   ^   � $ ContentContainer��6c   $	��1,2q�6}   .+�5f   b   F� $ BackgroundIcon��6g   "����     	= ��g5  � ���j  	= 	
�������$��4��6�   $��0,2,0,0q�6�   $
 None=�6�   $��7��6�   F� 5g   b   $ ForegroundIcon��6g   "����     	= ��g5  � ���j  	= 	
���   �$��4��6�   $	��0,1q�6�   $
 None=�6�   $��7��6�   5h   _   5i   [   5j   W   5l   O    5m   N   �� 5n   R   "  ��6X   .�5o   V   ��  �@��Interval$ 50��6q   �� 5p   V    � �� .  6r   �� 5q   V    � ��$ 0��6q   �� 5r   V    � ��$ 0��6r   �� 5s   V    � �� �	#FFDEDEDE6s   �� 5t   V    � ��
 �����6t   �� 5u   V    � ��$ 0q�6x   �� 5v   V    �@��	ClickMode$ Hover=�6r   5w   S   5y   K   y�5{   F   `� 5|   J   $ 0��6W   $ Visibility.Collapsed��6n   $  Default=�6�   ! 5}   N   l� 5~   R   .1�5�   N   �� $)" #ComputedVerticalScrollBarVisibility�6V    6�   �� *TemplatedParent+�� 5�   N   $" VerticalOffset�6V    6l   �� *TemplatedParent+�� 5�   N   $" ExtentHeight�6V    6j   �� *TemplatedParent+�� 5�   N   $" ViewportHeight�6V    6l   �� *TemplatedParent+5�   K   5�   C   �� 5�   B   $��ScrollBar.LineDownCommand��6�   |�.  $��Bottom=�6�   ��6O   ��  �� *TemplatedParent+��5�   F   �� 5�   J   "  ��6P   .�5�   N   ��  � ��$ 0,1,0,0q�6p   �� 5�   N    � �� 5�   R   �� 5�   V   " ��6f   .R�5�   Z   �� #��z��6a   #��z��6�   #��z��6�    5�   ^     5�   b   CommonStates��. 5�   f     
Normal��6r     5�   f   Pressed��. 5�   j   �� .��5�   n   T� $
 Fill�6�   $ ForegroundIcon��6�   ..�5�   r   i� $	 0��6�   
 5�   v   �� 5�   z   $ Black��6�   5�   s   5�   o   T� 5�   n   $
 Fill�6�   $ BackgroundIcon��6�   ..�5�   r   i� $	 0��6�   
 5�   v   �� 5�   z   $ White��6�   5�   s   5�   o   T� 5�   n   $ Margin�6�   $ ContentContainer��6�   ..�5�   r   i� $	 0��6�   
 5�   v   u�5�   z   1,3,1,1q�6�   5�   s   5�   o   5�   k   5�   g     5�   f   Disabled��. 5�   j   �� .��5�   n   W� $ 0D�6~   $	 0.5X�6�   $ Opacity�6�   $ ContentContainer��6�   5�   k   5�   g   5�   c     5�   b   FocusStates��. 5�   f     Focused��6r     5�   f   	Unfocused��6r   5�   c   ._�5�   ^   � $ ContentContainer��6c   $	��1,2q�6}   .+�5�   b   F� $ BackgroundIcon��6g   "����        ��g5 �g5 ���j     	
�������$��4��6�   $��0,2,0,0q�6�   $
 None=�6�   $��7��6�   F� 5�   b   $ ForegroundIcon��6g   "����        ��g5 �g5 ���j     	
���   �$��4��6�   $	��0,1q�6�   $
 None=�6�   $��7��6�   5�   _   5�   [   5�   W   5�   O    5�   N   �� 5�   R   "  ��6X   .�5�   V   ��  �@��Interval$ 50��6q   �� 5�   V    � �� .  6r   �� 5�   V    � ��$ 0��6q   �� 5�   V    � ��$ 0��6r   �� 5�   V    � �� �	#FFDEDEDE6s   �� 5�   V    � ��
 �����6t   �� 5�   V    � ��$ 0q�6x   �� 5�   V    �@��	ClickMode$ Hover=�6r   5�   S   5�   K   y�5�   F   `� 5�   J   $	 100��6W   $ Visibility.Collapsed��6p   $  Default=�6�   ! 5�   N   l� 5�   R   .1�5�   N   �� $)" #ComputedVerticalScrollBarVisibility�6V    6�   �� *TemplatedParent+�� 5�   N   $" VerticalOffset�6V    6l   �� *TemplatedParent+�� 5�   N   $" ExtentHeight�6V    6j   �� *TemplatedParent+�� 5�   N   $" ViewportHeight�6V    6l   �� *TemplatedParent+5�   K   5�   C   5�   ?   5�   ;   5�   3   �� 5�   2    �@��PanningMode$
 None=�6P   5�   /   .��5�   *   � .+�5�   .   �� 5�   +   5�   '   5�   #   5�      5�      5�      5�      5�      5�      �� 5�      "  69   .R�5�   
   � $ RootElement��6    5�        5�      CommonStates��. 5�        SubMenuOpen��. 5�      �� .��5�      W� $ 0D�6.   $ 1X�6;   $ Opacity�6B   $ HighlightVisual��6f   5�      5�        5�      Highlighted��. 5�      �� .��5�      W� $ 0D�6.   $ 1X�6;   $ Opacity�6B   $ HighlightVisual��6f   5�      5        5     Disabled��. 5     �� .��5     W� $ 0D�6.   $	 0.5X�6;   $ Opacity�6D   $ ContentGrid��6h   5     5       5     
Normal��6"   5       5     FocusStates��. 5	       	Unfocused��6"     5
     Focused��6"   5       5     CheckedStates��. 5       Checked��. 5     �� .��5     T� $ 0D�6<   $ 
Visibility�6I   $
 Tick��6p   ..�5  "   i� $	 0��69   
 5  &    5  *   Visible=�6>   5  #   5     T� 5     $ 0D�6<   $ 
Visibility�6I   $
 Icon��6p   ..�5  "   i� $	 0��69   
 5  &    5  *   	Collapsed=�6@   5  #   5     5     5       5     	Unchecked��6"     5      HideIcon��. 5!     �� .��5"     T� $ 0D�6<   $ 
Visibility�6I   $
 Icon��6p   ..�5#  "   i� $	 0��69   
 5$  &    5%  *   	Collapsed=�6@   5'  #   5(     5)     5*     5+       5,     
RoleStates��. 5-       TopLevelItem��. 5.     �� .��5/     T� $ 0D�6<   $ Margin�6I   $ IconHost��6l   ..�50  "   i� $	 0��69   
 51  &   u�52  *   	-2,0,-7,0q�6?   54  #   55     56     57       58     TopLevelHeader��. 59     �� .��5:     T� $ 0D�6<   $ Margin�6I   $ IconHost��6l   ..�5;  "   i� $	 0��69   
 5<  &   u�5=  *   	-2,0,-7,0q�6?   5?  #   5@     5A     5B       5C     SubmenuItem��6"     5D     SubmenuHeader��. 5E     �� .��5F     T� $ 0D�6<   $ 
Visibility�6I   $ 
ExpandIcon��6p   ..�5G  "   i� $	 0��69   
 5H  &    5I  *   Visible=�6>   5K  #   5L     5M     5N       5O     	Separator��6"   5P       5Q     OrientationStates��. 5R       
Horizontal��6"     5S     Vertical��6"   5T       5U     ChildrenStates��. 5V       HasChildren��6"     5W     
NoChildren��6"   5X       5Y     HeaderStates��. 5Z       VisibleHeader��6"     5[     EmptyHeader��. 5\     �� .��5]     T� $ 0D�6<   $ Margin�6I   $
 Icon��6l   ..�5^  "   i� $	 0��69   
 5_  &   u�5`  *   5,3,4,3q�6=   5b  #   5c     T� 5d     $ 0D�6<   $ 
Visibility�6I   $ Content��6p   ..�5e  "   i� $	 0��69   
 5f  &    5g  *   	Collapsed=�6@   5i  #   5j     5k     5l     5m       5n     InputGestureTextStates��. 5o       EmptyInputGestureText��6"     5p     VisibleInputGestureText��. 5q     �� .��5r     T� $ 0D�6<   $ 
Visibility�6I   $ PART_InputGestureText��6p   ..�5s  "   i� $	 0��69   
 5t  &    5u  *   Visible=�6>   5w  #   5x     5y     5z     5{     .+�5}     �� #��z��6   #��z��6A   #��z��6u   � 5~     $ ContentGrid��6   $��0q�6(   $��Stretch=�63   # 5     �� 5�     $
��Auto �6'   $��25��64   �� 5�     $��* �6'   �� 5�     $
��Auto �6'   �� 5�     $
��Auto �6'   $��40��64   .+�5�     �� $ HighlightVisual��6   $��0q�62   #���  6F   $��4��6�   $ 1��6�   $$ 0Q�6�   ._�5�     �� $  HighlightInnerBorderVisual��6   
�������$��0q�6U   $ 0��6i   5�     � 5�     $ IconHost��6   $��0q�6)   #��z% 64   .+�5�     F� $
 Tick��6   =���� 	= `Vl Ѡ�V  �m �@�p �ڊ � �  �$ рj� �� Ѡco @�� 	#��z��6_   $��LeftToRight=�6�   $��16��6�   $��0 0 4 0 q�6�   $y�	Collapsed=�6�   $��16��6�   �� 5�     $
& Icon��6'   #��z' 65   $��Center=�6f   #��z( 6�   $��5,3,4,3q�6�   5�     �� 5�     $& Content��6#   #��z��64   #��z��6g   $��1��6�   # z) 6�   $��Header��6�   #��z��6�   #��z��6/  ��. $��Bottom=�6m    5�     $* PART_InputGestureText��65   #��z+ 6T   $��2��6�   $��40,3,3,3q�6�   $, 	IGTColumn��6�   $y�	Collapsed=�6�     5�     $��3��65   $��Right=�6E   $, ExpandIconColumn��6a   .��5�     � $ 
ExpandIcon��6   $��19��6/   $��0,0,2,0q�6;   $y�	Collapsed=�6L   $��16��6c   .+�5�     F� �����        �    ��j �@B ��j �@B ��[ р� ��[ р� @KL ���- @KL ���-  	= � 	=  	= � 	= ��- ���- ��- ���- �� р� �� р� @B �@B @B �@B     	
�������$��6,7,6,5q�6u   F� 5�     �����        �    ��j �@B ��j �@B ��[ р� ��[ р� @KL ���- @KL ���-  	= � 	=  	= � 	= ��- ���- ��- ���- �� р� �� р� @B �@B @B �@B     	#���   6h   $��6q�6�   5�     5�     5�     � 5�     $- 
PART_Popup��6   . . |�.  $/ -1��6T   #��z0 6j   $1 -1��6�   .��5�     � #��z2 6   #��z3 6A   .+�5�     � $ PopupContentElement��6   .+�5�     �� #���  6!   $��1q�6^   
�������._�5�     � $	��300��6#   .+�5�  "   �� $ IconSeparatorBackground��6)   ���	#FF848484$��0q�6b   
������ $
��Left=�6�   ��6�   �� *IconColumnWidth+ �� *TemplatedParent+�� 5�  "   $ PART_ScrollViewer��6/    . .��5P  &   � .+�5Q  *   �� 5R  '   5S  #   5T     5U     5V     5W     5X     5Y     5Z     �� 5\     "  6<   .R�5]  
   � $ RootElement��6    5^       5_     CommonStates��. 5`       SubMenuOpen��. 5a     �� .��5b     W� $ 0D�6.   $ 1X�6;   $ Opacity�6B   $ HighlightVisual��6f   5c     5d       5e     Highlighted��. 5f     �� .��5g     W� $ 0D�6.   $ 1X�6;   $ Opacity�6B   $ HighlightVisual��6f   5h     5i       5j     Disabled��. 5k     �� .��5l     W� $ 0D�6.   $	 0.5X�6;   $ Opacity�6D   $ ContentGrid��6h   5m     5n       5o     
Normal��6"   5p       5q     FocusStates��. 5r       	Unfocused��6"     5s     Focused��6"   5t       5u     CheckedStates��. 5v       Checked��. 5w     �� .��5x     T� $ 0D�6<   $ 
Visibility�6I   $
 Tick��6p   ..�5y  "   i� $	 0��69   
 5z  &    5{  *   Visible=�6>   5}  #   5~     T� 5     $ 0D�6<   $ 
Visibility�6I   $
 Icon��6p   ..�5�  "   i� $	 0��69   
 5�  &    5�  *   	Collapsed=�6@   5�  #   5�     5�     5�       5�     	Unchecked��6"     5�     HideIcon��. 5�     �� .��5�     T� $ 0D�6<   $ 
Visibility�6I   $
 Icon��6p   ..�5�  "   i� $	 0��69   
 5�  &    5�  *   	Collapsed=�6@   5�  #   5�     5�     5�     5�       5�     
RoleStates��. 5�       TopLevelItem��. 5�     �� .��5�     T� $ 0D�6<   $ Margin�6I   $ IconHost��6l   ..�5�  "   i� $	 0��69   
 5�  &   u�5�  *   	-2,0,-7,0q�6?   5�  #   5�     5�     5�       5�     TopLevelHeader��. 5�     �� .��5�     T� $ 0D�6<   $ Margin�6I   $ IconHost��6l   ..�5�  "   i� $	 0��69   
 5�  &   u�5�  *   	-2,0,-7,0q�6?   5�  #   5�     5�     5�       5�     SubmenuItem��6"     5�     SubmenuHeader��. 5�     �� .��5�     T� $ 0D�6<   $ 
Visibility�6I   $ 
ExpandIcon��6p   ..�5�  "   i� $	 0��69   
 5�  &    5�  *   Visible=�6>   5�  #   5�     5�     5�       5�     	Separator��6"   5�       5�     OrientationStates��. 5�       
Horizontal��6"     5�     Vertical��6"   5�       5�     ChildrenStates��. 5�       HasChildren��6"     5�     
NoChildren��6"   5�       5�     HeaderStates��. 5�       VisibleHeader��6"     5�     EmptyHeader��. 5�     �� .��5�     T� $ 0D�6<   $ Margin�6I   $
 Icon��6l   ..�5�  "   i� $	 0��69   
 5�  &   u�5�  *   5,3,4,3q�6=   5�  #   5�     T� 5�     $ 0D�6<   $ 
Visibility�6I   $ Content��6p   ..�5�  "   i� $	 0��69   
 5�  &    5�  *   	Collapsed=�6@   5�  #   5�     5�     5�     5�       5�     InputGestureTextStates��. 5�       EmptyInputGestureText��6"     5�     VisibleInputGestureText��. 5�     �� .��5�     T� $ 0D�6<   $ 
Visibility�6I   $ PART_InputGestureText��6p   ..�5�  "   i� $	 0��69   
 5�  &    5�  *   Visible=�6>   5�  #   5�     5�     5�     5�     .+�5�     �� #��z��6   #��z��6A   #��z��6u   � 5�     $ ContentGrid��6   $��0q�6(   $��Stretch=�63   # 5�     �� 5�     $
��Auto �6'   $��0��64   �� 5�     $��* �6'   �� 5�     $
��Auto �6'   �� 5�     $
��Auto �6'   $��40��64   .+�5�     �� $ HighlightVisual��6   $��0q�62   #���  6F   $��4��6�   $ 1��6�   $$ 0Q�6�   ._�5�     �� $  HighlightInnerBorderVisual��6   
�������$��0q�6U   $ 0��6i   5�     � 5�     $ IconHost��6   $��0q�6)   $��0��64   .+�5�     F� $
 Tick��6   =���� 	= `Vl Ѡ�V  �m �@�p �ڊ � �  �$ рj� �� Ѡco @�� 	#��z��6_   $��LeftToRight=�6�   $��16��6�   $��0 0 4 0 q�6�   $y�	Collapsed=�6�   $��16��6�   �� 5�     $
& Icon��6'   #��z' 65   $��Center=�6f   #��z( 6�   $��5,3,4,3q�6�   5�     �� 5�     $& Content��6#   #��z��5�  #   #��z��6V   $��1��6y   # z) 6�   $��Header��6�   #��z��6�   #��z��6  ��. $��Bottom=�6\    5�     $* PART_InputGestureText��65   #��z+ 6T   $��2��6�   $��40,3,3,3q�6�   $, 	IGTColumn��6�   $y�	Collapsed=�6�     5�     $��3��65   $��Right=�6E   $, ExpandIconColumn��6a   .��5�     � $ 
ExpandIcon��6   $��19��6/   $��0,0,2,0q�6;   $y�	Collapsed=�6L   $��16��6c   .+�5�     F� �����        �    ��j �@B ��j �@B ��[ р� ��[ р� @KL ���- @KL ���-  	= � 	=  	= � 	= ��- ���- ��- ���- �� р� �� р� @B �@B @B �@B     	
�������$��6,7,6,5q�6u   F� 5�     �����        �    ��j �@B ��j �@B ��[ р� ��[ р� @KL ���- @KL ���-  	= � 	=  	= � 	= ��- ���- ��- ���- �� р� �� р� @B �@B @B �@B     	#���   6h   $��6q�6�   5�     5�     5�     � 5�     $- 
PART_Popup��6   . . |�.  $/ -1��6T   #��z0 6j   $1 -1��6�   .��5      � #��z2 6   #��z3 6A   .+�5     � $ PopupContentElement��6   .+�5     �� #���  6!   $��1q�6^   
�������._�5     � .+�5  "   �� $ IconSeparatorBackground��6)   ���	#FF848484$��0q�6b   
������ $
��Left=�6�   ��6�   �� *IconColumnWidth+ �� *TemplatedParent+�� 5  "   $ PART_ScrollViewer��6/    . $
��Auto=�6{   .��5�  &   � $	��300��6+   .+�5�  *   �� 5�  '   5�  #   5�     5�     5�     5�     5�     5�     5�     �� 5�     "   6   .�5�  
   ��  � ��$
 0 10q�6$   �� 5�  
    � ��$ 0q�6#   �� 5�  
    �@�Cursor$
 Hand��6#   �� 5�  
    � ��# �   6'   �� 5�  
    � ��# �  6'   �� 5�  
    � ��$
 10pt(�6%   �� 5�  
    � ��$ Normal"�6'   �� 5�  
    � ��$ Bottom=�65   �� 5�  
    � ��8   6%   �� 5�  
   ! �@ TopLevelHeaderTemplateKey8  66   �� 5�  
    �@ TopLevelItemTemplateKey8  64   �� 5�  
     �@ SubmenuHeaderTemplateKey8  65   �� 5�  
    �@ SubmenuItemTemplateKey8  63   �� 5�  
    �@ IconColumnWidth$ 26Q�6,   �� 5�  
    �@ SeparatorTemplateKey 5�     �� 5�     "  6"   .R�5�     �  5�       5�     OrientationStates��. 5�  "     VerticalState��6.     5�  "   HorizontalState��. 5�  &   �� .��5�  *   T� $ 0D�6H   $ 
Visibility�6U   $ HorizontalSeparator��6|   ..�5�  .   i� $	 0��6E   
 5�  2    5�  6   	Collapsed=�6L   5�  /   5�  +   T� 5�  *   $ 0D�6H   $ 
Visibility�6U   $ VerticalSeparator��6|   ..�5�  .   i� $	 0��6E   
 5�  2    5�  6   Visible=�6J   5�  /   5�  +   5�  '   5�  #   5�     .+�5�     � $ HorizontalSeparator��6   # 5�     �� 5�  "   $
��Auto �63   �� 5�  "   $��* �63   .+�5�     � $��0��6#   $
��Left=�63   #��z% 6N   � 5�     $��1��6#   $��2��63   $��4,4,4,3q�6>   .+�5�  "   �� ���	#FF848484$��1��6=   $	��Top=�6H   �� 5�  "   
������ $��1��6?   $��Bottom=�6J   5�     5�     � 5�     $ VerticalSeparator��6   $��2q�6:   $y�	Collapsed=�6E   $��2��6\   .+�5�     �� ���	#FF848484$
��Left=�69   $��1��6T   �� 5�     
������ $��Right=�6;   $��1��6W   5�     5      5     5     �� 5  
    �@��PanningMode$ VerticalFirst=�65    5  
   �� 5     "   6   .�5     ��  � ��
 ���� 6/   �� 5      � �� �	#FF84848460   �� 5	      � ��$ 0q�64   �� 5
      � ��$
 Left=�6?   �� 5      � ��$ Center=�6=   �� 5      �@J�SnapsToDevicePixels . 68   �� 5      �@�FocusVisualStyle 65   Z� �� 5      � y� 6/   �� *
Visibility+�� 5      �@ Icon 6)   �� *Icon+�� 5      �@�Tag 6(   �� $" DataContext� �� "4  �� 5      �@ IconTemplate 5     �� 5     .R�5     �� $��16��67   $��Uniform=�6C   $��Center=�6U   $��Center=�6r   ��6$   �� 6r   5     5     �� 5      � �� 6+   �� *Name+�� 5      � �� 60   �� *
SubItems+�� 5      �@ Command 6,   �� *Action+�� 5      �@ CommandParameter 65   �� $" DataContext� �� "4  5     5 5  
   P� 5     6 �@ Role$7 TopLevelHeader=�6&   . �5      ��  � �� 6-   �� *TopLevelHeaderTemplateKey+ �� *Self+5!     P� 5"     6 �@ Role$7 TopLevelItem=�6&   . �5#     ��  � �� 6-   �� *TopLevelItemTemplateKey+ �� *Self+5$     P� 5%     6 �@ Role$7 SubmenuHeader=�6&   . �5&     ��  � �� 6-   �� *SubmenuHeaderTemplateKey+ �� *Self+5'     P� 5(     6 �@ Role$7 SubmenuItem=�6&   . �5)     ��  � �� 6-   �� *SubmenuItemTemplateKey+ �� *Self+5*     P� 5+     6 �@ Role$7 	Separator=�6&   . �5,     ��  � �� 6-   �� *SeparatorTemplateKey+ �� *Self+5-     P� 5.     6 �@J�IsMouseOver7 . . �5/     ��  � ��# �  6/   �� 50      �@ Icon 6)   �� *	IconHover+51     53     56     