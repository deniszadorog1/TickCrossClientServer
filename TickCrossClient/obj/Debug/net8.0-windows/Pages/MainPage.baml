   M S B A M L   `   `   `  ����J  FTickCrossClient, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null=#clr-namespace:TickCrossClient.PagesTickCrossClient.Pages  5      $    TickCrossClient.Pages.MainPage   5      W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5      R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      5local#clr-namespace:TickCrossClient.Pages     5      _ [MaterialDesignThemes.Wpf, Version=5.2.1.0, Culture=neutral, PublicKeyToken=df2a72020bd7962aFmaterialDesign1http://materialdesigninxaml.net/winfx/xaml/themes  5      $	��450��5	      $	��500��6     N� Title$  MainPage��5
      $��Center=�5      $��Center=�6"   .��5      �  � ColumnDefinitions 5   
   �� 5      �� 5      .+�5   
   �� $��0��6   $;��5pack://application:,,,/Visuals/Images/ListInCells.png��6    $
��Fill=�5       K� ZIndex$ 0��6   �� 5   
   $��1��6   $;��5pack://application:,,,/Visuals/Images/ListInCells.png��6    $
��Fill=�5      $ 0��6   �� 5   
   -   5         ��Name$ 
MenuBorder��5      $��0��5      $	��190��6!   $	��340��6-   
������� �� CornerRadius$ 6��5      $��Right=�6"   $��0 0 20 0q�6>    J� Effect 5      3  -System.Windows.Media.Effects.DropShadowEffect  5          
BlurRadius$ 15Q�6#      	Direction$	 -90Q�63      RenderingBias$ Quality=�5!      	   ShadowDepth$	 2Q�6-   
   Color$

 Gray��6=   ._�5$      �� -   5         ��Name$ 	MenuPanel��5$      .+�5%      �� -   5        $��MenuName��5%      $
��Menu��6,   $��Center=�68   $��22(�6U   $
��Bold"�6c   $��10q�6u   �� 5'      -   5         ��Name$ StartGameBut��5'      $��
10 0 10 0 q�60    #  MaterialDesignRaisedLightButton#��[  5(      $��
Start Game��5)      
������ 
�������
���   �5-      �� 5/      -   5        $ FriendOptionsBut��5/      $��10 10 10 0 q�64   #��[  50      $��Friend options��51      
������ 
�������
���   �55      �� 57      -   5        $ 
OptionsBut��57      $��10 10 10 0 q�6.   #��[  58      $��Options��59      
������ 
�������
���   �5=      �� 5?      -   5        $ GameReqsBut��5?      $��10 10 10 0 q�6/   #��[  5@      $��Game requests��5A      
������ 
�������
���   �5E      �� 5G      -   5        $ FriendRequest��5G      $��10 10 10 0 q�61   #��[  5H      $��Friend requests��5I      
������ 
�������
���   �5M      �� 5O      -	   5        $ GameHistBut��5O      $��10 10 10 0 q�6/   #��[  5P      $��Game History��5Q      
������ 
�������
���   �5U      �� 5W      -
   5        $ ExitBut��5W      $��10 10 10 0 q�6+   #��[  5X      $
��Exit��5Y      
������ 
�������
���   �5]      5_      5`      �� 5b   
   -   5        $ UserInfoBorder��5b      $��1��5c      $	��190��6   $	��340��6)   
�������$ 6��5d   
   $
��Left=�6   $��20 0 0 0q�66    5e        5f      $ 15Q�6#   $	 -90Q�63   $ Quality=�5g      $	 2Q�6&   $

 Gray��66   ._�5j      �� -   5        $ UserInfoPanel��5j      .+�5k      �� -   5        $��LoggedUserBlock��5k      $��Logged user��64   $��Center=�6G   $��22(�5l      $
��Bold"�6*   $��10q�6<   �� 5n       �� Orientation$ 
Horizontal=�6   .+�5o      �� -   5        $��	LoginName��5o       $��Loggin: ��63   $
��Left=�6C   $��14(�5p      $��	10 0  0 0q�6+   �� 5r      -   5        $��	LoginText��5r       $��Center=�62   $��
user login��6M   $��14(�6`   5s      �� 5v      $ 
Horizontal=�6   $��0 10 0 0q�66   .+�5w      �� -   5        $��WonName��5w       $��Wins : ��61   $
��Left=�6@   $��14(�5x      $��10 0 0 0q�6+   �� 5z      -   5        $��WonsText��5z       $��Center=�61   $��wons amount��6L   $��14(�6`   5{      �� 5}      $ 
Horizontal=�6   $��0 10 0 0q�66   .+�5~      �� -   5        $��LoseName��5~       $��Loses: ��62   $
��Left=�6A   $��14(�5      $��10 0 0 0q�6+   �� 5�      -   5        $��	LosesText��5�       $��Center=�62   $��loses amount��6M   $��14(�6b   5�      �� 5�      $ 
Horizontal=�6   $��0 10 0 0q�66   .+�5�      �� -   5        $��DrawName��5�       $��Draws: ��62   $
��Left=�6A   $��14(�5�      $��10 0 0 0q�6+   �� 5�      -   5        $��	DrawsText��5�       $��Center=�62   $��loses amount��6M   $��14(�6b   5�      �� 5�      $ 
Horizontal=�6   $��0 10 0 0q�66   .+�5�      �� -   5        $��	TotalName��5�       $��TotalGames: ��61   $
��Left=�6E   $��14(�5�      $��10 0 0 0q�6+   �� 5�      -   5        $��TotalGamesText��5�       $��Center=�67   $��games amount��6R   $��14(�6g   5�      5�      5�      5�      5�      