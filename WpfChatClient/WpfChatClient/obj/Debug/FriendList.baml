   M S B A M L   `   `   `  ����H  DWpfChatClient, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null-clr-namespace:WpfChatClientWpfChatClient  5          WpfChatClient.FriendList   5      \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      -localclr-namespace:WpfChatClient     5        N� Title$  
FriendList��5	      .��5      � .+�5   
   �� -   5         ��Name$ sel_friend_list��5   )   $��28,33,533,92q�6    �� View 5      �� 5      .P�5      �� -   5        sel��$
��Name��5   2    �� Width$	 120��6@    �� DisplayMemberBinding 6L   �� *	friend_id+5      5      �� 5   
   -   5         ��Name$ 
sel_friend��5      $
��Left=�6&   $��23��6A   $��
284,66,0,0q�6M    �� TextWrapping$
 Wrap=�6a   $	��Top=�6�   $	��120��6�   ��6u   �� *SelectedItem.friend_id+ �� ElementName$ sel_friend_list��6�   �� 5   
   -   5         ��Name$ invite_friend��5      $��초대��6(   $
��Left=�65   $��329,106,0,0q�6P   $	��Top=�6e   $��75��6}   6�   5      5      