   M S B A M L   `   `   `  ����J  FTickCrossClient, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null=#clr-namespace:TickCrossClient.PagesTickCrossClient.Pages  5      '    !TickCrossClient.Pages.UserOptions   5      W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5      R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      5local#clr-namespace:TickCrossClient.Pages     5      _ [MaterialDesignThemes.Wpf, Version=5.2.1.0, Culture=neutral, PublicKeyToken=df2a72020bd7962aFmaterialDesign1http://materialdesigninxaml.net/winfx/xaml/themes  5        N� Title$  UserOptions��5	      $��Center=�5      $��Center=�6"   .��5      � .+�5   
   �� -   5        $��OptionBorder��5      $	��185��6&   $	��330��62   
������� �� CornerRadius$ 6��6R   $
��Left=�5   
   $��5q�6%    J� Effect 5      3  -System.Windows.Media.Effects.DropShadowEffect  5         
BlurRadius$ 15Q�6#      	Direction$	 -90Q�63      RenderingBias$ Quality=�5         ShadowDepth$ 2Q�6&      Color$
 Gray��66   ._�5      ��  �� Orientation$ Vertical=�6   .+�5      �� -   5        $��OptionsBlock��5      $��Options��62   $��Center=�6A   $��24(�5      $
��Bold"�6*   $��5q�6<   �� 5      -   5        $��
LoginBlock��5      $��	New login��6.   $��10 0 0 0q�5      $
��Bold"�6.   �� 5      -   5        	 ��Name$	 LoginTextBox��5      )  #MaterialDesignThemes.Wpf.HintAssist
   Hint$
 Login��5      $	��165��6:   $��30��5       $��Center=�6!    �� 	MaxLength$ 12��5!      $
��Left=�5"      $��10q�6.   �� 5$      -   5        $��PasswordBlock��5$      $��New Password��62   $��10 0 0 0q�5%      $
��Bold"�6.   �� 5&      -   5        $	 PasswordTextBox��5'      $ 12��6,   $
 Password��5(      $	��165��6=   $��30��5)      $��Center=�6!   $
��Left=�5*      $��10q�6.   �� 5,      -   5         ��Name$ ApplyBut��5,      $��10 10 10 0 q�6,    #  MaterialDesignRaisedLightButton#��[  5-      $��Apply��5.      
������ 
�������
���   �52      �� 54      -   5        $ BackBut��54      $��10 10 10 0 q�6+   #��[  55      $
��Back��56      
������ 
�������
���   �5:      5;      5=      5>      5?      