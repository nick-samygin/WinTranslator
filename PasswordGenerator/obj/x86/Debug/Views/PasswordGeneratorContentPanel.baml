   M S B A M L   `   `   `  ����L  HPasswordGenerator, Version=1.2.1.0, Culture=neutral, PublicKeyToken=null7 clr-namespace:PasswordBoss.ViewsPasswordBoss.Views  5      6    0PasswordBoss.Views.PasswordGeneratorContentPanel   5      W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      4pbviews clr-namespace:PasswordBoss.Views     5        � 	Resources  5	         BoolToVisibility��%   &	        �� 5
   
   6'   5      .��5      � -   5         �Name$ mainpassword_grid��5      $��1��6&    ! MainWindowGrayBackgroundColor#���  5       K� ZIndex$ 0��6H    � RowDefinitions 5   
   �� 5      $��44 �6   �� 5      $��43 �6   �� 5      $	��430 �6   �� 5      $��20 �6   �� 5      $	��130 �6    � ColumnDefinitions 5   
   �� 5      $��22 �6   �� 5      $��* �6   �� 5      $��20 �6   .+�5   
   �� $��0��6   $��0��6!   $��3��6.     
WhiteColor#���  6B   �� 5   
   $��0��6   $��1��6!     CreatePasswords#���  61   $��14(�6Z     ProximaSemiBold#���  5        $
��Bold"�6O     HeaderForegroundColorColor#���  5!       $��Center=�5"       $
��Left=�6;   �� 5$   
   $��1��6   $��1��6!   $
��Left=�61   $��Center=�6L   $	��650��6g   $
��Wrap=�5%       $��12(�5&         ProximaLight#���  6.      LightGrayTextForegroundColor#���  6Z    * &UsePasswordBossToCreateStrongPasswords#���  5'       5(      �� 5*   
   $��2��6   $��1��6   #���  6.   $��0q�6X    	 TextBoxBackground#��� 	 6l    �� CornerRadius$ 5��5+      ._�5,      �  5-      �� 5.      $��* �6'   �� 5/      $	��280 �6'   �� 50      $��* �6'    52      �� 53      $��18 �6$   �� 54      $��35 �6$   �� 55      $��1 �6$   �� 56      $
��Auto �6$   �� 57      $��1 �6$   �� 58      $
��Auto �6$   �� 59      $
��Auto �6$   �� 5:      $��30 �6$   �� 5;      $
��Auto �6$   �� 5<      $��24 �6$   �� 5=      $
��Auto �6$   �� 5>      $��43 �6$   �� 5?      $
��Auto �6$   �� 5@      $	��100 �6$   .+�5B      �� $��1��6   $��1��6'   $��1q�67   $ 5��6K   #��� 	 6\   $��35��5C   &   $	��280��62   $��Stretch=�6>   ._�5E      �� -   5         ��Name$ 	tPassword��5E      #��� 	 61   #���  6b   $��0q�6�   $��Stretch=�5F   *   $��Stretch=�6F   $
��Left=�5G   *    �� 	MaxLength$ 30��6L   $��Center=�5H   *   $��	11,0,12,3q�6L   $��14(�5J   *   $��35��69    �� 
IsReadOnly . $��0,0,0,0q�5K   <   ��5I   *   �� *SelectedItem+	 �� ElementName$	 HistoryListView��
 �� UpdateSourceTrigger$
 PropertyChanged=� �� Mode$ TwoWay=�5K   <   5L      � 5M      -   5         �Name$ PrgPwdGenerate��5M      $��3��66   $��1��6C   $��3��6T   $	��220��6_    
 PasswordProgressBarSetup#��� 
 5N   *   $
��Left=�5O   *    � Value 5N   ]   �� *PasswordStrengthValue+$
 PropertyChanged=�$ TwoWay=�5O   *   �� 5P      -   5         ��Name$ pStrong��5P      $��3��6-   $��1��6:   $��11(�6�   $��Italic$�5Q   (   ���#a7a7a7  ProximaRegularLight#���  6P   $��Right=�5R   (    �� TextAlignment$ Center=�6D   $��Center=�6[   ��5P   J   �� *PasswordStrengthText+$
 PropertyChanged=�$ TwoWay=�5R   [   �� 5T      -   5         ��Name$ ShowHistoryButton��5T      $��5��64   $��1��6A   #���  6Q   $��12(�5U   )   $��SemiBold"�67     PasswordBossGreenColor#���  6M     ProximaRegular#���  5V   )     ShowHistory#���  6W   $
��Left=�5W   )   $��16��6D   $	��Top=�6P   $	��Top=�6i     btnStyleNoHover#���  5X   )   y�6S   �� *ShowHistoryVisibility+ �� 	Converter# [  $
 PropertyChanged=�$ TwoWay=���5Y   )   �� *ShowHistoryCommand+�� 5[      -   5        $ HideHistoryButton��5[      $��5��64   $��1��6A   #���  6Q   $��12(�5\   )   $��SemiBold"�67   #���  6M   #���  5]   )     HideHistory#���  6W   #���  5^   )   $
��Left=�5_   )   y�5^   S   �� *HideHistoryVisibility+# [  $
 PropertyChanged=�$ TwoWay=���5_   D   �� *HideHistoryCommand+�� 5b      -   5         ��Name$ historyborder��5b      $��0,0,0,10q�60   $��6��6B   $��1��6O   $��1q�5c      $ 5��6-   #��� 	 6>   y�5d      �� *HideHistoryVisibility+# [  $
 PropertyChanged=�$ TwoWay=�._�5f      � -   5        $ showhistory_grid��5f      .+�5g      �� $��1q�6!   $ 5��65   ._�5l      �� -	   5         ��Name$ HistoryListView��5l   '   $	��125��6@   $��0q�6M    ! SecurityScoreListViewTemplate#���  5m   '    ) %PasswordGeneratorHistoryListItemStyle#���  5p   '   ��5n   '   �� *AllGeneratedPasswords+$
 PropertyChanged=�$ TwoWay=� �� SelectedItem 5o   '   �� *GeneratedPassword+$
 PropertyChanged=�$ TwoWay=�5p   '   5s      5t      5u      �� 5w      $��8��6   $��1��6)     PasswordLength#���  69   $
��12.8(�6a   #���  5x   (   #���  6T   $	��Top=�5y   (   $
��Left=�6@   $��	0,-17,0,0q�5z   (   �� 5{      -
   5         ��Name$ PasswordSlider��5{      $��8��61   $��1��6>     SliderStyle1#���  5|   -   $	��220��6�    � Minimum$ 4Q�5}   -    � Maximum$ 30Q�69   $
��Left=�6G    �� IsSnapToTickEnabled .  �� TickFrequency$ 1Q�5~   H   $��Center=�5   %   $��0,5,0,0q�6@    5|   T   �� *PasswordLength+$
 PropertyChanged=�$ TwoWay=�5   @   �� 5�      -   5        $ passwordlength_txt��5�      $��0,5,0,0q�68   $��8��6I   $��1��6V   $��Right=�5�   (   #���  6D   $��Center=�5�   (   #���  5�   (   $��18(�5�   (   ��5�   (   �� $	 PasswordSlider�� �� Path$ Value� �� StringFormat$ {00:0}��5�   (   � 5�      $��10��6   $��1��6%   $��2��65   $��Center=�6F    5�      �� 5�      $
��Auto �6+   �� 5�      $��50 �6+   �� 5�      $
��Auto �6+    5�      �� 5�      $
��Auto �6(   �� 5�      $
��Auto �6(   .+�5�      �� -   5         ��Name$ cLetters��5�      $��0��62   $��0��6B   #���  6O   $��0,10,0,0q�6�   $��12(�5�   +   #���  69     Letters#���  5�   +   $��Center=�6O     filledcheckboxstyle#���  5�   +    d� 	IsChecked 5�   +   �� *LettersCheck+$
 PropertyChanged=�$ TwoWay=��� 5�      -   5        $ cSymbols��5�      $��2��61   $��0��6A   #���  6N   $��0,10,0,0q�6�   $��12(�5�   +   #���  69     Symbols#���  5�   +   $��Center=�6O   #���  5�   +    6Y   �� *SymbolsCheck+$
 PropertyChanged=�$ TwoWay=��� 5�      -   5        $ cNumbers��5�      $��0��61   $��1��6A   #���  6N   $��0,10,0,0q�6�   $��12(�5�   +   #���  69     Numbers#���  5�   +   $��Center=�6O   #���  5�   +    6Y   �� *NumbersCheck+$
 PropertyChanged=�$ TwoWay=��� 5�      -   5        $ 	cCapitals��5�      $��2��62   $��1��6B   #���  6O   $��0,10,0,0q�6�   $��12(�5�   +   #���  6:     Capitals#���  5�   +   $��Center=�6P   #���  5�   +    6Y   �� *CapitalsCheck+$
 PropertyChanged=�$ TwoWay=�5�      �� 5�      $��0,0,0,20q�6   $��13��6/   $��1��6=    �� Orientation$ 
Horizontal=�6M   $��Center=�6f   .+�5�      �� -   5        $ 	createbtn��5�       
 Create#���  60   $��97��6S   #���  5�   )     roundButton#���  6S   $��13(�6y   $��SemiBold"�6�   $��25��6�   ��6�   �� *CreatePasswordCommand+�� 5�      -   5        $ copybtn��5�      $��15,0,0,0q�6.     Copy#���  6@   $��97��6a   #���  6l   #���  5�      $��13(�6C   $��SemiBold"�6Q   $��25��6g   ��6s   �� *CopyCommand+5�      5�      5�      5�      5�      