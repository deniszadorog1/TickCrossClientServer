   M S B A M L   `   `   `  ����J  FTickCrossClient, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null=#clr-namespace:TickCrossClient.PagesTickCrossClient.Pages  5      &     TickCrossClient.Pages.SetPlayers   5      W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5      R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      5local#clr-namespace:TickCrossClient.Pages     5      _ [MaterialDesignThemes.Wpf, Version=5.2.1.0, Culture=neutral, PublicKeyToken=df2a72020bd7962aFmaterialDesign1http://materialdesigninxaml.net/winfx/xaml/themes  5        N� 
Background
  �����$	��500��5	      $	��275��6   $	��500��5
      $	��275��6    N� Title$ 
SetPlayers��5      .��5      �  � RowDefinitions 5   
   �� 5      $��75 �6   �� 5      .+�5   
   �� $��Choose your enemy��6   $��Center=�6-   $��Center=�6J   $��26(�6e   $
��Bold"�6s   � 5   
   $��1��6    � ColumnDefinitions 5      �� 5      -   5         �Name$ 
ListBoxDef��5   #   �� 5      .+�5      ��  �� Orientation$ Vertical=�6   $��10q�60   .+�5      �� -   5        $��ChosenEnemyBlock��5      $��Enemy Login:��65   �� 5      -   5         ��Name$ EnemiesList��5      $��Visible=�6/   $	��160��6b   $	��200��6o   $
��Left=�6{   5       � 5"      $��1��6   .+�5#      �� $ Vertical=�6   $��10q�64   .+�5$      �� $��Enemys sign:��6!   �� 5&      -   5        $��EnemySignBox��5'   <   $��30��5&      $	��100��6+   $
��Left=�67   $	��Top=�6R   $��0 5 0 0q�6j   )  #MaterialDesignThemes.Wpf.HintAssist   Hint$ X��5'      .@�5(      �� .��6'   ⨉5*      �� 5+      .��6'   O5-      5.      �� 50      -   5         ��Name$ SendREquestBut��50      $��
10 10 10 0q�66    #  MaterialDesignRaisedLightButton#��[  51      $��30��52      $��Send request��53      
������ 
�������
���   �57      �� 59      -   5        $ UpdatePageBut��59      $��	10 5 10 0q�65   #��[  5:      $��30��5;      $��Update page��5<      
������ 
�������
���   �5@      �� 5B      -   5        $ BackBut��5B      $��	10 5 10 0q�6/   #��[  5C      $��30��5D      $
��Back��5E      
������ 
�������
���   �5I      5J      5K      5L      5O      5P      