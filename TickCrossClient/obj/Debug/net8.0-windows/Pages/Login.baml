   M S B A M L   `   `   `  ����J  FTickCrossClient, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null=#clr-namespace:TickCrossClient.PagesTickCrossClient.Pages  5      !    TickCrossClient.Pages.Login   5      W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5      R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      5local#clr-namespace:TickCrossClient.Pages     5      U QWpfAnimatedGif, Version=2.0.0.0, Culture=neutral, PublicKeyToken=9e7cd3b544a090dc,gif"http://wpfanimatedgif.codeplex.com  5      _ [MaterialDesignThemes.Wpf, Version=5.2.1.0, Culture=neutral, PublicKeyToken=df2a72020bd7962aFmaterialDesign1http://materialdesigninxaml.net/winfx/xaml/themes  5        N� Title$  Login��5
      .��5      � .+�5   
   �� -   5         ��Name$ 
MainBorder��5      $	��300��6%   $	��400��61   
������� �� CornerRadius$ 6��6Q    J� Effect 5      3  -System.Windows.Media.Effects.DropShadowEffect  5         
BlurRadius$ 15Q�6#      	Direction$	 -90Q�63      RenderingBias$ Quality=�5         ShadowDepth$ 2Q�64      Color$
 Gray��6D   ._�5      �� -   5        	 ��Name$	 	MainPanel��5      
 �� Orientation$
 Vertical=�6,   $��20q�6C   .+�5      �� $��Login��6   $��25(�6)   $��Center=�5      $	��Top=�5      $
��Bold"�5       �� Padding$ 10q�6'   � 5      $��Center=�6    � RowDefinitions 5      �� 5       $��* �6(   �� 5!      $��* �6(   �� 5"      $��* �6(   �� 5#      $��* �6(   .+�5&      �� -   5        $��UserNameBlock��5&   !   $��	User Name��5'      $��0��5(      $
��0,15q�5)      $
��Left=�5*      �� 5,      -   5        $��LoginBox��5.      $��1��5-      $��1��5/      $	��0,5q�50      $	��255��51      $��0,0,0,2q�52      $
��Left=�53      $�� ��54      �� 56      -   5        $��PasswordBlock��56       $��Password��57      $��2��58      $
��0,15q�59      $
��Left=�5:      G� 5<      -   5        $��PasswordBox��5=      $��3��5<   "    G� Password$  ��5>      $	��0,5q�5?      $	��255��5@      $��0,0,0,2q�5A      $
��Left=�5B      5D      �� 5F      -   5         ��Name$ LoginBut��5F      $��10 10 10 0 q�6,    #  MaterialDesignRaisedLightButton#��[  5G      $��Login��5H      
������ 
�������
���   �5L      �� 5N      -   5        $ RegistrationBut��5N      $��	10 5 10 0q�63   #��[  5O      $��Registration��5P      
������ 
�������
���   �5T      �� 5V      -	   5        $��
KchauImage��5V      $��/Visuals/Images/kchau.jpg��6*   $y�Hidden=�6M   5[      5]      5_      5`      